using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Data;

namespace Utils.ExtensionMethods
{
    public class PageCallable
    {
        public static bool HasValueLength(string s)
        {
            return s.HasValueLength();
        }
    }

    public static class ControlExtensions
    {
        public static string GetRenderedControl(this System.Web.UI.Control control)
        {
            if(control == null)
                return null;

            System.Web.UI.HtmlTextWriter writer = new System.Web.UI.HtmlTextWriter(new System.IO.StringWriter());
            control.RenderControl(writer);
            return writer.InnerWriter.ToString();
        }
    }

    public static class NetClassExtensions
    {
        /// <summary>
        /// Be careful when displayed to client! Local server may not always be what you are looking for!
        /// </summary>
        public static string AbbreviateTimeZone(this System.TimeZoneInfo info)
        {
            string s = info.DisplayName.ToLower().Trim();

            if (s.IndexOf("mountain") != -1)
                return "MST";
            if (s.IndexOf("central") != -1)
                return "CST";
            if (s.IndexOf("pacific") != -1)
                return "PST";
            if (s.IndexOf("eastern") != -1)
                return "EST";
            else
                return s;
        }
    }

    public static class StringExtensions
    {
        /// <summary>
        /// Tells us if the string value is not null and has a trimmed length > 0
        /// </summary>
        public static bool HasValueLength(this string s)
        {
            return (s != null && s.Trim().Length > 0);
        }

        public static string HtmlEncode(this string s)
        {
            s = System.Web.HttpUtility.HtmlEncode(s);
            return s;
        }
    }
}
