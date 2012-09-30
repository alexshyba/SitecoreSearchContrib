#region Usings

using System.Collections.Generic;
using System;
using System.Linq;
using Lucene.Net.Documents;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Search;
using scSearchContrib.Searcher.Parameters;

#endregion

namespace scSearchContrib.Searcher.Utilities
{
    public class SearchHelper
    {
        public static Database ContextDB
        {
            get { return Context.ContentDatabase ?? Context.Database; }
        }

        public static List<Item> GetItemListFromInformationCollection(List<SkinnyItem> skinnyItems)
        {
            return skinnyItems.Select(skinnyItem => skinnyItem.GetItem()).Where(i => i != null).ToList();
        }

        public static void GetItemsFromSearchResult(IEnumerable<SearchResult> searchResults, List<SkinnyItem> items, bool showAllVersions)
        {
            foreach (var result in searchResults)
            {
                var uriField = result.Document.GetField(BuiltinFields.Url);
                if (uriField != null && !String.IsNullOrEmpty(uriField.StringValue()))
                {
                    var itemUri = new ItemUri(uriField.StringValue());

                    var itemInfo = new SkinnyItem(itemUri);

                    foreach (Field field in result.Document.GetFields())
                    {
                        var fieldName = field.Name();
                        var fieldValue = field.StringValue();

                        //multi-valued fields will return as multiple Fields with the same name
                        if (itemInfo.Values.ContainsKey(fieldName))
                        {
                            var values = itemInfo.Values[fieldName].ToList();
                            values.Add(fieldValue);
                            itemInfo.Values[fieldName] = values;
                        }
                        else
                        {
                            var values = new List<string>();
                            values.Add(fieldValue);
                            itemInfo.Values[fieldName] = values;
                        }
                    }

                    items.Add(itemInfo);
                }

                if (showAllVersions)
                    GetItemsFromSearchResult(result.Subresults, items, true);
            }
        }

        public static IEnumerable<MultiFieldSearchParam.Refinement> CreateRefinements(string fieldName, string fieldValue)
        {
            var refinements = new List<MultiFieldSearchParam.Refinement>();

            if (!String.IsNullOrEmpty(fieldValue) && !String.IsNullOrEmpty(fieldValue))
            {
                if (fieldName.Contains("|"))
                {
                   var fieldNames = fieldName.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                   refinements.AddRange(fieldNames.Select(name => new MultiFieldSearchParam.Refinement(name, fieldValue)));
                }
                else
                {
                    refinements.Add(new MultiFieldSearchParam.Refinement(fieldName, fieldValue));
                }
            }

            return refinements;
        }
    }
}
