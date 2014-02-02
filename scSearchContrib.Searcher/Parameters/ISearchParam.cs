using System.Linq;

namespace scSearchContrib.Searcher.Parameters
{
    using Sitecore.Search;

    public interface ISearchParam
    {
        IQueryable<SkinnyItem> ProcessQuery(IQueryable<SkinnyItem> query, QueryOccurance condition, Index index);
    }
}