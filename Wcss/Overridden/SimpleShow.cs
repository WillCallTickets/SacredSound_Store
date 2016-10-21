using System;
using System.Collections.Generic;
using System.Text;

namespace Wcss
{
    #region SimpleShow

    public partial class SimpleShow
        {
            public static readonly string flagDelimiter = "~";
            public static readonly string flagSeparator = "^";
            public static readonly string flagString = "Date~Status~Headliner~Opener~Venue^";
            public static string RemoveDelimiterAndSeparator(string s) { return s.Replace(SimpleShow.flagDelimiter, string.Empty).Replace(SimpleShow.flagSeparator, string.Empty); }

            private int _idx = -1;
            public int IDX { get { return _idx; } set { _idx = value; } }
            private string _date = null;
            public string DATE { get { return _date; } set { _date = value; } }
            private string _status = null;
            public string STATUS { get { return _status; } set { _status = value; } }
            private string _headliner = null;
            public string HEADLINER { get { return _headliner; } set { _headliner = value; } }
            private string _opener = null;
            public string OPENER { get { return _opener; } set { _opener = value; } }
            private string _venue = null;
            public string VENUE { get { return _venue; } set { _venue = value; } }

            public SimpleShow(string date, string status, string headliner, string opener, string venue)
            {
                DATE = date;
                STATUS = status;
                HEADLINER = headliner;
                OPENER = opener;
                VENUE = venue;
            }
            public SimpleShow(int idx)
            {
                IDX = idx;
                DATE = string.Empty;
                STATUS = string.Empty;
                HEADLINER = string.Empty;
                OPENER = string.Empty;
                VENUE = string.Empty;
            }
            public SimpleShow(int idx, string line)
            {
                IDX = idx;

                if(line.EndsWith(SimpleShow.flagSeparator))
                    line = line.Remove(line.Length-SimpleShow.flagSeparator.Length);

                string[] parts = line.Split(SimpleShow.flagDelimiter.ToCharArray());
                DATE = parts[0];
                STATUS = parts[1];
                HEADLINER = parts[2];
                OPENER = parts[3];
                VENUE = parts[4];
            }

            public static List<SimpleShow> ShowList(string content)
            {
                return ShowList(content, SimpleShow.flagString);
            }
            public static List<SimpleShow> ShowList(string content, string flags)
            {
                List<SimpleShow> list = new List<SimpleShow>();

                if (flags != null && flags == SimpleShow.flagString &&
                    content != null && content.TrimEnd().Length > 0)
                {
                    //remove the last separator - note that we do not want to trim end chars as that may go back further than we would like
                    if(content.EndsWith(SimpleShow.flagSeparator))
                        content = content.Remove(content.Length-SimpleShow.flagSeparator.Length);

                    string[] delims = new string[1];
                    delims[0] = SimpleShow.flagSeparator;
                    string[] lines = content.Split(delims, StringSplitOptions.None);

                    foreach (string line in lines)
                    {
                        if (line.TrimEnd().Length > 0)
                        {
                            SimpleShow shw = new SimpleShow(list.Count, line);
                            list.Add(shw);
                        }
                    }
                }

                return list;
            }
            public static string ListToContent(List<SimpleShow> list)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                if (list.Count > 0)
                {
                    list.Sort(new Utils.Reflector.CompareEntities<SimpleShow>(Utils.Reflector.Direction.Ascending, "IDX"));

                    foreach (SimpleShow shw in list)
                        sb.Append(shw.ToString());
                }

                return sb.ToString();
            }
            public static string ReorderItem(List<SimpleShow> list, int currentOrdinal, string direction)
            {
                int moveToIndex = 0;

                direction = direction.Trim().ToLower();

                switch (direction)
                {
                    case"up"://towards the top of the list - 0
                    case "down"://towards the bottom of the list - list.Count-1
                        //dont move up more than max
                        if((direction == "up" && currentOrdinal > 0) || (direction == "down" && currentOrdinal < list.Count -1))
                        {
                            //swap the items
                            //set the new index
                            moveToIndex = (direction.Trim().ToLower() == "up") ? currentOrdinal -1 : currentOrdinal + 1;

                            //overwrite destination row
                            string lne = list[moveToIndex].ToString();
                            SimpleShow moveTo = new SimpleShow(currentOrdinal, lne);
                            list[moveToIndex] = moveTo;

                            //overwrite current row
                            lne = list[currentOrdinal].ToString();
                            list[currentOrdinal] = new SimpleShow(moveToIndex, lne);

                            //sort on index
                            list.Sort(new Utils.Reflector.CompareEntities<SimpleShow>(Utils.Reflector.Direction.Ascending, "IDX"));
                        }
                        break;
                }                

                //return the new index
                string listed = SimpleShow.ListToContent(list);
                return listed;
            }

            /// <summary>
            /// DATE~STATUS~HEADLINER~OPENER~VENUE~~
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return string.Format("{0}{6}{1}{6}{2}{6}{3}{6}{4}{5}",
                    (this.DATE != null) ? this.DATE.Trim() : string.Empty,
                    (this.STATUS != null) ? this.STATUS.Trim() : string.Empty,
                    (this.HEADLINER != null) ? this.HEADLINER.Trim() : string.Empty,
                    (this.OPENER != null) ? this.OPENER.Trim() : string.Empty,
                    (this.VENUE != null) ? this.VENUE.Trim() : string.Empty,
                    SimpleShow.flagSeparator,
                    SimpleShow.flagDelimiter);
            }
        }

    #endregion
}
