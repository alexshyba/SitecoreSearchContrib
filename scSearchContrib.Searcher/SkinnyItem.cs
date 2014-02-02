namespace scSearchContrib.Searcher
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Globalization;

    using Sitecore.Configuration;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.Converters;
    using Sitecore.ContentSearch.SearchTypes;

    /// <summary>
    /// The skinny item.
    /// </summary>
    public class SkinnyItem
    {
        /// <summary>
        /// The fields.
        /// </summary>
        private readonly Dictionary<string, object> fields = new Dictionary<string, object>();

        #region Construction

        public SkinnyItem()
        {
            
        }

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
            this.Uri = itemUri;
        }

        #endregion

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        public Dictionary<string, object> Fields { get { return this.fields; } }

        /// <summary>Gets the item id.</summary>
        [IndexField(Sitecore.ContentSearch.BuiltinFields.Group)]
        [TypeConverter(typeof(IndexFieldIDValueConverter))]
        public virtual ID ItemID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        [IndexField(Sitecore.ContentSearch.BuiltinFields.Content)]
        public virtual string Content
        {
            get;
            set;
        }

        /// <summary>Gets the item id.</summary>
        [IndexField(Sitecore.ContentSearch.BuiltinFields.Group)]
        [TypeConverter(typeof(IndexFieldIDValueConverter))]
        public virtual ID ItemId
        {
            get;
            set;
        }

        /// <summary>Gets or sets the language.</summary>
        [IndexField(Sitecore.ContentSearch.BuiltinFields.Language)]
        public virtual string Language
        {
            get;
            set;
        }

        /// <summary>Gets or sets the name.</summary>
        [IndexField(Sitecore.ContentSearch.BuiltinFields.Name)]
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>Gets or sets the uri.</summary>
        [IndexField(Sitecore.ContentSearch.BuiltinFields.UniqueId)]
        [TypeConverter(typeof(IndexFieldItemUriValueConverter))]
        public virtual ItemUri Uri { get; set; }

        /// <summary>Gets or sets the version.</summary>
        public virtual string Version
        {
            get
            {
                if (this.Uri == null)
                {
                    this.Uri = new ItemUri(this[Sitecore.ContentSearch.BuiltinFields.UniqueId]);
                }

                return this.Uri.Version.Number.ToString(CultureInfo.InvariantCulture);
            }

            set
            {
            }
        }

        [IndexField(BuiltinFields.Database)]
        public virtual string DatabaseName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the template name.
        /// </summary>
        [IndexField(Sitecore.ContentSearch.BuiltinFields.TemplateName)]
        public string TemplateName
        {
            get;
            set;
        }

        /// <summary>Gets or sets the template id.</summary>
        [IndexField(BuiltinFields.Template)]
        [TypeConverter(typeof(IndexFieldIDValueConverter))]
        public virtual ID TemplateId
        {
            get;
            set;
        }

        [IndexField(BuiltinFields.Links)]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public virtual IEnumerable<ID> Links
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the path.
        /// </summary>]
        [IndexField(Sitecore.ContentSearch.BuiltinFields.FullPath)]
        public virtual string Path
        {
            get;
            set;
        }

        [IndexField(BuiltinFields.Path)]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public virtual IEnumerable<ID> Paths
        {
            get;
            set;
        }

        public virtual string this[string key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException("key");

                return this.fields[key.ToLowerInvariant()].ToString();
            }

            set
            {
                if (key == null)
                    throw new ArgumentNullException("key");

                this.fields[key.ToLowerInvariant()] = value;
            }
        }

        /// <summary>Gets the <see cref="System.String"/> with the specified key.</summary>
        /// <param name="key">The key.</param>
        /// <returns>The result.</returns>
        public virtual object this[ObjectIndexerKey key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException("key");

                return this.fields[key.ToString().ToLowerInvariant()];
            }

            set
            {
                if (key == null)
                    throw new ArgumentNullException("key");

                this.fields[key.ToString().ToLowerInvariant()] = value;
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
