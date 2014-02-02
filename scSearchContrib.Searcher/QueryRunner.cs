using Sitecore.ContentSearch;

namespace scSearchContrib.Searcher
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using scSearchContrib.Searcher.Parameters;
    using Sitecore.Search;

    public class QueryRunner : IDisposable
    {
        #region ctor

        public QueryRunner(string indexId)
        {
            Index = indexId;
            UsePreparedQuery = false;
        }

        /// <summary>
        /// Allows use of a PreparedQuery, which bypasses some of Sitecore.Search's query rewriting and prevents it from translating Term queries into Prefix queries.
        /// </summary>
        /// <param name="indexId"></param>
        /// <param name="usePreparedQuery"></param>
        public QueryRunner(string indexId, bool usePreparedQuery)
        {
            Index = indexId;
            UsePreparedQuery = usePreparedQuery;
        }

        #endregion ctor

        #region Properties

        public string Index { get; set; }

        public bool UsePreparedQuery { get; set; }

        #endregion Properties

        #region static scope

        #endregion static scope

        #region Query Runner Methods

        #endregion

        #region Searching Methods

        public virtual List<SkinnyItem> GetItems(ISearchParam param, QueryOccurance innerCondition = QueryOccurance.Must, bool showAllVersions = false, string sortField = "", bool reverse = true, int start = 0, int end = 0)
        {
            var contentSearchIndex = ContentSearchManager.GetIndex(Index);
            using (var searchContext = contentSearchIndex.CreateSearchContext())
            {
                var query = searchContext.GetQueryable<SkinnyItem>();
                query = param.ProcessQuery(query, innerCondition, null);
                return query.ToList();
            }
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
