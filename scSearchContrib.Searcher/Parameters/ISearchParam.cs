namespace scSearchContrib.Searcher.Parameters
{
    using Lucene.Net.Search;

    using Sitecore.Search;

    public interface ISearchParam
    {
        Query ProcessQuery(QueryOccurance condition, Index index);
    }
}