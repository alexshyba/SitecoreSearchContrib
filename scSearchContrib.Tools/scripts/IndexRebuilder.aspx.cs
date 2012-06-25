using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Sitecore.Configuration;
using Sitecore.Jobs;
using Sitecore.Search;

namespace scSearchContrib.Tools.Scripts
{
    public partial class IndexRebuilder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowIndexes();
            }
        }

        protected void RebuildBtn_Click(object sender, EventArgs args)
        {
            foreach (ListItem item in cblIndexes.Items)
            {
                if (item.Selected)
                {
                    var options = new JobOptions("RebuildSearchIndex", "index", Sitecore.Client.Site.Name,
                                                 new Builder(item.Value), "Rebuild") { AfterLife = TimeSpan.FromMinutes(1.0) };
                    JobManager.Start(options);
                }
            }
        }

        private IEnumerable<KeyValuePair<string, Index>> GetSearchIndexes()
        {
            var _configuration = Factory.CreateObject("search/configuration", true) as SearchConfiguration;
            return _configuration.Indexes;
        }

        private void ShowIndexes()
        {
            foreach (var index in GetSearchIndexes())
            {
                cblIndexes.Items.Add(new ListItem(index.Key, index.Key));
            }
        }

        public class Builder
        {
            private string _indexName;

            public Builder(string indexName)
            {
                _indexName = indexName;
            }

            public void Rebuild()
            {
                var index = SearchManager.GetIndex(_indexName);
                if (index != null)
                {
                    index.Rebuild();
                    Optimize(false, index.Directory, index.Analyzer);
                }
            }

            protected virtual void Optimize(bool recreate, Directory directory, Analyzer analyzer)
            {
                using (new Sitecore.Data.Indexing.IndexLocker(directory.MakeLock("write.lock")))
                {
                    recreate |= !IndexReader.IndexExists(directory);
                    new IndexWriter(directory, analyzer, recreate).Optimize();
                }
            }
        }
    }
}
