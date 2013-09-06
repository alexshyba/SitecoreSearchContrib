using System.Text;
using Sitecore.Data.Items;
using scSearchContrib.Searcher.Utilities;

namespace scSearchContrib.Crawler.DynamicFields
{
    /// <summary>
    /// Allows enhancing the built-in Sitecore.Search.BuiltinFields.AllTemplates with
    /// all of an item's base templates. By default, just includes direct base templates of
    /// the item's Template.
    /// </summary>
    public class AllTemplatesField : BaseDynamicField
    {
        public override string ResolveValue(Item item)
        {
            var templates = new StringBuilder();
            GetAllTemplates(item.Template, templates);
            return templates.ToString();
        }

        public void GetAllTemplates(TemplateItem baseTemplate, StringBuilder builder)
        {
            if (baseTemplate.ID == Sitecore.TemplateIDs.StandardTemplate)
            {
                return;
            }
            var id = IdHelper.NormalizeGuid(baseTemplate.ID);
            builder.Append(id);
            foreach (var template in baseTemplate.BaseTemplates)
            {
                builder.Append(" ");
                GetAllTemplates(template, builder);
            }
        }
    }
}
