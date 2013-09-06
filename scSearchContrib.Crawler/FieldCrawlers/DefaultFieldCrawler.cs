using Sitecore.Data.Fields;
using Sitecore.Search.Crawlers.FieldCrawlers;

namespace scSearchContrib.Crawler.FieldCrawlers
{
    public class DefaultFieldCrawler : FieldCrawlerBase
    {
        public DefaultFieldCrawler(Field field)
            : base(field)
        {
        }
    }
}
