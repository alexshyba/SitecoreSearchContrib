using System;
using Lucene.Net.Documents;
using Sitecore.Search.Crawlers.FieldCrawlers;
using Field = Sitecore.Data.Fields.Field;

namespace scSearchContrib.Crawler.FieldCrawlers
{
    public class NumberFieldCrawler : FieldCrawlerBase
    {
        public NumberFieldCrawler(Field field) : base(field) { }

        public override string GetValue()
        {
            long value;

            if (!String.IsNullOrEmpty(_field.Value) && long.TryParse(_field.Value, out value))
            {
                return NumberTools.LongToString(value);
            }

            return String.Empty;
        }
    }
}
