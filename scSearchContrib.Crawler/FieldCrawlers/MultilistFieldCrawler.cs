namespace scSearchContrib.Crawler.FieldCrawlers
{
    using System.Collections.Generic;
    using System.Linq;

    using scSearchContrib.Searcher.Utilities;

    using Sitecore.Data.Fields;
    using Sitecore.Search.Crawlers.FieldCrawlers;

    /// <summary>
    /// The crawler for multilist fields.
    /// </summary>
    public class MultilistFieldCrawler : FieldCrawlerBase, IMultivaluedFieldCrawler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultilistFieldCrawler"/> class.
        /// </summary>
        /// <param name="field">
        /// The field.
        /// </param>
        public MultilistFieldCrawler(Field field)
            : base(field)
        {
        }

        /// <summary>
        /// The get values.
        /// </summary>
        /// <returns>
        /// The IEnumberable of values
        /// </returns>
        public IEnumerable<string> GetValues()
        {
            if (FieldTypeManager.GetField(_field) is MultilistField)
            {
                MultilistField field = _field;
                return field.TargetIDs.Select(IdHelper.NormalizeGuid);
            }

            return new string[0];
        }
    }
}
