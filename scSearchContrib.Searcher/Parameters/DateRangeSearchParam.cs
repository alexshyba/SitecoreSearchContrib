namespace scSearchContrib.Searcher.Parameters
{
    using System;
    using System.Collections.Generic;

    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.Search;

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

        public override BooleanQuery ProcessQuery(QueryOccurance condition, Index index)
        {
            var outerQuery = new BooleanQuery();

            var translator = new QueryTranslator(index);
            var innerCondition = translator.GetOccur(InnerCondition);
            var outerCondition = translator.GetOccur(condition);

            var baseQuery = base.ProcessQuery(condition, index);
            if (baseQuery != null && baseQuery.Clauses().Count > 0)
            {
                outerQuery.Add(baseQuery, outerCondition);
            }

            var dateRangeQuery = ApplyDateRangeSearchParam(innerCondition);

            if (dateRangeQuery != null)
            {
                outerQuery.Add(dateRangeQuery, outerCondition);
            }

            return outerQuery;
        }

        protected BooleanQuery ApplyDateRangeSearchParam(BooleanClause.Occur innerCondition)
        {
            var innerQuery = new BooleanQuery();

            if (Ranges.Count <= 0)
            {
                return null;
            }

            foreach (var dateParam in Ranges)
            {
                AddDateRangeQuery(innerQuery, dateParam, innerCondition);
            }

            return innerQuery;
        }

        protected void AddDateRangeQuery(BooleanQuery query, DateRange dateRangeField, BooleanClause.Occur condition)
        {
            var startDateTime = dateRangeField.StartDate;
            var endDateTime = dateRangeField.EndDate;

            // converting to lucene format
            var startDate = DateTools.DateToString(startDateTime, DateTools.Resolution.DAY);
            var endDate = DateTools.DateToString(endDateTime, DateTools.Resolution.DAY);

            var rangeQuery = new RangeQuery(new Term(dateRangeField.FieldName, startDate), new Term(dateRangeField.FieldName, endDate), true);
            query.Add(rangeQuery, condition);
        }
    }
}
