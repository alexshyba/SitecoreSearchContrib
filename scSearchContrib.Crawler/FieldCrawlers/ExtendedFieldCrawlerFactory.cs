using System;
using System.Collections.Generic;
using Sitecore.Collections;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Reflection;
using Sitecore.Search.Crawlers.FieldCrawlers;

namespace scSearchContrib.Crawler.FieldCrawlers
{
   public class ExtendedFieldCrawlerFactory
   {
      public static IEnumerable<string> GetFieldCrawlerValues(Field field, SafeDictionary<string, string> fieldCrawlers)
      {
         Assert.IsNotNull(field, "Field was not supplied");
         Assert.IsNotNull(fieldCrawlers, "Field Crawler collection is not specified");

         if (fieldCrawlers.ContainsKey(field.TypeKey))
         {
            var fieldCrawlerType = fieldCrawlers[field.TypeKey];

            if (!String.IsNullOrEmpty(fieldCrawlerType))
            {
               var fieldCrawler = ReflectionUtil.CreateObject(fieldCrawlerType, new object[] {field});

               if (fieldCrawler != null && fieldCrawler is IMultivaluedFieldCrawler)
               {
                   return (fieldCrawler as IMultivaluedFieldCrawler).GetValues();
               }

               if (fieldCrawler != null && fieldCrawler is FieldCrawlerBase)
               {
                  return new string[] { (fieldCrawler as FieldCrawlerBase).GetValue() };
               }
            }
         }

         return new string[] { new DefaultFieldCrawler(field).GetValue() };
      }
   }
}
