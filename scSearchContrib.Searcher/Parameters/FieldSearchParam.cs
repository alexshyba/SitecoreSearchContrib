namespace scSearchContrib.Searcher.Parameters
{
    using Lucene.Net.Search;

    using scSearchContrib.Searcher.Utilities;

    using Sitecore.Diagnostics;
    using Sitecore.Search;

    /// <summary>
    /// The field search param.
    /// </summary>
    public class FieldSearchParam : SearchParam
    {
        /// <summary>
        /// Gets or sets the field name.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the field value.
        /// </summary>
        public string FieldValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether partial.
        /// </summary>
        public bool Partial { get; set; }

        /// <summary>
        /// The process query.
        /// </summary>
        /// <param name="condition">
        /// The condition.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The <see cref="BooleanQuery"/>.
        /// </returns>
        public override Query ProcessQuery(QueryOccurance condition, Index index)
        {
            Assert.ArgumentNotNull(index, "Index");

            var baseQuery = base.ProcessQuery(condition, index);

            var translator = new QueryTranslator(index);
            Assert.IsNotNull(translator, "translator");

            var fieldQuery = this.Partial ? QueryBuilder.BuildPartialFieldValueClause(index, this.FieldName, this.FieldValue) :
                                            QueryBuilder.BuildExactFieldValueClause(index, this.FieldName, this.FieldValue);

            if (baseQuery == null)
            {
                return fieldQuery;
            }

            if (baseQuery is BooleanQuery)
            {
                var booleanQuery = baseQuery as BooleanQuery;
                booleanQuery.Add(fieldQuery, translator.GetOccur(condition));
            }

            return baseQuery;
        }
    }
}
