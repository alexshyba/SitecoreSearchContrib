namespace scSearchContrib.Demo
{
  using System.Collections.Generic;

  using scSearchContrib.Searcher;
  using scSearchContrib.Searcher.Parameters;

  public partial class RelationSearchDemoPage : BaseDemoPage
    {
        protected string RelationFilter
        {
            get { return RelatedIdsTextBox.Text; }
        }

        public override List<SkinnyItem> GetItems(string databaseName,
                                                  string indexName,
                                                  string language,
                                                  string templateFilter,
                                                  bool searchBaseTemplates,
                                                  string locationFilter,
                                                  string fullTextQuery)
        {
            var searchParam = new SearchParam
            {
                Database = databaseName,
                Language = language,
                RelatedIds = RelationFilter,
                TemplateIds = templateFilter,
                SearchBaseTemplates = searchBaseTemplates,
                LocationIds = locationFilter,
                FullTextQuery = fullTextQuery
            };

            using (var runner = new QueryRunner(indexName))
            {
                return runner.GetItems(searchParam);
            }
        }
    }
}