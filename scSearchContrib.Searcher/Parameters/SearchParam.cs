using System;
using System.Linq;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Sitecore;
using Sitecore.Data;
using Sitecore.Search;
using Sitecore.StringExtensions;
using scSearchContrib.Searcher.Utilities;

namespace scSearchContrib.Searcher.Parameters
{
    public class SearchParam : ISearchParam
    {
        private string _database;
        private string _language;

        public string RelatedIds { get; set; }
        public string TemplateIds { get; set; }
        public bool SearchBaseTemplates { get; set; }
        public string LocationIds { get; set; }
        public string FullTextQuery { get; set; }

        public string Language
        {
            get
            {
                return _language.IsNullOrEmpty() ? Context.Language.Name : _language;
            }

            set { _language = value; }
        }

        public string Database
        {
            get
            {
                return _database.IsNullOrEmpty() ? SearchHelper.ContextDB.Name : _database;
            }

            set { _database = value; }
        }

        public QueryOccurance Condition { get; set; }

        public SearchParam()
        {
            Condition = QueryOccurance.Must;
        }

        public virtual BooleanQuery ProcessQuery(QueryOccurance occurance, Index index)
        {
            var innerQuery = new CombinedQuery();
            ApplyFullTextClause(innerQuery, FullTextQuery, occurance);
            ApplyRelationFilter(innerQuery, RelatedIds, occurance);
            ApplyTemplateFilter(innerQuery, TemplateIds, occurance);
            ApplyLocationFilter(innerQuery, LocationIds, occurance);
            AddFieldValueClause(innerQuery, BuiltinFields.Database, Database, occurance);

            if (innerQuery.Clauses.Count < 1)
                return null;

            var translator = new QueryTranslator(index);
            var booleanQuery = translator.ConvertCombinedQuery(innerQuery);

            ApplyLanguageClause(booleanQuery, Language, translator.GetOccur(occurance));

            return booleanQuery;
        }

        protected void ApplyLanguageClause(BooleanQuery query, string language, BooleanClause.Occur occurance)
        {
            if (String.IsNullOrEmpty(language)) return;

            var phraseQuery = new PhraseQuery();
            phraseQuery.Add(new Term(BuiltinFields.Language, language.ToLowerInvariant()));

            query.Add(phraseQuery, occurance);
        }

        protected void ApplyFullTextClause(CombinedQuery query, string searchText, QueryOccurance occurance)
        {
            if (String.IsNullOrEmpty(searchText)) return;

            query.Add(new FullTextQuery(searchText), occurance);
        }

        protected void ApplyIdFilter(CombinedQuery query, string fieldName, string filter, QueryOccurance occurance)
        {
            if (String.IsNullOrEmpty(fieldName) || String.IsNullOrEmpty(filter)) return;

            var filterQuery = new CombinedQuery();

            var values = IdHelper.ParseId(filter);

            foreach (var value in values.Where(ID.IsID))
            {
                AddFieldValueClause(filterQuery, fieldName, value, QueryOccurance.Should);
            }

            query.Add(filterQuery, occurance);
        }

        protected void ApplyTemplateFilter(CombinedQuery query, string templateIds, QueryOccurance occurance)
        {
            if (String.IsNullOrEmpty(templateIds)) return;

            string field = BuiltinFields.Template;
            if (SearchBaseTemplates)
            {
                field = BuiltinFields.AllTemplates;
            }
            ApplyIdFilter(query, field, templateIds, occurance);
        }

        protected void ApplyLocationFilter(CombinedQuery query, string locationIds, QueryOccurance occurance)
        {
            ApplyIdFilter(query, BuiltinFields.Path, locationIds, occurance);
        }

        protected void ApplyRelationFilter(CombinedQuery query, string ids, QueryOccurance occurance)
        {
            ApplyIdFilter(query, BuiltinFields.Links, ids, occurance);
        }

        protected void AddFieldValueClause(CombinedQuery query, string fieldName, string fieldValue, QueryOccurance occurance)
        {
            if (String.IsNullOrEmpty(fieldName) || String.IsNullOrEmpty(fieldValue)) return;

            // if we are searching by _id field, do not lowercase
            fieldValue = IdHelper.ProcessGUIDs(fieldValue);
            query.Add(new FieldQuery(fieldName.ToLowerInvariant(), fieldValue), occurance);
        }
    }
}