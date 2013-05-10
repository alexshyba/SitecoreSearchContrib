using scSearchContrib.Searcher;
using scSearchContrib.Searcher.Parameters;
using System.Collections.Generic;
using Sitecore.StringExtensions;

namespace scSearchContrib.Demo
{
    public partial class NumericRangeDemoPage : BaseDemoPage
   {
      protected List<NumericRangeSearchParam.NumericRangeField> NumericRanges
      {
         get
         {
            var ranges = new List<NumericRangeSearchParam.NumericRangeField>();

            if (!NumericFieldName1TextBox.Text.IsNullOrEmpty() && !NumericStart1TextBox.Text.IsNullOrEmpty() && !NumericEnd1TextBox.Text.IsNullOrEmpty())
               ranges.Add(new NumericRangeSearchParam.NumericRangeField(NumericFieldName1TextBox.Text, long.Parse(NumericStart1TextBox.Text), int.Parse(NumericEnd1TextBox.Text)));

            if (!NumericFieldName2TextBox.Text.IsNullOrEmpty() && !NumericStart2TextBox.Text.IsNullOrEmpty() && !NumericEnd2TextBox.Text.IsNullOrEmpty())
                ranges.Add(new NumericRangeSearchParam.NumericRangeField(NumericFieldName2TextBox.Text, long.Parse(NumericStart2TextBox.Text), int.Parse(NumericEnd2TextBox.Text)));

            return ranges;
         }
      }

      public override List<SkinnyItem> GetItems(string databaseName,
                                                string indexName,
                                                string language,
                                                string templateFilter,
                                                bool searchBaseTemplates,
                                                string locationFilter,
                                                string fullTextQuery)
      {
         var searchParam = new NumericRangeSearchParam
                              {
                                 Database = databaseName,
                                 Ranges = NumericRanges,
                                 LocationIds = locationFilter,
                                 TemplateIds = templateFilter,
                                 SearchBaseTemplates = searchBaseTemplates,
                                 FullTextQuery = fullTextQuery,
                                 InnerCondition = GetCondition(InnerNumericRangeConditionList),
                                 Language = language
                              };

         using (var runner = new QueryRunner(indexName))
         {
            return runner.GetItems(searchParam);
         }
      }
   }
}