namespace scSearchContrib.Searcher.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Lucene.Net.Documents;

    using scSearchContrib.Searcher.Parameters;

    using Sitecore;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Search;

    /// <summary>
    /// The search helper.
    /// </summary>
    public class SearchHelper
    {
        /// <summary>
        /// Gets the context database.
        /// </summary>
        public static Database ContextDB
        {
            get { return Context.ContentDatabase ?? Context.Database; }
        }

        /// <summary>
        /// The get item list from information collection.
        /// </summary>
        /// <param name="skinnyItems">
        /// The skinny items.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public static List<Item> GetItemListFromInformationCollection(List<SkinnyItem> skinnyItems)
        {
            return skinnyItems.Select(skinnyItem => skinnyItem.GetItem()).Where(i => i != null).ToList();
        }

        /// <summary>
        /// The get items from search result.
        /// </summary>
        /// <param name="searchResults">
        /// The search results.
        /// </param>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <param name="showAllVersions">
        /// The show all versions.
        /// </param>
        public static void GetItemsFromSearchResult(IEnumerable<SearchResult> searchResults, List<SkinnyItem> items, bool showAllVersions)
        {
            foreach (var result in searchResults)
            {
                var uriField = result.Document.GetField(BuiltinFields.Url);
                if (uriField != null && !string.IsNullOrEmpty(uriField.StringValue()))
                {
                    var itemUri = new ItemUri(uriField.StringValue());
                    itemUri = new ItemUri(itemUri.ItemID, itemUri.Language, Sitecore.Data.Version.Latest, itemUri.DatabaseName);
                    var itemInfo = new SkinnyItem(itemUri);

                    foreach (Field field in result.Document.GetFields())
                    {
                        itemInfo.Fields.Add(field.Name(), field.StringValue());
                    }

                    items.Add(itemInfo);
                }

                if (showAllVersions)
                {
                    GetItemsFromSearchResult(result.Subresults, items, true);
                }
            }
        }

        /// <summary>
        /// The create refinements.
        /// </summary>
        /// <param name="fieldName">
        /// The field name.
        /// </param>
        /// <param name="fieldValue">
        /// The field value.
        /// </param>
        /// <returns>
        /// The IEnumerable of Refinements.
        /// </returns>
        public static IEnumerable<MultiFieldSearchParam.Refinement> CreateRefinements(string fieldName, string fieldValue)
        {
            var refinements = new List<MultiFieldSearchParam.Refinement>();

            if (!string.IsNullOrEmpty(fieldName) &&
                !string.IsNullOrEmpty(fieldValue))
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
