namespace scSearchContrib.Searcher.Parameters
{
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;

    using Sitecore.Diagnostics;

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
        public override Query ProcessQuery(QueryOccurance condition, Index index)
        {
            Assert.ArgumentNotNull(index, "Index");

            var baseQuery = base.ProcessQuery(condition, index);

            var translator = new QueryTranslator(index);
            Assert.IsNotNull(translator, "translator");

            var fieldQuery = this.Partial ? this.AddPartialFieldValueClause(index, this.FieldName, this.FieldValue) :
                                            this.AddExactFieldValueClause(index, this.FieldName, this.FieldValue);

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
        protected Query AddPartialFieldValueClause(Index index, string fieldName, string fieldValue)
        {
            Assert.ArgumentNotNull(index, "Index");
            Assert.ArgumentNotNullOrEmpty(fieldName, "fieldName");

            if (string.IsNullOrEmpty(fieldValue))
            {
                return null;
            }

            fieldValue = IdHelper.ProcessGUIDs(fieldValue);
            var fieldQuery = new QueryParser(fieldName.ToLowerInvariant(), index.Analyzer).Parse(fieldValue);
            return fieldQuery;
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
        protected Query AddExactFieldValueClause(Index index, string fieldName, string fieldValue)
        {
            Assert.ArgumentNotNull(index, "Index");

            if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(fieldValue))
            {
                return null;
            }

            fieldValue = IdHelper.ProcessGUIDs(fieldValue);

            var phraseQuery = new PhraseQuery();
            phraseQuery.Add(new Term(fieldName.ToLowerInvariant(), fieldValue.ToLowerInvariant()));

            return phraseQuery;
        }
    }
}
