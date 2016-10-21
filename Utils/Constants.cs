using System;
using System.Reflection;
using System.Collections.Generic;

namespace Utils
{
	/// <summary>
	/// Summary description for Constants.
	/// </summary>
	public class Constants
	{
        public static List<System.Drawing.Color> ColorList
        {
            get
            {
                //create a generic list of Color
                List<System.Drawing.Color> colors = new List<System.Drawing.Color>();

                //get an array of PropertyInfo using the our known color and specifying the proper binding flags
                PropertyInfo[] propertyInfoList = System.Drawing.Color.Red.GetType().GetProperties(
                                                                   BindingFlags.Static |
                                                                   BindingFlags.DeclaredOnly |
                                                                   BindingFlags.Public);

                //iterate thru the assembly properties
                for (int propertyIndex = 0; propertyIndex < propertyInfoList.Length; propertyIndex++)
                {
                    //retrieve the property info
                    PropertyInfo propertyInfo = (PropertyInfo)propertyInfoList[propertyIndex];
                    //add the color to our list
                    colors.Add((System.Drawing.Color)propertyInfo.GetValue(null, null));
                }

                //return the color list
                return colors;

            }
        }
        private static DateTime _minDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;// DateTime.Parse("1900-01-01 00:00:00.000");
        private static System.Data.SqlTypes.SqlDateTime _nullDate = System.Data.SqlTypes.SqlDateTime.Null;
        //public const int Varchar_MAX_Size = 50000;//somewhat arbitrary

        public static DateTime jsEventHorizon { get { return DateTime.Parse("1/1/1970 12:00 AM GMT"); } }
        /// <summary>
        /// Returns the SQL DateTime MinDate of 1/1/1753 12:00 AM
        /// </summary>
        public static DateTime _MinDate { get { return _minDate;  } }
        public static System.Data.SqlTypes.SqlDateTime _NullDate { get { return _nullDate; } }
        public static string SqlSeparator { get { return "/**************************************************************/"; } }
		public static char Separator { get { return '~'; } }

		/// <summary>
		/// returns the tab character
		/// </summary>
		public const string Tab = "\t";
        public static string Tabs(int depth)
        {
            System.Text.StringBuilder tabs = new System.Text.StringBuilder();
            for (int i = 0; i < depth; i++)
                tabs.Append(Tab);

            return tabs.ToString();
        }

		/// <summary>
		/// returns the carriage return character - appropraite for the system
		/// </summary>
		public static string NewLine = System.Environment.NewLine;
        public static string NewLines(int numberOfInstances)
        {
            //Only return specified # of newLines
            System.Text.StringBuilder lines = new System.Text.StringBuilder();
            for (int i = 0; i < numberOfInstances; i++)
                lines.Append(NewLine);

            return lines.ToString();
        }

        public static System.Web.UI.WebControls.Literal BR
        {
            get
            {
                System.Web.UI.WebControls.Literal l = new System.Web.UI.WebControls.Literal();
                l.Text = "<br />";
                return l;
            }
        }

        #region AddWhitespace
        //uses image as default
        /// <summary>
        /// Add specified amount of whitespace into html stream. Uses an image by default.
        /// </summary>
        /// <param name="numberOfSpaces"></param>
        /// <returns></returns>
        public static string AddWhitespace(int numberOfSpaces)
        {
            return AddWhitespace(numberOfSpaces, true);
        }
        public static string AddWhitespace(int numberOfSpaces, bool useSpaceImage)
        {
            string space = string.Empty;

            if (useSpaceImage)
            {
                //TODO: Add space image path to config
                space = string.Format("<img src=\"Resources/Images/space.gif\" alt=\"\" height=\"1\" width=\"{0}\" border=\"0\" />", numberOfSpaces.ToString());

                return space;
            }
            else
            {
                space = "&nbsp";

                System.Text.StringBuilder sb = new System.Text.StringBuilder(numberOfSpaces * space.Length);

                for (int i = 0; i < numberOfSpaces; i++)
                {
                    sb.AppendFormat("{0}", space);
                }

                return sb.ToString();
            }
        }
        #endregion
	}
}
