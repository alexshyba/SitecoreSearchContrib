namespace scSearchContrib.Searcher.Parameters
{
    using System.Collections.Generic;
    using System.Linq;

    using Lucene.Net.Search;

    using scSearchContrib.Searcher.Utilities;

    using Sitecore.Diagnostics;
    using Sitecore.Search;

    public class SearchParam : ISearchParam
    {
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

        /// <summary>
        /// Gets or sets the full text query.
        /// </summary>
        public string FullTextQuery { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }

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
        public virtual Query ProcessQuery(QueryOccurance condition, Index index)
        {
            Assert.ArgumentNotNull(index, "Index");

            var translator = new QueryTranslator(index);
            Assert.IsNotNull(translator, "Query Translator");

            var templateFieldName = this.SearchBaseTemplates ? SearchFieldIDs.AllTemplates : BuiltinFields.Template;

            var queries = new List<Query>
                              {
                                  QueryBuilder.BuildFullTextQuery(this.FullTextQuery, index),
                                  QueryBuilder.BuildRelationFilter(this.RelatedIds),
                                  QueryBuilder.BuildIdFilter(templateFieldName, this.TemplateIds),
                                  QueryBuilder.BuildLocationFilter(this.LocationIds),
                                  QueryBuilder.BuildFieldQuery(BuiltinFields.Database, this.Database),
                                  QueryBuilder.BuildLanguageQuery(this.Language)
                              }
                              .Where(q => q != null)
                              .ToList();

            if (!queries.Any())
            {
                return null;
            }

            var occur = translator.GetOccur(condition);
            var booleanQuery = new BooleanQuery();
            foreach (var query in queries)
            {
                booleanQuery.Add(query, occur);
            }

            return booleanQuery;
        }
    }
}