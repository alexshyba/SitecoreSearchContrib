using System;
using System.Linq;
using Sitecore.Data;

namespace scSearchContrib.Searcher.Utilities
{
    public class IdHelper
    {
        /// <summary>
        /// Returns array or strings of IDs passed as a string value. E.g. value of multi-selection fields like Multilist, Treelist etc.
        /// </summary>
        /// <param name="value">A string value that contains IDs separated with '|' character.</param>
        /// <returns>string[]</returns>
        public static string[] ParseId(string value)
        {
            return value.Split(new[] { "|", " ", "," }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Indicates if a string parameter has several GUIDs.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ContainsMultiGuids(string value)
        {
            string[] ids = ParseId(value).Where(ID.IsID).ToArray();
            return ids.Length > 0;
        }

        /// <summary>
        /// Converts GUID into ShortID value.
        /// Multiple GUIDs get separated with a space (" ") character.
        /// </summary>
        /// <param name="value">String that contains GUID</param>
        /// <param name="lowercase">Inticates if output values should be in lowercase.</param>
        /// <returns></returns>
        public static string ProcessGUIDs(string value)
        {
            if (String.IsNullOrEmpty(value))
                return String.Empty;

            if (ID.IsID(value))
            {
                return NormalizeGuid(value);
            }

            if (ContainsMultiGuids(value))
            {
                return string.Join(" ", ParseId(value).Select(NormalizeGuid).ToArray());
            }

            return value;
        }

        /// <summary>
        /// Converts a GUID into ShortID.
        /// </summary>
        /// <param name="guid">String that contains a GUID.</param>
        /// <returns></returns>
        public static string NormalizeGuid(string guid)
        {
            // checking if it is not a short guid
            if (!String.IsNullOrEmpty(guid))
            {
                ShortID result;
                if (ShortID.TryParse(guid, out result)) return result.ToString().ToLowerInvariant();
            }

            return guid;
        }

        /// <summary>
        /// Converts a GUID into ShortID.
        /// </summary>
        /// <param name="guid">Sitecore item ID.</param>
        /// <returns></returns>
        public static string NormalizeGuid(ID guid)
        {
            return NormalizeGuid(guid.ToString());
        }
    }
}
