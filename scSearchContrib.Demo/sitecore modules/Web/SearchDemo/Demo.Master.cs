using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Diagnostics;
using Sitecore.Caching;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using scSearchContrib.Demo;
using scSearchContrib.Searcher;
using scSearchContrib.Searcher.Utilities;

namespace SearchDemo.Scripts
{
    public partial class DemoMaster : System.Web.UI.MasterPage
    {
        protected string IndexName
        {
            get { return IndexNameList.SelectedValue; }
        }

        protected string Language
        {
            get { return LanguageList.SelectedValue; }
        }

        protected string LocationFilter
        {
            get { return RootItemId.Value; }
        }

        protected string DatabaseName
        {
            get { return DatabaseNameTextBox.Value; }
        }

        protected string TemplateFilter
        {
            get { return TemplateFilterTextBox.Text; }
        }

        protected string FullTextQuery
        {
            get { return FullTextQueryTextBox.Text; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            InitializeRoots();
            InitializeList();
            InitializeLanguages();
        }

        private void InitializeList()
        {
            // TODO: figure out something better
            IndexNameList.Items.Add("demo");
            IndexNameList.Items.Add("simple");
        }

        protected virtual void RunButton_Click(object sender, EventArgs e)
        {
            var items = new List<SkinnyItem>();
            var stopwatch = new Stopwatch();
            try
            {
                stopwatch.Start();

                items.AddRange(GetItems());

                if (FullItemOutputCheckbox.Checked)
                {
                    var fullItems = SearchHelper.GetItemListFromInformationCollection(items);
                    stopwatch.Stop();
                    RenderItemDetails(fullItems);
                }
                else
                {
                    stopwatch.Stop();
                    RenderItemDetails(items);
                }
            }
            catch (Exception)
            {
                ResultLabel.Text = "There was an error running search.";
                throw;
            }
            finally
            {
                stopwatch.Stop();
            }

            RenderTimingSummary(items.Count, stopwatch.Elapsed);
        }

        protected virtual void RenderItemDetails(IEnumerable<SkinnyItem> skinnyItems)
        {
            ResultLabel.Text = String.Empty;

            if (VerboseOutputCheckbox.Checked)
            {
                ResultLabel.Text = "<ol>";

                foreach (var skinnyItem in skinnyItems)
                {
                    ResultLabel.Text += String.Format("<li>{0}</li>", skinnyItem.Uri);
                }

                ResultLabel.Text += "</ol>";
            }
        }

        protected virtual void RenderItemDetails(IEnumerable<Item> items)
        {
            ResultLabel.Text = String.Empty;

            if (VerboseOutputCheckbox.Checked)
            {
                ResultLabel.Text = "<ol>";

                foreach (var item in items.Where(item => item != null))
                {
                    ResultLabel.Text += String.Format("<li>{0}</li>", item.Paths.FullPath);
                }

                ResultLabel.Text += "</ol>";
            }
        }

        protected virtual void RenderTimingSummary(int count, TimeSpan ts)
        {
            TimingLabel.Text = String.Empty;

            TimingLabel.Text += String.Format("<br />Processed {0} results", count);
            var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);

            TimingLabel.CssClass = "normal-timing";

            if (ts.Ticks > int.Parse(Threshold.Text) * 10000)
            {
                TimingLabel.CssClass = "exceeded-timing";
            }


            TimingLabel.Text += String.Format("<br />Fetching done in {0}<br />", elapsedTime);
        }

        protected virtual void InitializeRoots()
        {
            //var db = Sitecore.Context.Database;

            //var rootItem = db.GetItem(ItemIDs.ContentRoot);

            //if (rootItem != null)
            //{
            //   RootItemList.Items.Add(new ListItem { Text = rootItem.Paths.FullPath, Value = rootItem.ID.ToString(), Selected = true });

            //   foreach (Item subitem in rootItem.Children)
            //   {
            //      RootItemList.Items.Add(new ListItem { Text = subitem.Paths.FullPath, Value = subitem.ID.ToString() });
            //   }
            //}
        }

        protected virtual void InitializeIndexes()
        {
            // TODO
            //for(int i=0; i < SearchManager.IndexCount; i++)
            //{
            //    SearchManager.
            //}

            //var db = Sitecore.Context.Database;

            //var rootItem = db.GetItem(ItemIDs.ContentRoot);

            //if (rootItem != null)
            //{
            //   RootItemList.Items.Add(new ListItem { Text = rootItem.Paths.FullPath, Value = rootItem.ID.ToString(), Selected = true });

            //   foreach (Item subitem in rootItem.Children)
            //   {
            //      RootItemList.Items.Add(new ListItem { Text = subitem.Paths.FullPath, Value = subitem.ID.ToString() });
            //   }
            //}
        }


        protected virtual void InitializeLanguages()
        {
            LanguageList.Items.Clear();

            LanguageList.Items.Add(new ListItem { Text = Sitecore.Context.Language.GetDisplayName(), Value = Sitecore.Context.Language.Name, Selected = true });

            var languages = LanguageManager.GetLanguages(Sitecore.Context.Database);

            foreach (var language in languages.Where(language => !language.Equals(Sitecore.Context.Language)))
            {
                LanguageList.Items.Add(new ListItem { Text = language.GetDisplayName(), Value = language.Name, Selected = false });
            }
        }

        protected virtual void CacheClearButton_Click(object sender, EventArgs e)
        {
            foreach (var cache in CacheManager.GetAllCaches())
            {
                cache.Clear();
            }
            Response.Write("All caches cleared!");
        }

        protected virtual List<SkinnyItem> GetItems()
        {
            return ((BaseDemoPage)mainPH.Page).GetItems(DatabaseName, IndexName, Language, TemplateFilter, LocationFilter, FullTextQuery);
        }
    }
}