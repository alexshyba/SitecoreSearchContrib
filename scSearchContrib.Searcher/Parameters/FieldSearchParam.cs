using System.Linq;

namespace scSearchContrib.Searcher.Parameters
{
    using Lucene.Net.Search;
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
        public override IQueryable<SkinnyItem> ProcessQuery(IQueryable<SkinnyItem> query, QueryOccurance condition, Index index)
        {
            var baseQuery = base.ProcessQuery(query, condition, index);

            baseQuery = this.Partial ? baseQuery.Where(i => i[this.FieldName].Contains(this.FieldValue)) :
                                       baseQuery.Where(i => i[this.FieldName] == this.FieldValue);

            return baseQuery;
        }
    }
}
