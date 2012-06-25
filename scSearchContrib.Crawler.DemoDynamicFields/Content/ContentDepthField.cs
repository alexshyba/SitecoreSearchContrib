using Lucene.Net.Documents;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using scSearchContrib.Crawler.DynamicFields;

namespace scSearchContrib.Crawler.DemoDynamicFields
{
    public class ContentDepthField : BaseDynamicField
    {
        public override string ResolveValue(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            return NumberTools.LongToString(item.Axes.Level);
        }
    }
}
