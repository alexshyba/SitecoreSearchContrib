namespace scSearchContrib.Searcher.Parameters
{
    using Lucene.Net.Search;

    using scSearchContrib.Searcher.Utilities;

    using Sitecore.Diagnostics;
    using Sitecore.Search;

    /// <summary>
    /// Search parameter implementation allowing for a set of nested parameters
    /// </summary>
    public abstract class MultiSearchParam : SearchParam
    {
        public QueryOccurance InnerCondition { get; set; }

        public override Query ProcessQuery(QueryOccurance condition, Index index)
        {
            Assert.ArgumentNotNull(index, "Index");

            var translator = new QueryTranslator(index);
            Assert.IsNotNull(translator, "Query Translator");

            var baseQuery = base.ProcessQuery(condition, index) as BooleanQuery ?? new BooleanQuery();

            var outerCondition = translator.GetOccur(condition);

            var query = this.BuildQuery(translator.GetOccur(InnerCondition));
            if (query != null)
            {
                baseQuery.Add(query, outerCondition);
            }

            return baseQuery;
        }

        protected abstract Query BuildQuery(BooleanClause.Occur innerCondition);
    }
}
