using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scSearchContrib.Crawler.FieldCrawlers
{
    public interface IMultivaluedFieldCrawler
    {
        IEnumerable<string> GetValues();
    }
}
