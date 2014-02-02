namespace scSearchContrib.Searcher.Parameters
{
    using System.Linq;

    using Lucene.Net.Search;

    using Sitecore.Data;
    using Sitecore.StringExtensions;

    using scSearchContrib.Searcher.Utilities;
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
        public virtual IQueryable<SkinnyItem> ProcessQuery(IQueryable<SkinnyItem> query, QueryOccurance condition, Index index)
        {
            if (!this.Database.IsNullOrEmpty())
            {
                query = query.Where(q => q.DatabaseName == this.Database);
            }

            if (!this.Language.IsNullOrEmpty())
            {
                query = query.Where(q => q.Language == this.Language);
            }

            if (!this.FullTextQuery.IsNullOrEmpty())
            {
                query = query.Where(q => q.Content.Contains(this.FullTextQuery));
            }

            if (!this.TemplateIds.IsNullOrEmpty())
            {
                // Temp Limitation - only the first template id is taken into account
                var templateId = IdHelper.ParseId(this.TemplateIds).Select(ID.Parse).FirstOrDefault();
                query = query.Where(q => q.TemplateId == templateId);
            }

            if (!this.LocationIds.IsNullOrEmpty())
            {
                foreach (var id in IdHelper.ParseId(this.LocationIds).Select(ID.Parse))
                {
                    query = query.Where(q => q.Paths.Contains(id));
                }
            }

            if (!this.LocationIds.IsNullOrEmpty())
            {
                foreach (var id in IdHelper.ParseId(this.LocationIds).Select(ID.Parse))
                {
                    query = query.Where(q => q.Paths.Contains(id));
                }
            }

            if (!this.RelatedIds.IsNullOrEmpty())
            {
                // Temp Limitation - only the first related id is taken into account
                var relatedId = IdHelper.ParseId(this.RelatedIds).Select(ID.Parse).FirstOrDefault();
                query = query.Where(q => q.Links.Contains(relatedId));
            }

            return query;
        }
    }
}