namespace scSearchContrib.Crawler.FieldCrawlers
{
    using System.Collections.Generic;

    /// <summary>
    /// The MultivaluedFieldCrawler interface.
    /// </summary>
    public interface IMultivaluedFieldCrawler
    {
        /// <summary>
        /// The get values.
        /// </summary>
        /// <returns>
        /// The IEnumerable of values.
        /// </returns>
        IEnumerable<string> GetValues();
    }
}
