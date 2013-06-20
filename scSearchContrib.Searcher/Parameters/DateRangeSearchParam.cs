namespace scSearchContrib.Searcher.Parameters
{
    using System;
    using System.Collections.Generic;

    using Lucene.Net.Search;

    public class DateRangeSearchParam : MultiSearchParam
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

        public IEnumerable<DateRange> Ranges { get; set; }

        protected override Query BuildQuery(BooleanClause.Occur condition)
        {
            return QueryBuilder.BuildDateRangeSearchParam(this.Ranges, condition);
        }
    }
}
