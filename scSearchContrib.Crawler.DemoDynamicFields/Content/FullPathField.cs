using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using scSearchContrib.Crawler.DynamicFields;

namespace scSearchContrib.Crawler.DemoDynamicFields
{
    public class FullPathField : BaseDynamicField
    {
        public override string ResolveValue(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            return item.Paths.FullPath;
        }
    }
}
