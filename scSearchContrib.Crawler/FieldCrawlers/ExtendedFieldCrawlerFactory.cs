namespace scSearchContrib.Crawler.FieldCrawlers
{
    using System.Collections.Generic;

    using Sitecore.Collections;
    using Sitecore.Data.Fields;
    using Sitecore.Diagnostics;
    using Sitecore.Reflection;
    using Sitecore.Search.Crawlers.FieldCrawlers;

    /// <summary>
    /// The extended field crawler factory.
    /// </summary>
    public class ExtendedFieldCrawlerFactory
    {
        /// <summary>
        /// The get field crawler values.
        /// </summary>
        /// <param name="field">
        /// The field.
        /// </param>
        /// <param name="fieldCrawlers">
        /// The field crawlers.
        /// </param>
        /// <returns>
        /// The IEnumerable of field crawler values.
        /// </returns>
        public static IEnumerable<string> GetFieldCrawlerValues(Field field, SafeDictionary<string, string> fieldCrawlers)
        {
            Assert.IsNotNull(field, "Field was not supplied");
            Assert.IsNotNull(fieldCrawlers, "Field Crawler collection is not specified");

            if (fieldCrawlers.ContainsKey(field.TypeKey))
            {
                var fieldCrawlerType = fieldCrawlers[field.TypeKey];

                if (!string.IsNullOrEmpty(fieldCrawlerType))
                {
                    var fieldCrawler = ReflectionUtil.CreateObject(fieldCrawlerType, new object[] { field });

                    if (fieldCrawler is IMultivaluedFieldCrawler)
                    {
                        return (fieldCrawler as IMultivaluedFieldCrawler).GetValues();
                    }

                    if (fieldCrawler is FieldCrawlerBase)
                    {
                        return new[] { (fieldCrawler as FieldCrawlerBase).GetValue() };
                    }
                }
            }

            return new[] { new DefaultFieldCrawler(field).GetValue() };
        }
    }
}
