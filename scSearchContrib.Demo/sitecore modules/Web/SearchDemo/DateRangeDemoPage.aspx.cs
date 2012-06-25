using scSearchContrib.Searcher;
using scSearchContrib.Searcher.Parameters;
using System;
using System.Collections.Generic;
using Sitecore.StringExtensions;

namespace scSearchContrib.Demo
{
    public partial class DateRangeDemoPage : BaseDemoPage
   {
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
         var searchParam = new DateRangeSearchParam
                              {
                                 Database = databaseName,
                                 Ranges = DateRanges,
                                 LocationIds = locationFilter,
                                 TemplateIds = templateFilter,
                                 FullTextQuery = fullTextQuery,
                                 InnerCondition = GetCondition(InnerDateRangeOccuranceList),
                                 Language = language
                              };

         using (var runner = new QueryRunner(indexName))
         {
            return runner.GetItems(searchParam);
         }
      }
   }
}