using System.Linq;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Resources;
using Sitecore.Security.AccessControl;
using Sitecore.SecurityModel;

namespace scSearchContrib.Searcher.Utilities
{
    public class ItemUtil
    {
        /// <summary>
        /// Gets a path to an icon.
        /// </summary>
        /// <param name="item">Sitecore item.</param>
        /// <returns></returns>
        public static string GetIconPath(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            return Themes.MapTheme(item.Appearance.Icon);
        }

        /// <summary>
        /// Checks if an item has explicit security deny rules on it.
        /// </summary>
        /// <param name="item">Sitecore item.</param>
        /// <returns></returns>
        public static bool HasExplicitDenies(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            return item.Security.GetAccessRules().Any(rule => rule.SecurityPermission == SecurityPermission.DenyAccess);
        }

        /// <summary>
        /// Checks if an item has a layout assigned.
        /// </summary>
        /// <param name="item">Sitecore item.</param>
        /// <returns></returns>
        public static bool HasLayout(Item item)
        {
            Assert.ArgumentNotNull(item, "item");

            LayoutField layoutField = item.Fields[FieldIDs.LayoutField];

            if (layoutField != null)
            {
                var isStandardValue = layoutField.InnerField.ContainsStandardValue;
                var isEmpty = !layoutField.InnerField.HasValue;

                return !isStandardValue && !isEmpty;
            }

            return false;
        }

        /// <summary>
        /// Checks if item is in a final workflow state.
        /// </summary>
        /// <param name="item">Sitecore item.</param>
        /// <returns></returns>
        public static bool IsApproved(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            var stateItem = GetStateItem(item);
            return stateItem == null || stateItem[WorkflowFieldIDs.FinalState] == "1";
        }

        /// <summary>
        /// Gets a workflow state item a content item is in.
        /// </summary>
        /// <param name="item">Sitecore content item.</param>
        /// <returns>Gets a workflow state item. Returns null if the item is not in a workflow.</returns>
        public static Item GetStateItem(Item item)
        {
            string stateId = GetStateId(item);
            return stateId.Length > 0 ? GetStateItem(stateId, item.Database) : null;
        }

        /// <summary>
        /// Gets a workflow state item.
        /// </summary>
        /// <param name="stateId">Workflow state item id.</param>
        /// <param name="database">Database the workflow state is in.</param>
        /// <returns></returns>
        public static Item GetStateItem(string stateId, Database database)
        {
            var iD = MainUtil.GetID(stateId, null);
            return iD.IsNull ? null : GetItem(iD, database);
        }

        /// <summary>
        /// Returns a Sitecore item.
        /// </summary>
        /// <param name="itemId">Item ID.</param>
        /// <param name="database">Database the item is in.</param>
        /// <returns></returns>
        public static Item GetItem(ID itemId, Database database)
        {
            return ItemManager.GetItem(itemId, Language.Current, Version.Latest, database, SecurityCheck.Disable);
        }

        /// <summary>
        /// Returns a workflow state item ID.
        /// </summary>
        /// <param name="item">Sitecore content item.</param>
        /// <returns>ID as a string.</returns>
        public static string GetStateId(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            var workflowInfo = item.Database.DataManager.GetWorkflowInfo(item);
            return workflowInfo != null ? workflowInfo.StateID : string.Empty;
        }

        /// <summary>
        /// Returns a full item path in the content tree.
        /// </summary>
        /// <param name="item">Sitecore item.</param>
        /// <returns></returns>
        public static string GetItemPath(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            return item.Paths.FullPath;
        }
    }
}
