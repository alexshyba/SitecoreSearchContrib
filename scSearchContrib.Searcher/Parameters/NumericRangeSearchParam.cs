namespace scSearchContrib.Searcher.Parameters
{
    using System.Collections.Generic;

    using Lucene.Net.Search;

    using Sitecore.Diagnostics;

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

            /// <summary>
            /// Gets or sets the field name.
            /// </summary>
            public string FieldName { get; set; }

            /// <summary>
            /// Gets or sets the start.
            /// </summary>
            public long Start { get; set; }

            /// <summary>
            /// Gets or sets the end.
            /// </summary>
            public long End { get; set; }

            #endregion Properties
        }

        public List<NumericRangeField> Ranges { get; set; }

        public QueryOccurance InnerCondition { get; set; }

        public override Query ProcessQuery(QueryOccurance condition, Index index)
        {
            Assert.ArgumentNotNull(index, "Index");

            var baseQuery = base.ProcessQuery(condition, index) as BooleanQuery ?? new BooleanQuery();
            var translator = new QueryTranslator(index);
            Assert.IsNotNull(translator, "Query Translator");

            var innerCondition = translator.GetOccur(InnerCondition);
            var outerCondition = translator.GetOccur(condition);
            var dateRangeQuery = QueryBuilder.BuildNumericRangeSearchParam(this.Ranges, innerCondition);
            if (dateRangeQuery != null)
            {
                baseQuery.Add(dateRangeQuery, outerCondition);
            }

            return baseQuery;
        }
    }
}
