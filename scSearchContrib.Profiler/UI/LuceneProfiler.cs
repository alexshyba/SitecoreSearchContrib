using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace scSearchContrib.Profiler.UI
{
	public class LuceneProfiler : WebControl, INamingContainer
	{
		protected LinkButton btnClear;
		private QueryTraceScopeSessionProvider Provider { get { return new QueryTraceScopeSessionProvider(HttpContext.Current); } }

		protected override void CreateChildControls()
		{
			btnClear = new LinkButton { ID = "btnClear", Text = "clear", CssClass = "traceButton", CausesValidation = false};
			btnClear.Click += (btnClear_Click);
			Controls.Add(btnClear);
		}

		void btnClear_Click(object sender, EventArgs e)
		{
			Provider.Clear();
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			IncludeJavascript();
			IncludeCss();
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Id, "traceContainer");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			var linkToggle = new HtmlAnchor {HRef = "#", ID = "lnkToggleView", InnerText = "toggle"};
			linkToggle.RenderControl(writer);

			btnClear.RenderControl(writer);
			
			const string contentPlaceholder = @"<table id='traceTable'><td><b>Start</b></td><td><b>Duration</b></td><td><b>Query</b></td></table>";
			writer.Write(contentPlaceholder);

			writer.Write(GetTraceTemplate());
			writer.Write(GetTraceAsJson());
			writer.Write(GetToggleJavascript());

			writer.RenderEndTag();		// div

			base.RenderContents(writer);
		}

		private void IncludeJavascript()
		{
			var type = typeof(LuceneProfiler);
			Page.ClientScript.RegisterClientScriptInclude(type, "jquery-1.4.2", "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js");
			Page.ClientScript.RegisterClientScriptResource(type, "scSearchContrib.Profiler.UI.Resources.LuceneProfiler.js");	// embedded resourece
			Page.ClientScript.RegisterClientScriptInclude(type, "jquery.templates", "http://ajax.microsoft.com/ajax/jquery.templates/beta1/jquery.tmpl.min.js");
		}

		private void IncludeCss()
		{
			string cssTemplate = "<link rel='stylesheet' text='text/css' href='{0}' />";
			string cssLocation = Page.ClientScript.GetWebResourceUrl(this.GetType(), "scSearchContrib.Profiler.UI.Resources.LuceneProfiler.css");	// embedded resourece
			var include = new LiteralControl(String.Format(cssTemplate, cssLocation));

			if (Page.Header != null)
			{
				(Page.Header).Controls.Add(include);
			}
			else
			{
				Controls.Add(include);
			}
		}

		private static string GetTraceTemplate()
		{
			string template = string.Empty;
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("scSearchContrib.Profiler.UI.Resources.TraceTemplate.html"))
				if (stream != null)
				{
					using (var reader = new StreamReader(stream))
					{
						template = reader.ReadToEnd();
					}
				}
			return template;
		}

		private static string GetToggleJavascript()
		{
			var toggleWireup = "$j('#lnkToggleView').click(function() { profiler.toggle('#traceTable'); });";
			toggleWireup = string.Format("<script>{0}</script>", toggleWireup);
			return toggleWireup;
		}

		private string GetTraceAsJson()
		{
			string dataJson = string.Format("var data = {0};", Provider.GetQueryTraceAsJson());
			dataJson += "profiler.load(data);";
			dataJson = string.Format("<script>{0}</script>", dataJson);
			return dataJson;
		}
	}
}