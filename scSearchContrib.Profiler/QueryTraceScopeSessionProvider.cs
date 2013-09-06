using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;

namespace scSearchContrib.Profiler
{
	public class QueryTraceScopeSessionProvider : IQueryTraceScopeProvider
	{
		public HttpContext Context { get; private set; }
		private const string SessionKey = "QueryTraceScopeSessionProvider";

		public QueryTraceScopeSessionProvider(HttpContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			Context = context;
		}

		public bool SaveQueryTrace(QueryTraceScopeContext queryContext)
		{
			var trace = GetQueryTrace();

			bool isTraceValid = trace != null;
			if (isTraceValid)
			{
				trace.Add(queryContext);
				Context.Session[SessionKey] = trace;
			}

			return isTraceValid;
		}

		public string GetQueryTraceAsJson()
		{
			var trace = GetQueryTrace();

			var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
			return serializer.Serialize(trace);
		}

		public List<QueryTraceScopeContext> GetQueryTrace()
		{			
			bool isSessionValid = Context.Session[SessionKey] as List<QueryTraceScopeContext> != null;
			var results = isSessionValid ? (Context.Session[SessionKey] as List<QueryTraceScopeContext>) : new List<QueryTraceScopeContext>();
			return results.OrderByDescending(r => r.ExecutionStartTime).ToList();
		}

		public void Clear()
		{
			Context.Session.Remove(SessionKey);
		}
	}
}