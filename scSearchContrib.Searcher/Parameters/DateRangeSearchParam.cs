namespace scSearchContrib.Searcher.Parameters
{
    using System;
    using System.Collections.Generic;

    using Lucene.Net.Search;

    using Sitecore.Diagnostics;

    using scSearchContrib.Searcher.Utilities;

    using Sitecore.Search;

    public class DateRangeSearchParam : SearchParam
    {
        public class DateRange
        {
            public DateRange()
            {
            }

            public DateRange(string fieldName, DateTime startDate, DateTime endDate)
            {
                FieldName = fieldName.ToLowerInvariant();
                StartDate = startDate;
                EndDate = endDate;
            }

            #region Properties

            [Obsolete("This property is not used any more")]
            public bool InclusiveStart { get; set; }

            [Obsolete("This property is not used any more")]
            public bool InclusiveEnd { get; set; }

            public string FieldName { get; set; }

            public DateTime StartDate { get; set; }

            public DateTime EndDate { get; set; }

            #endregion Properties
        }

        public List<DateRange> Ranges { get; set; }

        public QueryOccurance InnerCondition { get; set; }

        public override Query ProcessQuery(QueryOccurance condition, Index index)
        {
            Assert.ArgumentNotNull(index, "Index");

            var baseQuery = base.ProcessQuery(condition, index) as BooleanQuery ?? new BooleanQuery();

            var translator = new QueryTranslator(index);
            Assert.IsNotNull(translator, "Query Translator");
            var innerCondition = translator.GetOccur(InnerCondition);
            var outerCondition = translator.GetOccur(condition);

            var dateRangeQuery = QueryBuilder.BuildDateRangeSearchParam(this.Ranges, innerCondition);
            if (dateRangeQuery != null)
            {
                baseQuery.Add(dateRangeQuery, outerCondition);
            }

            return baseQuery;
        }
    }
}
