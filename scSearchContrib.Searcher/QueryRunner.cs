namespace scSearchContrib.Searcher
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Lucene.Net.Search;

    using scSearchContrib.Searcher.Parameters;
    using scSearchContrib.Searcher.Utilities;

    using Sitecore.Diagnostics;
    using Sitecore.Search;
    using Sitecore.StringExtensions;

    public class QueryRunner : IDisposable
    {
        #region ctor

        public QueryRunner(string indexId)
        {
            Index = SearchManager.GetIndex(indexId);
            UsePreparedQuery = false;
        }

        /// <summary>
        /// Allows use of a PreparedQuery, which bypasses some of Sitecore.Search's query rewriting and prevents it from translating Term queries into Prefix queries.
        /// </summary>
        /// <param name="indexId"></param>
        /// <param name="usePreparedQuery"></param>
        public QueryRunner(string indexId, bool usePreparedQuery)
        {
            Index = SearchManager.GetIndex(indexId);
            UsePreparedQuery = usePreparedQuery;
        }

        #endregion ctor

        #region Properties

        public Index Index { get; set; }

        public bool UsePreparedQuery { get; set; }

        #endregion Properties

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

        #region Query Runner Methods

        public virtual List<SkinnyItem> RunQuery(Query query, bool showAllVersions, string sortField, bool reverse, int start, int end, out int totalResults)
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
                        if (UsePreparedQuery)
                        {
                            searchhits = context.Search(new PreparedQuery(query));
                        }
                        else
                        {
                            searchhits = context.Search(query);
                        }
                    }

                    if (searchhits == null)
                    {
                        totalResults = 0;
                        return null;
                    }

                    totalResults = searchhits.Length;
                    if (end == 0 || end > searchhits.Length)
                    {
                        end = totalResults;
                    }

                    var resultCollection = searchhits.FetchResults(start, end - start);
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

        public virtual List<SkinnyItem> RunQuery(Query query, bool showAllVersions = false, string sortField = "", bool reverse = true, int start = 0, int end = 0)
        {
            int temp;
            return RunQuery(query, showAllVersions, sortField, reverse, start, end, out temp);
        }

        public virtual List<SkinnyItem> RunQuery(QueryBase query, bool showAllVersions = false)
        {
            var translator = new QueryTranslator(Index);
            var luceneQuery = translator.Translate(query);
            return RunQuery(luceneQuery, showAllVersions);
        }

        #endregion

        #region Searching Methods

        public virtual List<SkinnyItem> GetItems(ISearchParam param, QueryOccurance innerCondition, bool showAllVersions, string sortField, bool reverse, int start, int end, out int totalResults)
        {
            Assert.IsNotNull(Index, "Index");
            var query = param.ProcessQuery(innerCondition, Index);
            return RunQuery(query, showAllVersions, sortField, reverse, start, end, out totalResults);
        }

        public virtual List<SkinnyItem> GetItems(ISearchParam param, QueryOccurance innerCondition = QueryOccurance.Must, bool showAllVersions = false, string sortField = "", bool reverse = true, int start = 0, int end = 0)
        {
            int temp;
            return GetItems(param, innerCondition, showAllVersions, sortField, reverse, start, end, out temp);
        }

        public virtual List<SkinnyItem> GetItems(IEnumerable<SearchParam> parameters, bool showAllVersions, string sortField, bool reverse, int start, int end, out int totalResults)
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

            return RunQuery(query, showAllVersions, sortField, reverse, start, end, out totalResults);
        }

        public virtual List<SkinnyItem> GetItems(IEnumerable<SearchParam> parameters, bool showAllVersions = false, string sortField = "", bool reverse = true, int start = 0, int end = 0)
        {
            int temp;
            return GetItems(parameters, showAllVersions, sortField, reverse, start, end, out temp);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Index = null;
        }

        #endregion
    }
}
