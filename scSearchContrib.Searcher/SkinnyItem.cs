using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Sitecore.Data;
using System.Collections.Specialized;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Search;

namespace scSearchContrib.Searcher
{
   public class SkinnyItem
   {
      public SkinnyItem(string id, string language, string version, string databaseName)
         : this(new ItemUri(ID.Parse(id), Sitecore.Globalization.Language.Parse(language), Sitecore.Data.Version.Parse(version), databaseName))
      { }

      public SkinnyItem(string itemUri)
         : this(ItemUri.Parse(itemUri))
      { }

      public SkinnyItem(ItemUri itemUri)
      {
         RenderedFields = new List<string>();
         Uri = itemUri;
         Values = new Dictionary<string, IEnumerable<string>>
                      {
                          {BuiltinFields.Language, new string[] {Uri.Language.Name}},
                          {SearchFieldIDs.Version, new string[] {Uri.Version.Number.ToString()}}
                      };
      }

      [Obsolete("Use Values collection or indexer")]
      public NameValueCollection Fields
      {
          get
          {
              var fields = new NameValueCollection();
              foreach (var field in Values.Keys)
              {
                  fields[field] = Values[field].LastOrDefault();
              }
              return fields;
          }
          set
          {
              return;
          }
      }

      public IDictionary<string, IEnumerable<string>> Values { get; set; }

      public ItemUri Uri { get; set; }

      public List<string> RenderedFields { get; set; }

      public string ItemID  { get { return Uri.ItemID.ToString(); } }

      public string Name { get { return this[BuiltinFields.Name]; } }

      public string Version { get { return Uri.Version.Number.ToString(); } }

      public string Language { get { return Uri.Language.Name; } }

      public string TemplateName { get { return this[SearchFieldIDs.TemplateName]; } }

      public string Path { get { return this[SearchFieldIDs.FullContentPath]; } }

       public string this[string field]
       {
           get
           {
               if (Values.ContainsKey(field) && Values[field] != null)
               {
                   return Values[field].LastOrDefault();
               }
               return null;
           }
       }

      public Item GetItem()
      {
         var db = Factory.GetDatabase(Uri.DatabaseName);
         return db != null ? db.GetItem(Uri.ItemID, Uri.Language, Uri.Version) : null;
      }

      public override string ToString()
      {
         var itemInformation = String.Format("{0}, {1}, {2}", Uri.ItemID, Uri.Language, Uri.Version);

         foreach (string key in Values.Keys)
         {
             itemInformation += ", " + this[key];
         }

         return itemInformation;
      }
   }
}
