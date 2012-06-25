namespace scSearchContrib.Searcher
{
   public class IndexConstants
   {
      //public static string Name
      //{
      //   get
      //   {
      //      return Context.Database.Name == "master" ? "advanced-master" : "advanced-web";
      //   }
      //}

      // Lucene recognizable format for Sitecore DateTime fields.
      //public static readonly string DateTimeFormat = "yyyyMMddHHmmss";
   }

   public class SearchFieldIDs
   {
      public static readonly string Version = "_version";
      public static readonly string TemplateName = "_templatename";
      public static readonly string FullContentPath = "_fullcontentpath";
   }
}
