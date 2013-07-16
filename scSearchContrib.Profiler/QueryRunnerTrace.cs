using System.Collections.Generic;
using scSearchContrib.Searcher;

namespace scSearchContrib.Profiler
{
	public class QueryRunnerTrace : QueryRunner
	{
		public QueryRunnerTrace(string indexId) : base(indexId) { }

		public override List<SkinnyItem> RunQuery(Lucene.Net.Search.Query query, bool showAllVersions = false, string sortField = "", bool reverse = true, int start = 0, int end = 0)
		{
			using (var scope = QueryTraceHelper.GetQueryTraceScope(query))
			{
				return base.RunQuery(query, showAllVersions, sortField, reverse, start, end);	
			}
		}

		public override List<SkinnyItem> RunQuery(Lucene.Net.Search.Query query, bool showAllVersions, string sortField, bool reverse, int start, int end, out int totalResults)
		{
			using (var scope = QueryTraceHelper.GetQueryTraceScope(query))
			{
				return base.RunQuery(query, showAllVersions, sortField, reverse, start, end, out totalResults);
			}
		}

		public override List<SkinnyItem> RunQuery(Sitecore.Search.QueryBase query, bool showAllVersions)
		{
			using (var scope = QueryTraceHelper.GetQueryTraceScope(query))
			{
				return base.RunQuery(query, showAllVersions);
			}
		}
	}
}