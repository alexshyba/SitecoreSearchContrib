using System.Collections.Generic;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Sitecore.Search;
using scSearchContrib.Searcher.Utilities;

namespace scSearchContrib.Searcher.Parameters
{
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

        public override BooleanQuery ProcessQuery(QueryOccurance occurance, Index index)
        {
            var outerQuery = new BooleanQuery();

            var baseQuery = base.ProcessQuery(occurance, index) ?? new BooleanQuery();
            var translator = new QueryTranslator(index);
            var innerOccurance = translator.GetOccur(InnerCondition);
            var outerOccurance = translator.GetOccur(occurance);

            if (baseQuery.Clauses().Count > 0)
                outerQuery.Add(baseQuery, outerOccurance);

            var numericQuery = ApplyNumericRangeSearchParam(innerOccurance);

            if (numericQuery != null)
                outerQuery.Add(numericQuery, outerOccurance);

            return outerQuery;
        }

        protected BooleanQuery ApplyNumericRangeSearchParam(BooleanClause.Occur innerOccurance)
        {
            var innerQuery = new BooleanQuery();

            if (Ranges.Count <= 0) return null;

            foreach (var range in Ranges)
            {
                AddNumericRangeQuery(innerQuery, range, innerOccurance);
            }

            return innerQuery;
        }

        protected void AddNumericRangeQuery(BooleanQuery query, NumericRangeField range, BooleanClause.Occur occurance)
        {
            var startTerm = new Term(range.FieldName, NumberTools.LongToString(range.Start));
            var endTerm = new Term(range.FieldName, NumberTools.LongToString(range.End));
            var rangeQuery = new RangeQuery(startTerm, endTerm, true);
            query.Add(rangeQuery, occurance);
        }
    }
}
