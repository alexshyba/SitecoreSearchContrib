#region

using System;
using System.Collections.Generic;
using System.Web.UI;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Jobs;
using Sitecore.Search;
using Sitecore.Text;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;
using Control = Sitecore.Web.UI.HtmlControls.Control;

#endregion

namespace scSearchContrib.Tools
{
    public class Builder
    {
        // Fields
        private readonly ListString databaseNames;

        // Methods
        public Builder(string databaseNames)
        {
            Assert.ArgumentNotNull(databaseNames, "databaseNames");
            this.databaseNames = new ListString(databaseNames);
        }

        protected void Build()
        {
            var job = Context.Job;
            if (job == null) return;
            try
            {
                foreach (var db in databaseNames)
                {
                    if (db.StartsWith("$"))
                    {
                        var index = SearchManager.GetIndex(db.Substring(1));
                        Assert.IsNotNull(index, db.Substring(1));
                        index.Rebuild();
                    }
                    else
                    {
                        var database = Factory.GetDatabase(db, false);
                        if (database == null)
                        {
                            continue;
                        }
                        for (var i = 0; i < database.Indexes.Count; i++)
                        {
                            database.Indexes[i].Rebuild(database);
                            Log.Audit(this, "Rebuild search index: {0}", new[] {database.Name});
                        }
                    }
                    var status = job.Status;
                    status.Processed += 1L;
                }
            }
            catch (Exception exception)
            {
                job.Status.Failed = true;
                job.Status.Messages.Add(exception.ToString());
            }
            job.Status.State = JobState.Finished;
        }
    }

    public class RebuildSearchIndexForm : WizardForm
    {
        // Fields
        protected Memo ErrorText;
        protected Border Indexes;
        protected Memo ResultText;

        public string IndexMap
        {
            get { return StringUtil.GetString(base.ServerProperties["IndexMap"]); }
            set
            {
                Assert.ArgumentNotNull(value, "value");
                base.ServerProperties["IndexMap"] = value;
            }
        }

        // Methods
        protected override void ActivePageChanged(string page, string oldPage)
        {
            Assert.ArgumentNotNull(page, "page");
            Assert.ArgumentNotNull(oldPage, "oldPage");
            base.ActivePageChanged(page, oldPage);
            base.NextButton.Header = "Next >";
            if (page == "Database")
            {
                base.NextButton.Header = "Rebuild >";
            }
            if (page == "Rebuilding")
            {
                base.NextButton.Disabled = true;
                base.BackButton.Disabled = true;
                base.CancelButton.Disabled = true;
                Context.ClientPage.ClientResponse.Timer("StartRebuilding", 10);
            }
        }

        protected override bool ActivePageChanging(string page, ref string newpage)
        {
            Assert.ArgumentNotNull(page, "page");
            Assert.ArgumentNotNull(newpage, "newpage");
            if ((page == "Retry") && (newpage == "Rebuilding"))
            {
                newpage = "Database";
                base.NextButton.Disabled = false;
                base.BackButton.Disabled = false;
                base.CancelButton.Disabled = false;
            }
            return base.ActivePageChanging(page, ref newpage);
        }

        protected virtual void BuildIndexCheckbox(string name, string header, ListString selected, ListString indexMap)
        {
            Assert.ArgumentNotNull(name, "name");
            Assert.ArgumentNotNull(header, "header");
            Assert.ArgumentNotNull(selected, "selected");
            Assert.ArgumentNotNull(indexMap, "indexMap");
            var child = new Checkbox();
            Indexes.Controls.Add(child);
            child.ID = Control.GetUniqueID("dk_");
            child.Header = header;
            child.Value = name;
            child.Checked = selected.Contains(name);
            indexMap.Add(child.ID);
            indexMap.Add(name);
            Indexes.Controls.Add(new LiteralControl("<br />"));
        }

        protected virtual void BuildIndexes()
        {
            var selected = new ListString(Registry.GetString("/Current_User/Rebuild Search Index/Selected"));
            var indexMap = new ListString();
            foreach (var str3 in Factory.GetDatabaseNames())
            {
                var database = Factory.GetDatabase(str3);
                Assert.IsNotNull(database, "database");
                if (database.Indexes.Count > 0)
                {
                    BuildIndexCheckbox(str3, str3, selected, indexMap);
                }
            }
            foreach (var str4 in GetIndexes())
            {
                var name = string.Format("${0}", str4);
                BuildIndexCheckbox(name, str4, selected, indexMap);
            }
            IndexMap = indexMap.ToString();
        }

        protected virtual void CheckStatus()
        {
            var str = Context.ClientPage.ServerProperties["handle"] as string;
            Assert.IsNotNullOrEmpty(str, "handle");
            var job = JobManager.GetJob(Handle.Parse(str));
            if (job == null)
            {
                base.Active = "Retry";
                base.NextButton.Disabled = true;
                base.BackButton.Disabled = false;
                base.CancelButton.Disabled = false;
                ErrorText.Value = "Job has finished unexpectedly";
            }
            else if (job.Status.Failed)
            {
                base.Active = "Retry";
                base.NextButton.Disabled = true;
                base.BackButton.Disabled = false;
                base.CancelButton.Disabled = false;
                ErrorText.Value = StringUtil.StringCollectionToString(job.Status.Messages);
            }
            else
            {
                string str2;
                if (job.Status.State == JobState.Running)
                {
                    str2 = Translate.Text("Processed {0} items. ", new object[] {job.Status.Processed, job.Status.Total});
                }
                else
                {
                    str2 = Translate.Text("Queued.");
                }
                if (job.IsDone)
                {
                    base.Active = "LastPage";
                    base.BackButton.Disabled = true;
                    ResultText.Value = StringUtil.StringCollectionToString(job.Status.Messages);
                }
                else
                {
                    Context.ClientPage.ClientResponse.SetInnerHtml("Status", str2);
                    Context.ClientPage.ClientResponse.Timer("CheckStatus", 500);
                }
            }
        }

        protected virtual IEnumerable<string> GetIndexes()
        {
            var configuration = Factory.CreateObject("search/configuration", true) as SearchConfiguration;
            if (configuration != null)
            {
                return configuration.Indexes.Keys;
            }
            return new List<string>();
        }

        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            base.OnLoad(e);
            if (!Context.ClientPage.IsEvent)
            {
                BuildIndexes();
            }
        }

        protected virtual void StartRebuilding()
        {
            var str = new ListString();
            var str2 = new ListString(IndexMap);
            foreach (string str3 in Context.ClientPage.ClientRequest.Form.Keys)
            {
                if (!string.IsNullOrEmpty(str3) && str3.StartsWith("dk_"))
                {
                    var index = str2.IndexOf(str3);
                    if (index >= 0)
                    {
                        str.Add(str2[index + 1]);
                    }
                }
            }
            Registry.SetString("/Current_User/Rebuild Search Index/Selected", str.ToString());
            var options2 = new JobOptions("RebuildSearchIndex", "index", Client.Site.Name, new Builder(str.ToString()),
                                          "Build")
                               {
                                   AfterLife = TimeSpan.FromMinutes(1.0),
                                   ContextUser = Context.User
                               };
            var options = options2;
            var job = JobManager.Start(options);
            Context.ClientPage.ServerProperties["handle"] = job.Handle.ToString();
            Context.ClientPage.ClientResponse.Timer("CheckStatus", 500);
        }
    }
}