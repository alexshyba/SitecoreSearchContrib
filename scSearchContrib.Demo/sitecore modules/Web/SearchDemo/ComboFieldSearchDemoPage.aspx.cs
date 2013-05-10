namespace scSearchContrib.Demo
{
    using System.Collections.Generic;

    using Sitecore.Search;

    using scSearchContrib.Searcher;
    using scSearchContrib.Searcher.Parameters;

    public partial class ComboFieldSearchDemoPage : BaseDemoPage
    {
        public override List<SkinnyItem> GetItems(string databaseName,
                                                  string indexName,
                                                  string language,
                                                  string templateFilter,
                                                  bool searchBaseTemplates,
                                                  string locationFilter,
                                                  string fullTextQuery)
        {
            var searchParam1 = new SearchParam
            {
                Database = databaseName,
                Language = language,
                TemplateIds = templateFilter,
                SearchBaseTemplates = searchBaseTemplates,
                LocationIds = locationFilter,
                FullTextQuery = fullTextQuery,
                Condition = this.GetCondition(this.BaseConditionList)
            };

            var searchParam2 = new FieldSearchParam
            {
                FieldName = this.Field1Name.Text,
                FieldValue = this.Field1Value.Text,
                Partial = this.Partial1.Checked,
                Condition = this.GetCondition(this.FieldSearchParameter1ConditionList)
            };

            var searchParam3 = new FieldSearchParam
            {
                FieldName = this.Field2Name.Text,
                FieldValue = this.Field2Value.Text,
                Partial = this.Partial2.Checked,
                Condition = this.GetCondition(this.FieldSearchParameter2ConditionList)
            };

            using (var runner = new QueryRunner(indexName))
            {
                return runner.GetItems(new [] { searchParam1, searchParam2, searchParam3 });
            }
        }
    }
}