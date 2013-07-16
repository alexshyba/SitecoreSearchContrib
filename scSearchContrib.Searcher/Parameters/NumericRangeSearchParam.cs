namespace scSearchContrib.Searcher.Parameters
{
    using System.Collections.Generic;

    using Lucene.Net.Search;

    public class NumericRangeSearchParam : MultiSearchParam
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

        public IEnumerable<NumericRangeField> Ranges { get; set; }

        protected override Query BuildQuery(BooleanClause.Occur condition)
        {
            return QueryBuilder.BuildNumericRangeSearchParam(this.Ranges, condition);
        }
    }
}
