using scSearchContrib.Searcher;
using scSearchContrib.Searcher.Parameters;

namespace scSearchContrib.Demo
{
    using System.Collections.Generic;

    public partial class SortDemoPage : BaseDemoPage
    {
        public bool Reverse { get { return DescendingCheckBox.Checked; } }

        public string SortFieldName { get { return SortByTextBox.Text; } }

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
                                      TemplateIds = templateFilter,
                                      LocationIds = locationFilter,
                                      FullTextQuery = fullTextQuery
                                  };

            using (var runner = new QueryRunner(indexName))
            {
                return runner.GetItems(searchParam, sortField:SortFieldName, reverse:Reverse);
            }
        }
    }
}