namespace scSearchContrib.Searcher.Parameters
{
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;

    using scSearchContrib.Searcher.Utilities;

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
        public override BooleanQuery ProcessQuery(QueryOccurance condition, Index index)
        {
            var query = base.ProcessQuery(condition, index) ?? new BooleanQuery();

            if (Partial)
            {
                AddPartialFieldValueClause(index, query, FieldName, FieldValue);
            }
            else
            {
                AddExactFieldValueClause(index, query, FieldName, FieldValue);
            }

            return query;
        }

        /// <summary>
        /// The add partial field value clause.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="fieldName">
        /// The field name.
        /// </param>
        /// <param name="fieldValue">
        /// The field value.
        /// </param>
        protected void AddPartialFieldValueClause(Index index, BooleanQuery query, string fieldName, string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldValue))
            {
                return;
            }

            fieldValue = IdHelper.ProcessGUIDs(fieldValue);

            var fieldQuery = new QueryParser(fieldName.ToLowerInvariant(), index.Analyzer).Parse(fieldValue);

            query.Add(fieldQuery, BooleanClause.Occur.MUST);
        }

        /// <summary>
        /// The add exact field value clause.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="fieldName">
        /// The field name.
        /// </param>
        /// <param name="fieldValue">
        /// The field value.
        /// </param>
        protected void AddExactFieldValueClause(Index index, BooleanQuery query, string fieldName, string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldValue))
            {
                return;
            }

            fieldValue = IdHelper.ProcessGUIDs(fieldValue);

            var phraseQuery = new PhraseQuery();
            phraseQuery.Add(new Term(fieldName.ToLowerInvariant(), fieldValue));

            query.Add(phraseQuery, BooleanClause.Occur.MUST);
        }
    }
}
