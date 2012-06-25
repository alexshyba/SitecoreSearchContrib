#region usings

using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Search;
using Sitecore.Diagnostics;
using Sitecore.Search;
using Sitecore.StringExtensions;
using scSearchContrib.Searcher.Parameters;
using scSearchContrib.Searcher.Utilities;

#endregion

namespace scSearchContrib.Searcher
{
    public class QueryRunner : IDisposable
    {
        #region ctor

        public QueryRunner(string indexId)
        {
            Index = SearchManager.GetIndex(indexId);
        }

        #endregion ctor

        #region Properties

        public Index Index { get; set; }

        #endregion Properties

        #region Query Runner Methods

        public virtual List<SkinnyItem> RunQuery(Query query, bool showAllVersions = false, string sortField = "", bool reverse = true, int start = 0, int end = 0)
        {
            Assert.ArgumentNotNull(Index, "Demo");

            var items = new List<SkinnyItem>();

            try
            {
                using (var context = new IndexSearchContext(Index))
                {
                    SearchHits searchhits;
                    if (!sortField.IsNullOrEmpty())
                    {
                        // type hardcoded to 3 - means sorting as string
                        var sort = new Sort(new SortField(sortField, 3, reverse));
                        var hits = context.Searcher.Search(query, sort);
                        searchhits = new SearchHits(hits);
                    }
                    else
                    {
                        searchhits = context.Search(query);
                    }

                    if (searchhits == null) return null;
                    if (end == 0 || end > searchhits.Length) end = searchhits.Length;
                    var resultCollection = searchhits.FetchResults(start, end);
                    SearchHelper.GetItemsFromSearchResult(resultCollection, items, showAllVersions);
                }
            }
            catch (Exception exception)
            {
                Log.Error("scSearchContrib.Searcher. There was a problem while running a search query. Details: " + exception.Message, this);
                Log.Error(exception.StackTrace, this);
                throw;
            }

            return items;
        }

        public virtual List<SkinnyItem> RunQuery(QueryBase query, bool showAllVersions)
        {
            var translator = new QueryTranslator(Index);
            var luceneQuery = translator.Translate(query);
            return RunQuery(luceneQuery, showAllVersions);
        }

        public virtual List<SkinnyItem> RunQuery(QueryBase query)
        {
            return RunQuery(query, false);
        }

        #endregion

        #region Searching Methods

        public virtual List<SkinnyItem> GetItems(ISearchParam param, QueryOccurance innerOccurance = QueryOccurance.Must, bool showAllVersions = false, string sortField = "", bool reverse = true, int start = 0, int end = 0)
        {
            Assert.IsNotNull(Index, "Index");
            var query = param.ProcessQuery(innerOccurance, Index);
            return RunQuery(query, showAllVersions, sortField, reverse, start, end);
        }

        public virtual List<SkinnyItem> GetItems(IEnumerable<SearchParam> parameters, bool showAllVersions = false, string sortField = "", bool reverse = true, int start = 0, int end = 0)
        {
            Assert.IsNotNull(Index, "Index");

            var translator = new QueryTranslator(Index);
            var query = new BooleanQuery();

            foreach (var parameter in parameters.Where(p => p != null))
            {
                var innerQueryResult = parameter.ProcessQuery(parameter.Condition, Index);
                if (innerQueryResult.GetClauses().Length > 0)
                {
                    var clause = new BooleanClause(innerQueryResult, translator.GetOccur(parameter.Condition));
                    query.Add(clause);
                }
            }

            return RunQuery(query, showAllVersions, sortField, reverse, start, end);
        }

        #endregion

        #region static scope

        /// <summary>
        /// Returns a search index by specified index id
        /// </summary>
        /// <param name="indexId">Search index id</param>
        /// <returns>Search index object</returns>
        public static Index GetIndex(string indexId)
        {
            return SearchManager.GetIndex(indexId);
        }

        #endregion static scope

        #region IDisposable Members

        public void Dispose()
        {
            Index = null;
        }

        #endregion
    }
}
