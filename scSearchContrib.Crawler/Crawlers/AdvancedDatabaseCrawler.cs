#region Usings

using System;
using System.Collections.Generic;
using System.Xml;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Lucene.Net.Documents;
using Sitecore.Reflection;
using Sitecore.Search;
using Sitecore.Search.Crawlers;
using Sitecore.Xml;
using System.Linq;
using scSearchContrib.Crawler.DynamicFields;
using scSearchContrib.Crawler.FieldCrawlers;
using scSearchContrib.Searcher.Utilities;
using SCField = Sitecore.Data.Fields.Field;
using LuceneField = Lucene.Net.Documents.Field;

#endregion

namespace scSearchContrib.Crawler.Crawlers
{
    public class AdvancedDatabaseCrawler : DatabaseCrawler
    {
        #region Fields

        private List<BaseDynamicField> _dynamicFields = new List<BaseDynamicField>();
        private SafeDictionary<string, string> _fieldCrawlers = new SafeDictionary<string, string>();
        private readonly SafeDictionary<string, bool> _fieldFilter = new SafeDictionary<string, bool>();
        private SafeDictionary<string, SearchField> _fieldTypes = new SafeDictionary<string, SearchField>();
        private bool _hasFieldExcludes;
        private bool _hasFieldIncludes;
        private bool _hasFieldTypeExcludes;

        #endregion

        #region Override Methods

        // checking if item has template
        protected override void IndexVersion(Item item, Item latestVersion, IndexUpdateContext context)
        {
            if (item.Template != null)
            {
                base.IndexVersion(item, latestVersion, context);
            }

            else
            {
                Log.Warn(string.Format("AdvancedDatabaseCrawler: Cannot update item version. Reason: Template is NULL in item '{0}'.", item.Paths.FullPath), this);
            }
        }

        protected override void AddAllFields(Document document, Item item, bool versionSpecific)
        {
            Assert.ArgumentNotNull(document, "document");
            Assert.ArgumentNotNull(item, "item");

            var fields = FilteredFields(item);

            foreach (var field in fields)
            {
                ProcessField(field, document);
            }

            ProcessDynamicFields(document, item);
        }

        protected virtual void ProcessField(SCField field, Document document)
        {
            var values = ExtendedFieldCrawlerFactory.GetFieldCrawlerValues(field, FieldCrawlers);
            foreach (var value in values)
            {
                ProcessFieldValue(field, document, value);
            }
        }

        protected virtual void ProcessFieldValue(SCField field, Document document, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            var indexType = GetIndexType(field);
            var storageType = GetStorageType(field);
            var vectorType = GetVectorType(field);

            value = IdHelper.ProcessGUIDs(value);
            ProcessField(document, field.Key, value, storageType, indexType, vectorType);

            if (indexType == LuceneField.Index.TOKENIZED)
            {
                ProcessField(document, BuiltinFields.Content, value, LuceneField.Store.NO, LuceneField.Index.TOKENIZED);
            }
        }

        #endregion

        #region Config Methods

        public virtual void AddDynamicFields(XmlNode configNode)
        {
            Assert.ArgumentNotNull(configNode, "configNode");
            var type = XmlUtil.GetAttribute("type", configNode);
            var fieldName = XmlUtil.GetAttribute("name", configNode);
            var storageType = XmlUtil.GetAttribute("storageType", configNode);
            var indexType = XmlUtil.GetAttribute("indexType", configNode);
            var vectorType = XmlUtil.GetAttribute("vectorType", configNode);
            var boost = XmlUtil.GetAttribute("boost", configNode);
            var field = ReflectionUtil.CreateObject(type);

            if (field == null || !(field is BaseDynamicField)) return;

            var dynamicField = field as BaseDynamicField;

            dynamicField.SetStorageType(storageType);
            dynamicField.SetIndexType(indexType);
            dynamicField.SetVectorType(vectorType);
            dynamicField.SetBoost(boost);
            dynamicField.FieldKey = fieldName.ToLowerInvariant();
            DynamicFields.Add(dynamicField);
        }

        public virtual void AddFieldCrawlers(XmlNode configNode)
        {
            Assert.ArgumentNotNull(configNode, "configNode");
            var type = XmlUtil.GetAttribute("type", configNode);
            var fieldType = XmlUtil.GetAttribute("fieldType", configNode);
            FieldCrawlers.Add(fieldType.ToLowerInvariant(), type);
        }

        public virtual void AddFieldTypes(XmlNode configNode)
        {
            Assert.ArgumentNotNull(configNode, "configNode");
            var fieldName = XmlUtil.GetAttribute("name", configNode);
            var storageType = XmlUtil.GetAttribute("storageType", configNode);
            var indexType = XmlUtil.GetAttribute("indexType", configNode);
            var vectorType = XmlUtil.GetAttribute("vectorType", configNode);
            var boost = XmlUtil.GetAttribute("boost", configNode);
            var exclude = XmlUtil.GetAttribute("exclude", configNode);
            var searchField = new SearchField(storageType, indexType, vectorType, boost, exclude);
            FieldTypes.Add(fieldName.ToLowerInvariant(), searchField);
            if (searchField.Exclude)
            {
                _hasFieldTypeExcludes = true;
            }
        }

        #endregion

        #region Helper Methods

        protected AbstractField CreateField(string name, string value, LuceneField.Store storageType, LuceneField.Index indexType, LuceneField.TermVector vectorType, float boost)
        {
            var field = new LuceneField(name, value, storageType, indexType, vectorType);
            field.SetBoost(boost);
            return field;
        }

        public void ExcludeField(string value)
        {
            Assert.ArgumentNotNullOrEmpty(value, "fieldId");
            Assert.IsTrue(ID.IsID(value), "fieldId parameter is not a valid GUID");
            _hasFieldExcludes = true;
            _fieldFilter[value] = false;
        }

        public void IncludeField(string value)
        {
            Assert.ArgumentNotNullOrEmpty(value, "fieldId");
            Assert.IsTrue(ID.IsID(value), "fieldId parameter is not a valid GUID");
            _hasFieldIncludes = true;
            _fieldFilter[value] = true;
        }

        protected virtual List<SCField> FilteredFields(Item item)
        {
            var filteredFields = new List<SCField>();
            if (IndexAllFields)
            {
                item.Fields.ReadAll();
                filteredFields.AddRange(item.Fields);
            }
            else if (HasFieldIncludes)
            {
                foreach (var includeFieldId in from p in FieldFilter where p.Value select p)
                {
                    filteredFields.Add(item.Fields[ID.Parse(includeFieldId.Key)]);
                }
            }
            if (HasFieldExcludes || HasFieldTypeExcludes)
            {
                foreach (SCField field in item.Fields)
                {
                    var fieldKey = field.ID.ToString();
                    if (!(!FieldFilter.ContainsKey(fieldKey) ? true : FieldFilter[fieldKey]))
                    {
                        filteredFields.Remove(field);
                    }
                    else
                    {
                        object searchField = FieldTypes[field.TypeKey];
                        if (searchField is SearchField && ((SearchField)searchField).Exclude)
                        {
                            filteredFields.Remove(field);
                        }
                    }
                }
            }

            return filteredFields.Where(f => !String.IsNullOrEmpty(f.Key)).ToList();
        }

        protected LuceneField.Index GetIndexType(SCField field)
        {
            if (FieldTypes.ContainsKey(field.TypeKey))
            {
                object searchField = FieldTypes[field.TypeKey];
                if (searchField is SearchField)
                {
                    return (searchField as SearchField).IndexType;
                }
            }
            return LuceneField.Index.UN_TOKENIZED;
        }

        protected LuceneField.Store GetStorageType(SCField field)
        {
            if (FieldTypes.ContainsKey(field.TypeKey))
            {
                var searchField = FieldTypes[field.TypeKey];
                return searchField.StorageType;
            }
            return LuceneField.Store.NO;
        }

        protected LuceneField.TermVector GetVectorType(SCField field)
        {
            if (FieldTypes.ContainsKey(field.TypeKey))
            {
                var searchField = FieldTypes[field.TypeKey];
                return searchField.VectorType;
            }
            return LuceneField.TermVector.NO;
        }

        protected virtual void ProcessDynamicFields(Document document, Item item)
        {
            foreach (var dynamicField in DynamicFields)
            {
                var fieldValue = dynamicField.ResolveValue(item);

                if (fieldValue != null)
                {
                    ProcessField(document, dynamicField.FieldKey, fieldValue, dynamicField.StorageType,
                                 dynamicField.IndexType, dynamicField.VectorType, dynamicField.Boost);
                }
            }
        }

        protected virtual void ProcessField(Document doc, string fieldKey, string fieldValue, LuceneField.Store storage, LuceneField.Index index)
        {
            ProcessField(doc, fieldKey, fieldValue, storage, index, LuceneField.TermVector.NO);
        }

        protected virtual void ProcessField(Document doc, string fieldKey, string fieldValue, LuceneField.Store storage, LuceneField.Index index, LuceneField.TermVector vector)
        {
            ProcessField(doc, fieldKey, fieldValue, storage, index, vector, 1f);
        }

        protected virtual void ProcessField(Document doc, string fieldKey, string fieldValue, LuceneField.Store storage, LuceneField.Index index, LuceneField.TermVector vector, float boost)
        {
            if ((!String.IsNullOrEmpty(fieldKey) && !String.IsNullOrEmpty(fieldValue))
               && (index != LuceneField.Index.NO || storage != LuceneField.Store.NO))
            {
                doc.Add(CreateField(fieldKey, fieldValue, storage, index, vector, boost));
            }
        }

        #endregion

        #region Properties

        protected List<BaseDynamicField> DynamicFields
        {
            get
            {
                return _dynamicFields;
            }
        }

        protected SafeDictionary<string, string> FieldCrawlers
        {
            get
            {
                return _fieldCrawlers;
            }
        }

        protected SafeDictionary<string, bool> FieldFilter
        {
            get
            {
                return _fieldFilter;
            }
        }

        protected SafeDictionary<string, SearchField> FieldTypes
        {
            get
            {
                return _fieldTypes;
            }
        }

        protected bool HasFieldExcludes
        {
            get
            {
                return _hasFieldExcludes;
            }
            set
            {
                _hasFieldExcludes = value;
            }
        }

        protected bool HasFieldIncludes
        {
            get
            {
                return _hasFieldIncludes;
            }
            set
            {
                _hasFieldIncludes = value;
            }
        }

        protected bool HasFieldTypeExcludes
        {
            get
            {
                return _hasFieldTypeExcludes;
            }
            set
            {
                _hasFieldTypeExcludes = value;
            }
        }

        #endregion
    }
}