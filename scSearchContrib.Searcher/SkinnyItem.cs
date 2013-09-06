namespace scSearchContrib.Searcher
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;

    using Sitecore.Configuration;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Search;

    /// <summary>
    /// The skinny item.
    /// </summary>
    public class SkinnyItem
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnyItem"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="language">
        /// The language.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <param name="databaseName">
        /// The database name.
        /// </param>
        public SkinnyItem(string id, string language, string version, string databaseName)
            : this(new ItemUri(ID.Parse(id), Sitecore.Globalization.Language.Parse(language), Sitecore.Data.Version.Parse(version), databaseName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnyItem"/> class.
        /// </summary>
        /// <param name="itemUri">
        /// The item uri.
        /// </param>
        public SkinnyItem(string itemUri)
            : this(ItemUri.Parse(itemUri))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnyItem"/> class.
        /// </summary>
        /// <param name="itemUri">
        /// The item uri.
        /// </param>
        public SkinnyItem(ItemUri itemUri)
        {
            this.Fields = new NameValueCollection();
            this.RenderedFields = new List<string>();
            this.Uri = itemUri;
            this.Fields.Add(BuiltinFields.Language, this.Uri.Language.Name);
            this.Fields.Add(SearchFieldIDs.Version, this.Uri.Version.Number.ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        public NameValueCollection Fields { get; set; }

        /// <summary>
        /// Gets or sets the uri.
        /// </summary>
        public ItemUri Uri { get; set; }

        /// <summary>
        /// Gets or sets the rendered fields.
        /// </summary>
        public List<string> RenderedFields { get; set; }

        /// <summary>
        /// Gets the item id.
        /// </summary>
        public string ItemID
        {
            get
            {
                return this.Uri.ItemID.ToString();
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return this[BuiltinFields.Name];
            }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        public string Version
        {
            get
            {
                return this.Uri.Version.Number.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets the language.
        /// </summary>
        public string Language
        {
            get
            {
                return this.Uri.Language.Name;
            }
        }

        /// <summary>
        /// Gets the template name.
        /// </summary>
        public string TemplateName
        {
            get
            {
                return this[SearchFieldIDs.TemplateName];
            }
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        public string Path
        {
            get
            {
                return this[SearchFieldIDs.FullContentPath];
            }
        }

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="field">
        /// The field.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string this[string field]
        {
            get
            {
                return this.Fields[field];
            }
        }

        #region Methods

        /// <summary>
        /// The get item.
        /// </summary>
        /// <returns>
        /// The <see cref="Item"/>.
        /// </returns>
        public Item GetItem()
        {
            var db = Factory.GetDatabase(this.Uri.DatabaseName, false);
            if (db == null)
            {
                return null;
            }

            var versionItem = db.GetItem(this.Uri.ToDataUri());

            // handling edge case when an item was published but not updated in the index
            if (versionItem != null && versionItem.Fields.Count > 0)
            {
                return versionItem;
            }

            return db.GetItem(this.Uri.ItemID, this.Uri.Language);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            var itemInformation = string.Format("{0}, {1}, {2}", this.Uri.ItemID, this.Uri.Language, this.Uri.Version);

            foreach (string key in this.Fields.Keys)
            {
                itemInformation += ", " + this.Fields[key];
            }

            return itemInformation;
        }

        #endregion
    }
}
