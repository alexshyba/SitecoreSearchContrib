using scSearchContrib.Searcher;
using scSearchContrib.Searcher.Parameters;
using System.Collections.Generic;

namespace scSearchContrib.Demo
{
    using Sitecore.Diagnostics;

    public partial class FieldSearchDemoPage : BaseDemoPage
    {

        public override List<SkinnyItem> GetItems(string databaseName,
                                                  string indexName,
                                                  string language,
                                                  string templateFilter,
                                                  bool searchBaseTemplates,
                                                  string locationFilter,
                                                  string fullTextQuery)
        {
            var searchParam = new FieldSearchParam
            {
                Database = databaseName,
                Language = language,
                FieldName = this.FieldName.Text,
                FieldValue = this.FieldValue.Text,
                TemplateIds = templateFilter,
                SearchBaseTemplates = searchBaseTemplates,
                LocationIds = locationFilter,
                FullTextQuery = fullTextQuery,
                Partial = this.Partial.Checked
            };

            using (var runner = new QueryRunner(indexName))
            {
                return runner.GetItems(searchParam);
            }
        }
    }
}