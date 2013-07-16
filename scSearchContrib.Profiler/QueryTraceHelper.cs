using System.Web;

namespace scSearchContrib.Profiler
{
	public class QueryTraceHelper
	{
		public static QueryTraceScope GetQueryTraceScope(Sitecore.Search.QueryBase query)
		{
			var queryContext = new QueryTraceScopeContext(query);
			return GetQueryTraceScope(queryContext);	
		}

		public static QueryTraceScope GetQueryTraceScope(Lucene.Net.Search.Query query)
		{
			var queryContext = new QueryTraceScopeContext(query);
			return GetQueryTraceScope(queryContext);
		}

		public static QueryTraceScope GetQueryTraceScope(QueryTraceScopeContext context)
		{
			var provider = new QueryTraceScopeSessionProvider(HttpContext.Current);
			return new QueryTraceScope(context, provider);
		}
	}
}