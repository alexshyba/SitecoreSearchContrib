namespace scSearchContrib.Crawler.FieldCrawlers
{
    using scSearchContrib.Searcher.Utilities;

    using Sitecore.Data;
    using Sitecore.Data.Fields;
    using Sitecore.Search.Crawlers.FieldCrawlers;

    /// <summary>
    /// Defines a way to crawl lookup fields.
    /// </summary>
    public class LookupFieldCrawler : FieldCrawlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LookupFieldCrawler"/> class.
        /// </summary>
        /// <param name="field">
        /// The field.
        /// </param>
        public LookupFieldCrawler(Field field)
            : base(field)
        {
        }

        /// <summary>
        /// Returns lookup field value as normalized ID of the target item
        /// </summary>
        /// <returns>processed field value</returns>
        public override string GetValue()
        {
            var value = string.Empty;

            if (FieldTypeManager.GetField(_field) is LookupField)
            {
                var lookupField = new LookupField(_field);
                if (lookupField.TargetID != ID.Null)
                {
                    value = IdHelper.NormalizeGuid(lookupField.TargetID);
                }
            }
            if (FieldTypeManager.GetField(_field) is ReferenceField)
            {
                var referenceField = new ReferenceField(_field);
                if (referenceField.TargetID != ID.Null)
                {
                    value = IdHelper.NormalizeGuid(referenceField.TargetID);
                }
            }

            return value;
        }
    }
}