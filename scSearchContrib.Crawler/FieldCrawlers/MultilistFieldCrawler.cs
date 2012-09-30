using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Search.Crawlers.FieldCrawlers;
using Sitecore.Data.Fields;
using scSearchContrib.Searcher.Utilities;

namespace scSearchContrib.Crawler.FieldCrawlers
{
    public class MultilistFieldCrawler : FieldCrawlerBase, IMultivaluedFieldCrawler
    {
        public MultilistFieldCrawler(Field field) : base(field) { }

        public IEnumerable<string> GetValues()
        {
            if (FieldTypeManager.GetField(_field) is MultilistField)
            {
                MultilistField field = base._field;
                return field.TargetIDs.Select(IdHelper.NormalizeGuid);
            }
            return new string[0];
        }
    }
}
