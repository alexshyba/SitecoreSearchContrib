using System.Web.UI.WebControls;
using SearchDemo.Scripts;
using scSearchContrib.Searcher;
using scSearchContrib.Searcher.Parameters;
using System.Collections.Generic;

namespace scSearchContrib.Demo
{
    public partial class RelationSearchDemoPage : BaseDemoPage
    {
        protected string RelationFilter
        {
            get { return RelatedIdsTextBox.Text; }
        }

        public override List<SkinnyItem> GetItems(string databaseName,
                                                  string indexName,
                                                  string language,
                                                  string templateFilter,
                                                  string locationFilter,
                                                  string fullTextQuery)
        {
            var searchParam = new SearchParam
            {
                Database = databaseName,
                Language = language,
                RelatedIds = RelationFilter,
                TemplateIds = templateFilter,
                LocationIds = locationFilter,
                FullTextQuery = fullTextQuery
            };

            using (var runner = new QueryRunner(indexName))
            {
                return runner.GetItems(searchParam);
            }
        }
    }
}