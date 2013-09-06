using System.Collections.Generic;

namespace scSearchContrib.Profiler
{
	public interface IQueryTraceScopeProvider
	{
		bool SaveQueryTrace(QueryTraceScopeContext queryContext);
		List<QueryTraceScopeContext> GetQueryTrace();
	}
}