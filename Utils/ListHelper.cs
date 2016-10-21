using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Text;

namespace Utils
{
	/// <summary>
	/// Summary description for FillList.
	/// </summary>
	public class ListHelper
	{
        /// <summary>
        /// Requires that the ddl be ddlMonth or ddlYear
        /// </summary>
        /// <param name="ddl"></param>
        public static void FillExpiryList_Month(DropDownList ddl)
        {
            FillExpiryList(ddl, 0);
        }
        public static void FillExpiryList_Year(DropDownList ddl, int maxExpiryYears)
        {
            if (maxExpiryYears <= 0)
                throw new ArgumentOutOfRangeException("maxExpiryYears must be greater then zero.");

            FillExpiryList(ddl, maxExpiryYears);
        }
        private static void FillExpiryList(DropDownList ddl, int maxExpiryYears)
        {
            if (ddl.Items.Count == 0)
            {
                ddl.Items.Add(new ListItem("--", string.Empty));
                int start = (maxExpiryYears == 0) ? 1 : DateTime.Now.Year;
                int end = (maxExpiryYears == 0) ? 13 : (start + maxExpiryYears);

                for (int i = start; i < end; i++)
                    ddl.Items.Add(new ListItem(i.ToString()));
            }
        }
        public static void FillCountryList(DropDownList ddl)
        {
            if (ddl.Items.Count == 0)
            {
                ddl.DataSource = Utils.ListFiller.CountryListing;
                ddl.DataTextField = "Text";
                ddl.DataValueField = "Value";
                ddl.DataBind();
            }
        }

        //protected void ddlMonth_DataBinding(object sender, EventArgs e)
        //{
        //    DropDownList ddl = (DropDownList)sender;
        //    List<ListItem> list = new List<ListItem>();

        //    string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        //    for (int i = 0; i < months.Length; i++ )
        //        list.Add(new ListItem(months[i], (i+1).ToString()));

        //    ddl.DataSource = list;
        //    ddl.DataTextField = "Text";
        //    ddl.DataValueField = "Value";
        //}

		public static void MakeStateList(DropDownList list)
		{
			MakeStateList(list, "Select", "X");
		}
		public static void MakeStateList(DropDownList list, string listTop, string defaultVal)
		{
			list.Items.Clear();
            
			//list.Items.Add(new ListItem());
			list.Items.Add(new ListItem(listTop, defaultVal));
			list.Items.Add(new ListItem("AA","AA"));
			list.Items.Add(new ListItem("AB","AB"));
			list.Items.Add(new ListItem("AE","AE"));
			list.Items.Add(new ListItem("AK","AK"));
			list.Items.Add(new ListItem("AL","AL"));
			list.Items.Add(new ListItem("AP","AP"));
			list.Items.Add(new ListItem("AR","AR"));			
			list.Items.Add(new ListItem("AS","AS"));
			list.Items.Add(new ListItem("AZ","AZ"));			
			list.Items.Add(new ListItem("BC","BC"));
			list.Items.Add(new ListItem("CA","CA"));
			list.Items.Add(new ListItem("CO","CO"));
			list.Items.Add(new ListItem("CT","CT"));
			list.Items.Add(new ListItem("DC","DC"));
			list.Items.Add(new ListItem("DE","DE"));
			list.Items.Add(new ListItem("FL","FL"));
			list.Items.Add(new ListItem("FM","FM"));
			list.Items.Add(new ListItem("GA","GA"));
			list.Items.Add(new ListItem("GU","GU"));
			list.Items.Add(new ListItem("HI","HI"));
			list.Items.Add(new ListItem("ID","ID"));
			list.Items.Add(new ListItem("IL","IL"));
			list.Items.Add(new ListItem("IN","IN"));
			list.Items.Add(new ListItem("IA","IA"));
			list.Items.Add(new ListItem("KS","KS"));
			list.Items.Add(new ListItem("KY","KY"));
			list.Items.Add(new ListItem("LA","LA"));
			list.Items.Add(new ListItem("MA","MA"));
			list.Items.Add(new ListItem("MB","MB"));
			list.Items.Add(new ListItem("MD","MD"));
			list.Items.Add(new ListItem("ME","ME"));
			list.Items.Add(new ListItem("MH","MH"));			
			list.Items.Add(new ListItem("MI","MI"));
			list.Items.Add(new ListItem("MN","MN"));
			list.Items.Add(new ListItem("MO","MO"));
			list.Items.Add(new ListItem("MP","MP"));
			list.Items.Add(new ListItem("MS","MS"));			
			list.Items.Add(new ListItem("MT","MT"));
			list.Items.Add(new ListItem("NB","NB"));
			list.Items.Add(new ListItem("NC","NC"));
			list.Items.Add(new ListItem("ND","ND"));
			list.Items.Add(new ListItem("NE","NE"));
			list.Items.Add(new ListItem("NF","NF"));
			list.Items.Add(new ListItem("NH","NH"));
			list.Items.Add(new ListItem("NJ","NJ"));
			list.Items.Add(new ListItem("NM","NM"));
			list.Items.Add(new ListItem("NS","NS"));
			list.Items.Add(new ListItem("NT","NT"));
			list.Items.Add(new ListItem("NV","NV"));			
			list.Items.Add(new ListItem("NY","NY"));
			list.Items.Add(new ListItem("OH","OH"));
			list.Items.Add(new ListItem("OK","OK"));
			list.Items.Add(new ListItem("ON","ON"));
			list.Items.Add(new ListItem("OR","OR"));			
			list.Items.Add(new ListItem("PA","PA"));
			list.Items.Add(new ListItem("PE","PE"));
			list.Items.Add(new ListItem("PR","PR"));
			list.Items.Add(new ListItem("PW","PW"));
			list.Items.Add(new ListItem("QC","QC"));
			list.Items.Add(new ListItem("RI","RI"));
			list.Items.Add(new ListItem("SK","SK"));
			list.Items.Add(new ListItem("SC","SC"));
			list.Items.Add(new ListItem("SD","SD"));
			list.Items.Add(new ListItem("TN","TN"));
			list.Items.Add(new ListItem("TX","TX"));
			list.Items.Add(new ListItem("UT","UT"));			
			list.Items.Add(new ListItem("VA","VA"));
			list.Items.Add(new ListItem("VI","VI"));
			list.Items.Add(new ListItem("VT","VT"));
			list.Items.Add(new ListItem("WA","WA"));
			list.Items.Add(new ListItem("WI","WI"));
			list.Items.Add(new ListItem("WV","WV"));			
			list.Items.Add(new ListItem("WY","WY"));
			list.Items.Add(new ListItem("YK","YK"));
		}

        //public static bool HasItemInArrayListObject(ArrayList list, object toFind)
        //{
        //    foreach(object obj in list)
        //    {
        //        if(obj == toFind)
        //            return true;
        //    }
        //    return false;
        //}

        //public static bool HasItemInArrayListText(ArrayList list, string toFind)
        //{
        //    //works with arraylists containing an array of list items
        //    foreach(ListItem li in list)
        //    {
        //        if(li.Text.ToLower() == toFind.ToLower())
        //            return true;
        //    }

        //    return false;
        //}
        //public static bool HasItemInArrayListValue(ArrayList list, string toFind)
        //{
        //    //works with arraylists containing an array of list items
        //    foreach(ListItem li in list)
        //    {
        //        if(li.Value.ToString().ToLower() == toFind.ToLower())
        //            return true;
        //    }

        //    return false;
        //}

        //public static void AddValueToArrayListItem(ArrayList list, string toFind, string toAdd)
        //{	
        //    foreach(ListItem li in list)
        //    {
        //        if(li.Text == toFind)
        //            li.Value += "," + toAdd;
        //    }
        //}

        //public static bool HasItemInTriplet(ArrayList list, string toFind)
        //{
        //    //works with arraylists containing an array of list items
        //    foreach(System.Web.UI.Triplet t in list)
        //    {
        //        if(((string)t.Second).Equals(toFind))
        //            return true;
        //    }

        //    return false;
        //}
        //public static void AddValueToTriplet(ArrayList list, string idx, string text, string toAdd)
        //{	
        //    foreach(System.Web.UI.Triplet t in list)
        //    {
        //        if((string)t.Second == text)
        //        {
        //            string[] ids = ((string)t.First).Split(',');
        //            if(ids[0] != null)
        //                t.First += string.Format(",{0}", idx);

        //            t.Third += string.Format(",{0}", toAdd);
        //        }
        //    }
        //}

		public static DateTime FormatProperShowDate(string sDate, string sTime, string sAmPm)
		{
			//client must validate values
			return DateTime.Parse(sDate + " " + FormatProperTimeStamp(sTime, sAmPm));
		}

		public static DateTime FormatProperShowDate(DateTime dDate, string sTime, string sAmPm)
		{
			//client must validate values
			return DateTime.Parse(dDate.ToShortDateString() + " " + FormatProperTimeStamp(sTime, sAmPm));
		}

		public static string FormatProperTimeStamp(string sTime, string sAmPm)
		{
			//this will return a string in the time format x:00 PM (or AM)
			if(Validation.Validate12HourTime(sTime))
			{
				StringBuilder sb = new StringBuilder();

				sb.Append(sTime);
				if(sTime.IndexOf(":") == -1) sb.Append(":00");

				sb.Append(" ");

				sb.Append(sAmPm.ToUpper());

				return sb.ToString();
			}

			return null;
		}

		public static void FillDropDownFromArrayList(DropDownList list, ArrayList items, string selectedItem)
		{
			list.Items.Clear();

			for(int i = 0; i < items.Count; i++)
			{
				ListItem li = (ListItem)items[i];
				if (li.Text == selectedItem || li.Value == selectedItem) li.Selected = true;

				list.Items.Add(li);
			}
		}

		public static void FillListBoxFromArrayList(ListBox list, ArrayList items, string selectedItem)
		{
			list.Items.Clear();

			for(int i = 0; i < items.Count; i++)
			{
				ListItem li = (ListItem)items[i];
				if (li.Value == selectedItem) li.Selected = true;

				list.Items.Add(li);
			}
		}

		public static void RearrangeListBox(ListBox lb, string idx, string movement)
		{
			ArrayList al = new ArrayList(lb.Items);
			ListItem temp = new ListItem();
			ListItem curr = new ListItem();
			int size = 0;

			switch (movement.ToLower())
			{
				case "up":										
					size = al.Count;
					temp = (ListItem)al[0];

					for(int i = 0; i < size; i++)
					{
						curr = (ListItem)al[i];

						if(curr.Value == idx)
						{
							al[i-1] = curr;
							al[i] = temp;
						}
						temp = curr;
					}
					break;

				case "down":					
					size = al.Count;
					temp = (ListItem)al[size-1];

					for(int i = size-1; i >= 0; i--)
					{
						curr = (ListItem)al[i];

						if(curr.Value == idx)
						{
							al[i+1] = curr;
							al[i] = temp;
						}
						temp = curr;
					}
					break;
			}

			int j = 0;
			lb.Items.Clear();
			foreach(ListItem li in al)
			{
				Debug.WriteLine(li.Value);
				lb.Items.Add(li);
				if(li.Value == idx)
					lb.Items[j].Selected = true;
				j++;
			}
		}
	}
}
