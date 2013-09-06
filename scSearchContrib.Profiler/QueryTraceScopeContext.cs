using System;

namespace scSearchContrib.Profiler
{
	public class QueryTraceScopeContext
	{
		public Guid Id { get { return Guid.NewGuid(); } }
		public string Name { get; private set; }
		public string Query { get; private set; }
		public DateTime ExecutionStartTime { get; set; }
		public DateTime ExecutionEndTime { get; set; }
		public int ExecutionDuration { get { return ExecutionEndTime.Subtract(ExecutionStartTime).Milliseconds; } }
		public string ExecutionStartTimeFormatted { get { return ExecutionStartTime.ToString(); } }
		public string ExecutionEndTimeFormatted { get { return ExecutionEndTime.ToString(); } }

		public QueryTraceScopeContext(Sitecore.Search.QueryBase query)
		{
			Query = query.ToString();
		}

		public QueryTraceScopeContext(Lucene.Net.Search.Query query)
		{
			Query = query.ToString();
		}
	}
}