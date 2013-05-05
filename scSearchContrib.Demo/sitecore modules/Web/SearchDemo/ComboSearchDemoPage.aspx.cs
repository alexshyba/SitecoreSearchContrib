using scSearchContrib.Searcher;
using scSearchContrib.Searcher.Parameters;
using System;
using System.Collections.Generic;
using Sitecore.StringExtensions;

namespace scSearchContrib.Demo
{
    public partial class ComboSearchDemoPage : BaseDemoPage
    {
        protected List<NumericRangeSearchParam.NumericRangeField> NumericRanges
        {
            get
            {
                var ranges = new List<NumericRangeSearchParam.NumericRangeField>();

                if (!NumericFieldName1TextBox.Text.IsNullOrEmpty() && !NumericStart1TextBox.Text.IsNullOrEmpty() && !NumericEnd1TextBox.Text.IsNullOrEmpty())
                    ranges.Add(new NumericRangeSearchParam.NumericRangeField(NumericFieldName1TextBox.Text, int.Parse(NumericStart1TextBox.Text), int.Parse(NumericEnd1TextBox.Text)));

                if (!NumericFieldName2TextBox.Text.IsNullOrEmpty() && !NumericStart2TextBox.Text.IsNullOrEmpty() && !NumericEnd2TextBox.Text.IsNullOrEmpty())
                    ranges.Add(new NumericRangeSearchParam.NumericRangeField(NumericFieldName2TextBox.Text, int.Parse(NumericStart2TextBox.Text), int.Parse(NumericEnd2TextBox.Text)));

                return ranges;
            }
        }

        protected List<DateRangeSearchParam.DateRange> DateRanges
        {
            get
            {
                var dateRanges = new List<DateRangeSearchParam.DateRange>();

                if (!DateFieldName1TextBox.Text.IsNullOrEmpty() && !DateStartDate1TextBox.Text.IsNullOrEmpty() &&
                    !DateEndDate1TextBox.Text.IsNullOrEmpty())
                    dateRanges.Add(new DateRangeSearchParam.DateRange(DateFieldName1TextBox.Text,
                                                                      DateTime.Parse(DateStartDate1TextBox.Text),
                                                                      DateTime.Parse(DateEndDate1TextBox.Text)));

                if (!DateFieldName2TextBox.Text.IsNullOrEmpty() && !DateStartDate2TextBox.Text.IsNullOrEmpty() &&
                    !DateEndDate2TextBox.Text.IsNullOrEmpty())
                    dateRanges.Add(new DateRangeSearchParam.DateRange(DateFieldName2TextBox.Text,
                                                                      DateTime.Parse(DateStartDate2TextBox.Text),
                                                                      DateTime.Parse(DateEndDate2TextBox.Text)));

                return dateRanges;
            }
        }

        public override List<SkinnyItem> GetItems(string databaseName,
                                                 string indexName,
                                                 string language,
                                                 string templateFilter,
                                                 string locationFilter,
                                                 string fullTextQuery)
        {

            var baseCondition = GetCondition(BaseConditionList);
            var outerNumParamParamCondition = GetCondition(NumericRangeConditionList);
            var outerDateParamCondition = GetCondition(DateRangeConditionList);
            var innerNumParamParamCondition = GetCondition(InnerNumericRangeConditionList);
            var innerDateParamCondition = GetCondition(InnerDateRangeConditionList);

            var baseSearchParam = new SearchParam
            {
                Database = databaseName,
                LocationIds = locationFilter,
                TemplateIds = templateFilter,
                FullTextQuery = fullTextQuery,
                Language = language,
                Condition = baseCondition
            };

            var numSearchParam = new NumericRangeSearchParam
            {
                Ranges = NumericRanges,
                InnerCondition = innerNumParamParamCondition,
                Condition = outerNumParamParamCondition
            };

            var dateSearchParam = new DateRangeSearchParam
               {
                   Ranges = DateRanges,
                   InnerCondition = innerDateParamCondition,
                   Condition = outerDateParamCondition
               };

            using (var runner = new QueryRunner(indexName))
            {
                return runner.GetItems(new[] { baseSearchParam, numSearchParam, dateSearchParam });
            }
        }
    }
}