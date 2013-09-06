using System;
using Lucene.Net.Documents;
using Sitecore.Data.Fields;
using Sitecore.Search.Crawlers.FieldCrawlers;
using DateField = Sitecore.Data.Fields.DateField;
using Field = Sitecore.Data.Fields.Field;

namespace scSearchContrib.Crawler.FieldCrawlers
{
    /// <summary>
    /// Defines a way to crawl date and datetime fields.
    /// </summary>
    public class DateFieldCrawler : FieldCrawlerBase
    {
        public DateFieldCrawler(Field field) : base(field) { }

        /// <summary>
        /// Returns date/datetime field value in yyyyMMddHHmmss format.
        /// </summary>
        /// <returns></returns>
        public override string GetValue()
        {
            if (String.IsNullOrEmpty(_field.Value))
            {
                return DateTools.DateToString(DateTime.MinValue, DateTools.Resolution.DAY);
            }

            if (FieldTypeManager.GetField(_field) is DateField)
            {
                var dateField = new DateField(_field);

                if (dateField.DateTime > DateTime.MinValue)
                {
                    return DateTools.DateToString(dateField.DateTime, DateTools.Resolution.DAY);
                }
            }

            return String.Empty;
        }
    }
}
