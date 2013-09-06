using System;

namespace scSearchContrib.Profiler
{
	public class QueryTraceScope : IDisposable
	{
		public QueryTraceScopeContext Context { get; private set; }
		public IQueryTraceScopeProvider Provider { get; private set; }

		public QueryTraceScope(QueryTraceScopeContext context, IQueryTraceScopeProvider provider)
		{
			Context = context;
			Provider = provider;
			
			LogScopeStart();
		}

		protected void LogScopeStart()
		{
			Context.ExecutionStartTime = DateTime.Now;
		}

		protected void LogScopeComplete()
		{
			Context.ExecutionEndTime = DateTime.Now;
			Provider.SaveQueryTrace(Context);
		}

		public void Dispose()
		{
			LogScopeComplete();
		}
	}
}