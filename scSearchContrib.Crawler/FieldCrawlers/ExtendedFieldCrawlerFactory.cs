namespace scSearchContrib.Crawler.FieldCrawlers
{
    using System;
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
                    var fieldCrawler = GetFieldCrawlerByType(fieldCrawlerType, field);

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

        private static object GetFieldCrawlerByType(string fieldCrawlerType, Field field)
        {
            Func<Field, object> constructor;
            // We could put in interlocks into the below code. However it is idempotent.
            // Yes it will be non-optimal if multiple threads access the same type at the same time
            // However afterwards, we will have better perf, when there is no locking.
            // Plus its simplier :)
            if (_compiledExpressions.TryGetValue(fieldCrawlerType, out constructor) == false)
            {
                _compiledExpressions[fieldCrawlerType] = constructor = ConstructorExpression(fieldCrawlerType);
            }
            return constructor(field);
        }
        
        // Cache the IL for constructing FieldCrawlers
        // Expression compile is expensive. But still SOOO much cheaper than reflection that it is replacing.
        private readonly static Dictionary<string, Func<Field, object>> _compiledExpressions = new Dictionary<string, Func<Field, object>>();


        // Create IL to run the constructor.
        private static Func<Field, object> ConstructorExpression(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type == null)
                return field => null;

            var constructor = type.GetConstructor(new[] {typeof (Field)});

            if (constructor == null)
                return field => null;

            var paramExpression = System.Linq.Expressions.Expression.Parameter(typeof (Field), "field");
            var newExpression = System.Linq.Expressions.Expression.New(constructor, paramExpression);
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<Field, object>>(newExpression, paramExpression);

            return lambda.Compile();

        }
    }
}
