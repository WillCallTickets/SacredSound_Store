//using System;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//namespace Utils.Controls
//{
//    /// <summary>
//    /// Summary description for SubstringColumn.
//    /// </summary>

//    public class SubstringColumn : BoundColumn
//    {
//        private int subStart = 0;
//        private int subEnd = 0;

//        public int SubStart
//        {
//            get { return subStart; }
//            set { subStart = value; }
//        }
//        public int SubEnd
//        {
//            get { return subEnd; }
//            set { subEnd = value; }
//        }
//        public bool VenueMode
//        {
//            get { return subEnd; }
//            set { subEnd = value; }
//        }

//        protected override string FormatDataValue(object dataValue)
//        {
//            return Truncate(dataValue.ToString());
//        }

//        string Truncate(string input)
//        {
//            if()
//            string output = input;

//            // Check if the string is longer than the allowed amount
//            // otherwise do nothing
//            if (output.Length > characterLimit && characterLimit > 0)
//            {

//                // cut the string down to the maximum number of characters
//                output = output.Substring(0, characterLimit);

//                // Check if the space right after the truncate point 
//                // was a space. if not, we are in the middle of a word and 
//                // need to cut out the rest of it
//                if (input.Substring(output.Length, 1) != " ")
//                {
//                    int LastSpace = output.LastIndexOf(" ");

//                    // if we found a space then, cut back to that space
//                    if (LastSpace != -1)
//                    {
//                        output = output.Substring(0, LastSpace);
//                    }
//                }
//                // Finally, add the "..."
//                output += "...";
//            }
//            return output;
//        }
//    } // End LimitColumn
//} // End myNameSpace

