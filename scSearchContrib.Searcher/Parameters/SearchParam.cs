namespace scSearchContrib.Searcher.Parameters
{
    using System.Linq;

    using Lucene.Net.Index;
    using Lucene.Net.Search;

    using scSearchContrib.Searcher.Utilities;

    using Sitecore;
    using Sitecore.Data;
    using Sitecore.Search;
    using Sitecore.StringExtensions;

    public class SearchParam : ISearchParam
    {
        #region Fields

        private string database;
        private string language;

        #endregion

        #region Construction

        public SearchParam()
        {
            Condition = QueryOccurance.Must;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the related ids.
        /// </summary>
        public string RelatedIds { get; set; }

        /// <summary>
        /// Gets or sets the template ids.
        /// </summary>
        public string TemplateIds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether search base templates.
        /// </summary>
        public bool SearchBaseTemplates { get; set; }

        /// <summary>
        /// Gets or sets the location ids.
        /// </summary>
        public string LocationIds { get; set; }

        public string FullTextQuery { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        public string Language
        {
            get
            {
                return language.IsNullOrEmpty() ? Context.Language.Name : language;
            }

            set
            {
                language = value;
            }
        }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database
        {
            get
            {
                return database.IsNullOrEmpty() ? SearchHelper.ContextDB.Name : database;
            }

            set { database = value; }
        }

        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        public QueryOccurance Condition { get; set; }

        #endregion

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
        public virtual BooleanQuery ProcessQuery(QueryOccurance condition, Index index)
        {
            var innerQuery = new CombinedQuery();
            ApplyFullTextClause(innerQuery, FullTextQuery, condition);
            ApplyRelationFilter(innerQuery, RelatedIds, condition);
            ApplyTemplateFilter(innerQuery, TemplateIds, condition);
            ApplyLocationFilter(innerQuery, LocationIds, condition);
            AddFieldValueClause(innerQuery, BuiltinFields.Database, Database, condition);

            if (innerQuery.Clauses.Count < 1)
            {
                return null;
            }

            var translator = new QueryTranslator(index);
            var booleanQuery = translator.ConvertCombinedQuery(innerQuery);

            ApplyLanguageClause(booleanQuery, Language, translator.GetOccur(condition));

            return booleanQuery;
        }

        protected void ApplyLanguageClause(BooleanQuery query, string language, BooleanClause.Occur condition)
        {
            if (string.IsNullOrEmpty(language))
            {
                return;
            }

            var phraseQuery = new PhraseQuery();
            phraseQuery.Add(new Term(BuiltinFields.Language, language.ToLowerInvariant()));

            query.Add(phraseQuery, condition);
        }

        protected void ApplyFullTextClause(CombinedQuery query, string searchText, QueryOccurance condition)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return;
            }

            query.Add(new FullTextQuery(searchText), condition);
        }

        protected void ApplyIdFilter(CombinedQuery query, string fieldName, string filter, QueryOccurance condition)
        {
            if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(filter))
            {
                return;
            }

            var filterQuery = new CombinedQuery();

            var values = IdHelper.ParseId(filter);

            foreach (var value in values.Where(ID.IsID))
            {
                AddFieldValueClause(filterQuery, fieldName, value, QueryOccurance.Should);
            }

            query.Add(filterQuery, condition);
        }

        /// <summary>
        /// The apply template filter.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="templateIds">
        /// string with one or multiple pipe separated template IDs
        /// </param>
        /// <param name="condition">
        /// The condition.
        /// </param>
        protected void ApplyTemplateFilter(CombinedQuery query, string templateIds, QueryOccurance condition)
        {
            if (string.IsNullOrEmpty(templateIds))
            {
                return;
            }

            var fieldQuery = new CombinedQuery();
            var values = IdHelper.ParseId(templateIds);
            foreach (var value in values.Where(ID.IsID))
            {
                AddFieldValueClause(fieldQuery, BuiltinFields.Template, value, QueryOccurance.Should);
            }

            query.Add(fieldQuery, condition);
        }

        protected void ApplyLocationFilter(CombinedQuery query, string locationIds, QueryOccurance condition)
        {
            ApplyIdFilter(query, BuiltinFields.Path, locationIds, condition);
        }

        protected void ApplyRelationFilter(CombinedQuery query, string ids, QueryOccurance condition)
        {
            ApplyIdFilter(query, BuiltinFields.Links, ids, condition);
        }

        protected void AddFieldValueClause(CombinedQuery query, string fieldName, string fieldValue, QueryOccurance condition)
        {
            if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(fieldValue))
            {
                return;
            }

            // if we are searching by _id field, do not lowercase
            fieldValue = IdHelper.ProcessGUIDs(fieldValue);
            query.Add(new FieldQuery(fieldName.ToLowerInvariant(), fieldValue), condition);
        }
    }
}