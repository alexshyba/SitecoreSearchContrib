namespace scSearchContrib.Searcher.Parameters
{
    using System.Collections.Generic;

    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.Search;

    using scSearchContrib.Searcher.Utilities;

    using Sitecore.Search;

    public class NumericRangeSearchParam : SearchParam
    {
        public class NumericRangeField
        {
            public NumericRangeField() { }

            public NumericRangeField(string fieldName, long start, long end)
            {
                FieldName = fieldName.ToLowerInvariant();
                Start = start;
                End = end;
            }

            #region Properties

            public string FieldName { get; set; }
            public long Start { get; set; }
            public long End { get; set; }

            #endregion Properties
        }

        public List<NumericRangeField> Ranges { get; set; }

        public QueryOccurance InnerCondition { get; set; }

        public override BooleanQuery ProcessQuery(QueryOccurance condition, Index index)
        {
            var outerQuery = new BooleanQuery();

            var baseQuery = base.ProcessQuery(condition, index) ?? new BooleanQuery();
            var translator = new QueryTranslator(index);
            var innerCondition = translator.GetOccur(InnerCondition);
            var outerCondition = translator.GetOccur(condition);

            if (baseQuery.Clauses().Count > 0)
            {
                outerQuery.Add(baseQuery, outerCondition);
            }

            var numericQuery = ApplyNumericRangeSearchParam(innerCondition);

            if (numericQuery != null)
            {
                outerQuery.Add(numericQuery, outerCondition);
            }

            return outerQuery;
        }

        protected BooleanQuery ApplyNumericRangeSearchParam(BooleanClause.Occur innerCondition)
        {
            var innerQuery = new BooleanQuery();

            if (Ranges.Count <= 0)
            {
                return null;
            }

            foreach (var range in Ranges)
            {
                AddNumericRangeQuery(innerQuery, range, innerCondition);
            }

            return innerQuery;
        }

        protected void AddNumericRangeQuery(BooleanQuery query, NumericRangeField range, BooleanClause.Occur condition)
        {
            var startTerm = new Term(range.FieldName, NumberTools.LongToString(range.Start));
            var endTerm = new Term(range.FieldName, NumberTools.LongToString(range.End));
            var rangeQuery = new RangeQuery(startTerm, endTerm, true);
            query.Add(rangeQuery, condition);
        }
    }
}
