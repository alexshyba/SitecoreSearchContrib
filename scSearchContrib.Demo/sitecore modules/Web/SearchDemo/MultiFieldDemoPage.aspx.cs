using SearchDemo.Scripts;
using scSearchContrib.Searcher;
using scSearchContrib.Searcher.Parameters;
using System.Collections.Generic;
using Sitecore.StringExtensions;

namespace scSearchContrib.Demo
{
    public partial class MultiFieldDemoPage : BaseDemoPage
    {
        protected List<MultiFieldSearchParam.Refinement> GetRefinements()
        {
            var refinements = new List<MultiFieldSearchParam.Refinement>();

            if (!FieldName1TextBox.Text.IsNullOrEmpty() && !FieldValue1TextBox.Text.IsNullOrEmpty())
                refinements.Add(new MultiFieldSearchParam.Refinement(FieldName1TextBox.Text, FieldValue1TextBox.Text));

            if (!FieldName2TextBox.Text.IsNullOrEmpty() && !FieldValue2TextBox.Text.IsNullOrEmpty())
                refinements.Add(new MultiFieldSearchParam.Refinement(FieldName2TextBox.Text, FieldValue2TextBox.Text));

            return refinements;
        }

        public override List<SkinnyItem> GetItems(string databaseName,
                                                  string indexName,
                                                  string language,
                                                  string templateFilter,
                                                  bool searchBaseTemplates,
                                                  string locationFilter,
                                                  string fullTextQuery)
        {
            var refinements = GetRefinements();

            var searchParam = new MultiFieldSearchParam
                                 {
                                     Database = databaseName,
                                     Refinements = refinements,
                                     InnerCondition = GetCondition(ConditionList),
                                     LocationIds = locationFilter,
                                     TemplateIds = templateFilter,
                                     SearchBaseTemplates = searchBaseTemplates,
                                     FullTextQuery = fullTextQuery,
                                     Language = language
                                 };

            using (var runner = new QueryRunner(indexName))
            {
                return runner.GetItems(searchParam);
            }
        }
    }
}