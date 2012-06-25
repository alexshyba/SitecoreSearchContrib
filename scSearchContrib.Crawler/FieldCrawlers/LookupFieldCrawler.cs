using System;
using Sitecore.Data.Fields;
using Sitecore.Search.Crawlers.FieldCrawlers;
using scSearchContrib.Searcher.Utilities;

namespace scSearchContrib.Crawler.FieldCrawlers
{
    /// <summary>
    /// Defines a way to crawle lookup fields.
    /// </summary>
    public class LookupFieldCrawler : FieldCrawlerBase
    {
        public LookupFieldCrawler(Field field) : base(field) { }

        /// <summary>
        /// Returns lookup field value as an item display name instead of item ID.
        /// </summary>
        /// <returns></returns>
        public override string GetValue()
        {
            var value = String.Empty;

            if (FieldTypeManager.GetField(_field) is LookupField)
            {
                var lookupField = new LookupField(_field);
                value = IdHelper.NormalizeGuid(lookupField.TargetID);
            }
            if (FieldTypeManager.GetField(_field) is ReferenceField)
            {
                var referenceField = new ReferenceField(_field);
                value = IdHelper.NormalizeGuid(referenceField.TargetID);
            }

            return value;
        }
    }
}