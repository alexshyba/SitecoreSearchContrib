using Lucene.Net.Search;
using Sitecore.Search;

namespace scSearchContrib.Searcher.Parameters
{
   public interface ISearchParam
   {
      BooleanQuery ProcessQuery(QueryOccurance occurance, Index index);
   }
}