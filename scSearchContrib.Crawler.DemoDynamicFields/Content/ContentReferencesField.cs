using System.Linq;
using Lucene.Net.Documents;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using scSearchContrib.Crawler.DynamicFields;

namespace scSearchContrib.Crawler.DemoDynamicFields
{
    public class ContentReferencesField : BaseDynamicField
    {
        public override string ResolveValue(Item item)
        {
            Assert.ArgumentNotNull(item, "item");

            var referrers = Globals.LinkDatabase.GetReferrers(item).Select(r => r.GetSourceItem()).ToList();

            var total = 0;

            if (referrers.Any()) total = referrers.Count(ValidReferrer);

            return NumberTools.LongToString(total);
        }

        protected bool ValidReferrer(Item item)
        {
            return item != null && item.Paths.IsContentItem;
        }
    }
}
