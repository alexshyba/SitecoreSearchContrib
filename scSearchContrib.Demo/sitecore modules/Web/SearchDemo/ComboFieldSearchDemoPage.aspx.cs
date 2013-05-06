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
                                                  string locationFilter,
                                                  string fullTextQuery)
        {

            //var searchParam1 = new FieldSearchParam
            //{
            //    FieldName = "standard",
            //    FieldValue = "cdma",
            //    Partial = false,
            //    Condition = QueryOccurance.Must
            //};

            //var searchParam2 = new SearchParam
            //{
            //    TemplateIds = "{1370EB95-810A-4DEA-9C1C-C8B33E40EA35}",
            //    Condition = QueryOccurance.Must
            //};

            //var searchParam3 = new FieldSearchParam
            //                       {
            //                           FieldName = "brand",
            //                           FieldValue = "{11438547-9702-4137-A610-9E1A5A2B4CE3}",
            //                           Condition = QueryOccurance.Must
            //                       };

            var searchParam1 = new SearchParam
            {
                Database = databaseName,
                Language = language,
                TemplateIds = templateFilter,
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
                return runner.GetItems(new SearchParam[] { searchParam1, searchParam2, searchParam3 });
            }
        }
    }
}