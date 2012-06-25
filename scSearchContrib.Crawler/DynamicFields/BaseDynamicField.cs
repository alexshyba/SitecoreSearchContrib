using Sitecore.Data.Items;

namespace scSearchContrib.Crawler.DynamicFields
{
    public abstract class BaseDynamicField : SearchField
    {
        public abstract string ResolveValue(Item item);

        public string FieldKey { get; set; }
    }
}
