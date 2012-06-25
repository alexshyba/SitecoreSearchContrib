using System;
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
         Fields = new NameValueCollection();
         RenderedFields = new List<string>();
         Uri = itemUri;
         Fields.Add(BuiltinFields.Language, Uri.Language.Name);
         Fields.Add(SearchFieldIDs.Version, Uri.Version.Number.ToString());
      }

      public NameValueCollection Fields { get; set; }

      public ItemUri Uri { get; set; }

      public List<string> RenderedFields { get; set; }

      public string ItemID  { get { return Uri.ItemID.ToString(); } }

      public string Name { get { return Fields[BuiltinFields.Name]; } }

      public string Version { get { return Uri.Version.Number.ToString(); } }

      public string Language { get { return Uri.Language.Name; } }

      public string TemplateName { get { return Fields[SearchFieldIDs.TemplateName]; } }

      public string Path { get { return Fields[SearchFieldIDs.FullContentPath]; } }

      public Item GetItem()
      {
         var db = Factory.GetDatabase(Uri.DatabaseName);
         return db != null ? db.GetItem(Uri.ItemID, Uri.Language, Uri.Version) : null;
      }

      public override string ToString()
      {
         var itemInformation = String.Format("{0}, {1}, {2}", Uri.ItemID, Uri.Language, Uri.Version);

         foreach (string key in Fields.Keys)
         {
            itemInformation += ", " + Fields[key];
         }

         return itemInformation;
      }
   }
}
