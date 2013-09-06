using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using scSearchContrib.Crawler.DynamicFields;

namespace scSearchContrib.Crawler.DemoDynamicFields.Workflow
{
    public class IsLockedField : BaseDynamicField
    {
        public override string ResolveValue(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            return item.Locking.IsLocked().ToString().ToLowerInvariant();
        }
    }
}
