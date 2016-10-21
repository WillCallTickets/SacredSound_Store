using System;
using System.Diagnostics;
using System.Configuration;
using System.Collections;
using System.Data.SqlTypes;

using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.IO;
using System.Net;

using Wcss;
using Utils;
using Newtonsoft.Json;
using SubSonic;

using Wcss.QueryRow;

namespace TestApp
{
	/// <summary>
	/// This test app can be used to test your new Biz object.
	/// Please make sure that the test app is set as the startup Project.
	/// </summary>
	class TestApp
	{

        //protected static void ExportShopify()
        //{
        //    ShopifyExport exp = new ShopifyExport(true);

        //    var o = "p";
        //}

        //protected static void PlayRefund()
        //{

        //}

        //protected static void PlayMeta()
        //{

            //string result = "<br/> <br/>  <br/>Instructions for credit redemption<br/><br/> <br/>  <br/>   You may redeem your store credit by following the steps below<br/>  <br/>   Login to your account at http://sts9store.com/WebUser/Default.aspx?p=credit<br/>  <br/>   Enter the code for the item from your confirmation email or invoice.<br/>  <br/>   Click the &quot;redeem code&quot;&nbsp;button to redeem your credit!<br/> <br/><br/>";

            //result = Regex.Replace(result.Trim(), @"(<br\/>\s+)+", "<br/>");
            //result = Regex.Replace(result.Trim(), @"(<br\/>)+", "<br/>");
            //result = Regex.Replace(result.Trim(), @"(^<br\/>)|(<br\/>$)", "");


            //string f = "LavaBlastManyManyList";

            //SalePromotion sp = new SalePromotion(10231);

            //List<System.Web.UI.Triplet> AmountRewardTier = new List<System.Web.UI.Triplet>();
            //AmountRewardTier.Add(new System.Web.UI.Triplet(25,5,0));
            //AmountRewardTier.Add(new System.Web.UI.Triplet(50,15,1));
            //AmountRewardTier.Add(new System.Web.UI.Triplet(100,40,2));

            /*
            SalePromotionMeta bob = new SalePromotionMeta();
            bob.AmountRewardTier = new List<System.Web.UI.Triplet>();
            bob.AmountRewardTier.Add(new System.Web.UI.Triplet(25, 5, 0));
            bob.AmountRewardTier.Add(new System.Web.UI.Triplet(50, 15, 1));
            bob.AmountRewardTier.Add(new System.Web.UI.Triplet(100, 40, 2));

            bob.pickle = "gerkin";

            string json = JsonConvert.SerializeObject(bob, Formatting.Indented);
            
            sp.JsonMeta = json;
            sp.Save();
            */
            

        //}


        //protected static void ExamineFiles()
        //{
        //    int count = 0;

        //    string baseDir = @"D:\Users\rob\Music\GD Europe 72";

        //    DirectoryInfo di = new DirectoryInfo(baseDir);

        //    foreach(DirectoryInfo d in di.GetDirectories())
        //    {
        //        foreach (FileInfo fi in d.GetFiles("*.flac"))
        //        {
        //            string s = fi.FullName;
        //            string p = Path.GetFileName(fi.FullName);
        //            //if it matches the pattern 
        //            string pattern = " (%Y)";
        //            if (p.IndexOf(pattern) != -1)
        //            {
        //                //make and map destination 
        //                string newFileName = p.Replace(pattern, string.Empty);                        
        //                string dest = string.Format("{0}\\{1}", Path.GetDirectoryName(fi.FullName), newFileName);
        //                //make a copy
                        
        //                File.Copy(s, dest);
        //                //delete the original
        //                File.Delete(s);
        //            }

        //            //count++;
        //            //if (count >= 1)
        //            //    return;
        //        }

        //    }


        //}

        //protected static void TestAlpha()
        //{
        //    List<string> users = new List<string>();
        //    users.Add("bob Sanders");
        //    users.Add("bob_sandford");
        //    users.Add("bob-sandford");
        //    users.Add("bob Sander's");
        //    users.Add(" bob Sander's");
        //    users.Add(" bob Sa876r's");
        //    users.Add(" bob S%$#$76r's");
        //    users.Add("bob Sander's ");
        //    users.Add("bob S@ander's ");
        //    users.Add("bob Sande[r's ");
        //    /*users.Add("bob.terminal1.bob@sts9.com");
        //    users.Add("bob.terbobminal1.bob@ststerminal9.com");
        //    users.Add("bob.terminal1456.bob@sts9.com");
        //    users.Add("bob.terminal_1.bob@sts9.com");
        //    users.Add("bob.terminal__1.bob@sts9.com");
        //    users.Add("bob.terminalbob@sts9.com");
        //    users.Add("bob.terminal_bob@sts9.com");
        //    users.Add("bob.terminalbob76@sts9.com");
        //    users.Add("bob.terminal_bob987@sts9.com");
        //    users.Add("bob.terminal_8_1.bob@sts9.com");
        //    users.Add("bob.terminal_a_1.bob@sts9.com");
        //    */
        //    Regex regterm = new Regex(@"^[a-zA-Z0-9_\-\'\s]*$", RegexOptions.IgnoreCase);
        //    //Regex regterm = new Regex(@"[a-zA-Z0-9_\-\'\s]*", RegexOptions.IgnoreCase);
        //    //Regex regterm = new Regex(@"terminal_\d+", RegexOptions.IgnoreCase);
        //    List<string> successes = new List<string>();

        //    foreach (string s in users)
        //    {
        //        Match m = regterm.Match(s);

        //        if (m.Success)
        //            successes.Add(string.Format("[{0}] matches the pattern.", s));
        //    }

        //    //evaluate fails
        //    foreach (string s in successes)
        //        Debug.WriteLine(s);
        //}

        //protected static void TestTerminalSignup()
        //{
        //    List<string> users = new List<string>();
        //    users.Add("terminal1@sts9.com");
        //    users.Add("bobterminal1@sts9.com");
        //    users.Add("bob.terminal1@sts9.com");
        //    users.Add("bobterminal1bob@sts9.com");
        //    users.Add("bob.terminal1.bob@sts9.com");
        //    users.Add("bob.terbobminal1.bob@ststerminal9.com");
        //    users.Add("bob.terminal1456.bob@sts9.com");
        //    users.Add("bob.terminal_1.bob@sts9.com");
        //    users.Add("bob.terminal__1.bob@sts9.com");
        //    users.Add("bob.terminalbob@sts9.com");
        //    users.Add("bob.terminal_bob@sts9.com");
        //    users.Add("bob.terminalbob76@sts9.com");
        //    users.Add("bob.terminal_bob987@sts9.com");
        //    users.Add("bob.terminal_8_1.bob@sts9.com");
        //    users.Add("bob.terminal_a_1.bob@sts9.com");

        //    Regex regterm = new Regex(@"terminal_*\d+", RegexOptions.IgnoreCase);
        //    //Regex regterm = new Regex(@"terminal_\d+", RegexOptions.IgnoreCase);
        //    List<string> fails = new List<string>();

        //    foreach (string s in users)
        //    {
        //        string[] parts = s.Split('@');
        //        string usernamePart = parts[0];
        //        Match m = regterm.Match(usernamePart);

        //        if (m.Success)
        //            fails.Add(string.Format("{0} matches the pattern.", s));
        //    }

        //    //evaluate fails
        //    foreach (string s in fails)
        //        Debug.WriteLine(s);
        //}

        /// <summary>
		/// The main entry point for the application.
		/// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            string procId = "33314";//todo - ensure trim

            string procIdTrunc = (procId.Length >= 4) ?
                string.Format("x{0}", procId.Substring(procId.Length - 4, 4)) :
                (procId.Length == 0) ? "NA" : string.Format("x{0}", procId);

            string g = "l";
                            


            //ExportShopify();
            //PlayRefund();
            //PlayMeta();

            //ExamineFiles();
            //object f = "";
            //decimal d= decimal.Parse(f.ToString());

            //string fff = "l";
            //TestAlpha();
            //try
            //{
            //    MailQueue.SendEmail("customerservice@sts9store.com", "the service",
            //        "rob@robkurtz.net", null, null,
            //        "Your Requested Information", null, "ConfigureBodyLinks_Html of EmailLetter here", null, false, null);
            //}
            //catch (Exception e)
            //{
            //    string g = e.Message;
            //}


            //int sum = 0;
            //using (System.Data.IDataReader reader = SPs.TxInventoryBundleGetChildItemCount(10011).GetReader())
            //{
            //    if (reader != null)
            //    {
            //        while (reader.Read())
            //        {
            //            sum = reader.GetInt32(reader.GetOrdinal("BundleSum"));
            //        }
            //    }
            //}


            //string i = "";


            //decimal d = 1.669M;
            //decimal priceForSelection = 0;// decimal.Round(d, 2, MidpointRounding.ToEven);

            //priceForSelection = (decimal)Utils.WctMath.RoundDown((double)d, 2);

            //string g = "";

            //string[] numberOfThings = { "one thing", "yet another", "something else", "something borrowed", "something blue"};
            //int counter = 0;

            //foreach (string s in numberOfThings)
            //{
            //    Console.WriteLine(string.Format("{0}) {1}", counter++.ToString(), s));
            //}







            /*
            string delimiter = "-";
            string txt = "2 @ aksdjh*(&^*&$%^#<div></div><a href=\"somelink\">>>fkjhsdf";

            string working = txt;// Utils.ParseHelper.StripHtmlTags(txt);
            //strip non alpha-numeric
            string trans = System.Text.RegularExpressions.Regex.Replace(working.Replace("'", ""), @"\W",
                delimiter.ToString(), System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            */



        }
//        public static void TryRefunds()
//        {
//            InvoiceCollection coll = new InvoiceCollection();

//            System.Text.StringBuilder sb = new System.Text.StringBuilder();

//            //select top 1 tinvoiceid from winter.dbo.wininv where lock = 0
//            sb.AppendFormat("update winter.dbo.wininv set lock = 0; ");
//            sb.AppendFormat("select top {0} tinvoiceid into #tmpids from winter.dbo.wininv where lock = 0 order by tinvoiceid; select tinvoiceid from #tmpids; ", "50000");
//            sb.AppendFormat("update winter.dbo.wininv set lock = 1 where tinvoiceid in (select tinvoiceid from #tmpids); ");
//            sb.AppendFormat("drop table #tmpids; ");

//            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
//            Debug.WriteLine(DateTime.Now.ToLongTimeString());
//            using (IDataReader dr = SubSonic.DataService.GetReader(cmd))
//            {
//                while (dr.Read())
//                {
//                    int idx = (int)dr.GetValue(dr.GetOrdinal("tinvoiceid"));

//                    int exists = coll.GetList().FindIndex(delegate(Invoice match) { return (match != null && match.Id == idx); });

//                    if (exists == -1)
//                    {
//                        Invoice inv = Invoice.FetchByID(idx);
//                        coll.Add(inv);
//                    }
//                    else
//                        Debug.WriteLine(string.Format("{0} Already exists!", idx.ToString()));
//                }
//            }

//            Debug.WriteLine(DateTime.Now.ToLongTimeString());

//            //ExecuteRefunds("Winter_2011", coll);
//        }

//        //public static void ExecuteRefunds(string refundDescription, InvoiceCollection coll)
//        //{
//        //    foreach (Invoice i in coll)
//        //    {
//        //        //reset grid
//        //        GridView1.DataSource = null;
//        //        GridView1.DataBind();

//        //        //load the user's profile 
//        //        ProfileCommon userProfile = Profile.GetProfile(i.AspnetUserRecord.UserName);

//        //        //do the refund                
//        //        string proc = "AuthNet";

//        //        //construct list of items to refund in the invoice
//        //        List<RefundListItem> list = new List<RefundListItem>();
//        //        InvoiceItemCollection items = new InvoiceItemCollection();
//        //        items.AddRange(i.InvoiceItemRecords());
                
//        //        foreach (InvoiceItem ii in items)
//        //        {
//        //            bool refund = false;

//        //            if(ii.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString() && ii.LineItemTotal > 0)
//        //            {
//        //                //we are refunding tickets
//        //                if (ii.IsTicketItem && ii.DateOfShow > DateTime.Parse("1/31/2011"))
//        //                    refund = true;
//        //                //merchdownloads within the period specified
//        //                else if (ii.IsMerchandiseItem && ii.MerchRecord.IsDownloadDelivery && ii.MerchRecord.Id >= 10926 && ii.MerchRecord.Id <= 10951)
//        //                    refund = true;
//        //                //shipping except for these items that have already shipped
//        //                else if (ii.IsShippingItem_Ticket && ii.DateShipped < DateTime.MaxValue)
//        //                    refund = true;

//        //                //no processing
//        //                //no other merch items
//        //                //no merchshipping
//        //                //no discounts are affected
//        //                //no note items
//        //            }

//        //            if(refund)
//        //            {
//        //                RefundListItem rli = new RefundListItem(ii);
//        //                list.Add(rli);
//        //            }

//        //        }



//        //        GridView1.DataSource = list;
//        //        GridView1.DataBind();



//        //        //setup logging
//        //        string result = string.Empty;
//        //        string logit = string.Format("Customer: {0} \r\n", userProfile.UserName);
//        //        logit += string.Format("Invoice: {0} - {1}\r\n", i.Id, i.UniqueId);
//        //        logit += string.Format("InvoiceDate: {0}\r\n", i.InvoiceDate.ToString("MM/dd/yyyy hh:mmtt"));


//        //        try
//        //        {
//        //            result = OrderRefund.DoRefund(userProfile, proc, _invoice, GridView1,
//        //                description, this.Profile.UserName, this.Request.UserHostAddress);
//        //            //result = string.Empty;
//        //        }
//        //        catch (Exception ex)
//        //        {
//        //            result = ex.Message;
//        //        }

//        //        if (result == "SUCCESS")
//        //        {
//        //            //log success into refund log
//        //            _Error.LogToFile(logit, refundDescription);
//        //        }
//        //        else
//        //        {
//        //            logit += string.Format("Reason: {0}\r\n", result);
//        //            //log error
//        //            _Error.LogToFile(logit, string.Format("{0}_FAILURES", refundDescription));
//        //        }

//        //        GridView1.DataSource = null;
//        //        GridView1.DataBind();

//        //    }
//        //}





















//        public static string InterleavedString2(string firstString, string secondString, int bufferLength = 16)
//        {   
//            string interleave = string.Empty;

//            int i = 0;
//            int j = 0;

//            while (i < bufferLength)
//            {

//                if (j < firstString.Length)
//                {
//                    interleave += firstString.Substring(j,1);
//                    i++;
//                }

//                if (j < secondString.Length)
//                {
//                    interleave += secondString.Substring(j,1);
//                    i++;
//                }

//                j++;
//            }

//            return interleave;
//        }
//        public static string InterleavedString(string firstString, string secondString, int bufferLength = 16)
//        {
//            System.Text.StringBuilder interleave = new System.Text.StringBuilder();

//            int i = 0;
//            int j = 0;
//            int firstLength = firstString.Length;
//            int secondLength = secondString.Length;

//            while (i < bufferLength)
//            {

//                if (j < firstLength)
//                {
//                    interleave.Append(firstString.Substring(j, 1));
//                    i++;
//                }

//                if (j < secondLength)
//                {
//                    interleave.Append(secondString.Substring(j, 1));
//                    i++;
//                }

//                j++;
//            }

//            return interleave.ToString();
//        }
//        private static void TestUSPS()
//        {
//            StringBuilder xml = new StringBuilder();

//            xml.Append("API=RateV4&XML=<RateV4Request USERID=\"131SACRE0362\"><Revision>2</Revision><Package ID=\"0\">");

//            ///FIRST CLASS, FIRST CLASS HFP COMMERCIAL, PRIORITY, PRIORITY COMMERCIAL, PRIORITY HFP COMMERCIAL, 
//            ///EXPRESS, EXPRESS COMMERCIAL, EXPRESS SH, EXPRESS SH COMMERCIAL, EXPRESS HFP, EXPRESS HFP COMMERCIAL, 
//            ///PARCEL, MEDIA, LIBRARY, ALL, ONLINE
//            ///

//            if (false)//usps_mediaratequalified)
//                xml.AppendFormat("<Service>{0}</Service>", "MEDIA");
//            else
//                xml.AppendFormat("<Service>{0}</Service>", "PARCEL");



//            xml.Append("<ZipOrigination>80302</ZipOrigination><ZipDestination>80304</ZipDestination><Pounds>0</Pounds><Ounces>48</Ounces>");
//            //xml.Append("<Container>FLAT RATE BOX</Container>");
//            xml.Append("<Container/>");
//            xml.Append("<Size>REGULAR</Size><Machinable>False</Machinable></Package></RateV4Request>");

//            //construct the request
//            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://stg-production.shippingapis.com/ShippingAPI.dll");
            
//            req.Method = "POST";
//            req.ContentType = "application/x-www-form-urlencoded";
//            req.ContentLength = xml.Length;

//            StreamWriter stOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
//            stOut.Write(xml.ToString());
//            stOut.Close();
//            StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
//            string m_usps_raw_xml_text = stIn.ReadToEnd();
//            stIn.Close();

//            //reset the xml
//            xml.Length = 0;

//            System.Xml.XmlDocument myxmldocument = new System.Xml.XmlDocument();
//            myxmldocument.LoadXml(m_usps_raw_xml_text);
//            string errordesc = "";

//            if (m_usps_raw_xml_text.Length > 7 && m_usps_raw_xml_text.Substring(0, 7) == "<Error>")
//                errordesc = m_usps_raw_xml_text;

//            string rateResponseVersion = "RateV4Response";
//            System.Xml.XmlNodeList ratedshipment_nodelist;
//            ratedshipment_nodelist = myxmldocument.SelectNodes(string.Format("/{0}/Package/Postage", rateResponseVersion));

//            foreach (System.Xml.XmlNode node in ratedshipment_nodelist)
//            {
//                //usps_rate_detail usps = new usps_rate_detail();

//                System.Xml.XmlNode nd = node.SelectSingleNode("MailService");
//                string desc = nd.InnerText;
//                desc = Utils.ParseHelper.StripHtmlTags(System.Web.HttpUtility.HtmlDecode(desc.ToLower())).Replace("&reg;", string.Empty);

//                decimal rate = Convert.ToDecimal(node.SelectSingleNode("Rate").InnerText);

//                string g = "";

//            }

//        }

//        ///// <summary>
//        ///// The main entry point for the application.
//        ///// </summary>
//        //[STAThread]
//        //static void Main(string[] args)
//        //{
//        //    decimal amt = decimal.Round(decimal.Parse("105.00"), 2);

//        //    string txt = "2 @ aksdjhfkjhsdf";
//        //    string tst = txt.Substring(0, txt.IndexOf("@"));
//        //    int qty = int.Parse(txt.Substring(0, txt.IndexOf("@")));

//        //    decimal perItemPrice = (amt > 0) ? (decimal)(amt / qty) : 0;

//        //    string working = Utils.ParseHelper.StripHtmlTags(this.DisplayName);
//        //            //strip non alpha-numeric

//        //            string delimiter = "-";

//        //            string trans = System.Text.RegularExpressions.Regex.Replace(working.Replace("'", ""), @"\W", 
//        //                delimiter.ToString(), System.Text.RegularExpressions.RegexOptions.IgnoreCase);

//            //TryRefunds();
//            //string invoiceid = "00109c4a038acf32";
//            //int idx = 1765223;

//            //TestUSPS();
//            //Trace.WriteLine(DateTime.Now.Ticks.ToString());
//            //string wha = TestApp.InterleavedString2(invoiceid, idx.ToString());
//            //Trace.WriteLine(DateTime.Now.Ticks.ToString());

//            //Trace.WriteLine(DateTime.Now.Ticks.ToString());
//            //string who = TestApp.InterleavedString(invoiceid, idx.ToString());
//            //Trace.WriteLine(DateTime.Now.Ticks.ToString());


//            //string idxer = idx.ToString();

//            //char[] inv = invoiceid.ToCharArray();
//            //char[] itm = idxer.ToCharArray();

//            //int bufferLength = 16;
//            //char[] newid = new char[bufferLength];

//            //int i = 0;
//            //int j = 0;

//            //// 0 0 1 0 9 c 4 a 0 3 8 a c f 3 2
//            ////  1 7 6 5 2 2 3
//            ////"0107160592c243a0"

//            //while (i < bufferLength)
//            //{

//            //    if (j < inv.Length)
//            //    {
//            //        newid[i] = inv[j];
//            //        i++;
//            //    }

//            //    if (j < itm.Length)
//            //    {
//            //        newid[i] = itm[j];
//            //        i++;
//            //    }

//            //    j++;
//            //}

                
//            ////evaluate i
//            ////tack on remaining

//            ////get substring to match length of required


//            ////"0107160592c243a0"


//            //string code = string.Format("{0}{1}", invoiceid, idx.ToString());
            
            
            
//            //int len = code.Length;

//            //if (len > 16)
//            //{
//            //    code = code.Substring(len - 16, 16);
//            //}

//            //int s = 1;




//            //string bubba = @"four score and <strong>super</strong> duper<br />  ";
//            //bubba += "<a href=\"http://google.com?inv=e5690d7d632e23d9&amp;usr=rob@robkurtz.net&amp;itm=12694\"><img src=\"cvsome source file\" />google.com<img src=\"cvsome source file\"></img></a><br />  hey hey";

//            //string bubbatrans = Utils.ParseHelper.LinksToHref(bubba);

//            //string transbubba = Utils.ParseHelper.LinksToHref(bubba, false);

//           // string easy = Utils.ParseHelper.StripHtmlTags(bubba);

//            //string g = "l";
//            //System.Data.DataSet ds = new System.Data.DataSet();
//            //ds.Tables[
//            //ShowDateCollection sdColl = new Select((ShowDate.Schema.TableName + ".*", Show.Schema.TableName + ".*")
//            //    .From(ShowDate.Schema)
//            //    .InnerJoin<ShowStatus>()               

//            //    .InnerJoin<Show>()
//            //    .InnerJoin<Venue>()

//            //    .LeftOuterJoin<ShowTicket>()
//            //    .InnerJoin<Age>()
//            //    .InnerJoin<Vendor>()

//            //    .Where(Show.Columns.Name).IsGreaterThan(_Config.SHOWOFFSETDATE.ToString("yyyy/MM/dd hh:mmtt"))
//            //    .OrderAsc(Show.Columns.Name)//.From<ShowDate>()
//            //    .ExecuteAsCollection<ShowDateCollection>();

//            //int cnt = sdColl.Count;


//            //SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);
//            //SubSonic.DataService.e
            
                
                
//                //DataSet ds = new Select(ShowDate.Schema.TableName + ".*", Show.Schema.TableName + ".*") 
//            //    .From(ShowDate.Schema)
//            //    .InnerJoin<ShowStatus>()
//            //    .InnerJoin<Age>()
//            //    .InnerJoin<Vendor>()

//            //    .InnerJoin<Show>()
//            //    .InnerJoin<Venue>()
                
//            //    .LeftOuterJoin<ShowTicket>()

                
//            //    .Where(Show.Columns.Name).IsGreaterThan(_Config.SHOWOFFSETDATE.ToString("yyyy/MM/dd hh:mmtt")) 
//            //    .OrderAsc(Show.Columns.Name) 
//            //    .ExecuteAsCollection<ShowDate>();

//            //ShowDateCollection sd = new ShowDateCollection();
//            //sd.Load();
//            //sd.

//            //using(IDataReader rdr = SPs.TxGetSaleShowDates(_Config.APPLICATION_ID, _Config.SHOWOFFSETDATE.ToString("yyyy/MM/dd hh:mmtt")).GetReader())
//            //{
//            //    ShowDateCollection showDates = new ShowDateCollection();
//            //    showDates.LoadAndCloseReader(rdr);

//            //    ShowTicketCollection coll = new ShowTicketCollection();
//            //    coll.AddRange(showDates[0].ShowTicketRecords());
//            //}

//            //SubSonic.QueryCommand

//            //using (IDataReader = 


//            //ConcurrencyTesting.StartTest();

//            ////string input = "whatever    dudes' \"i think\" this and &therefore   ,       & somehow <span>jujoobee</span>";
//            //string input = "http://www.frog.com/";
//            //string result = regexTag.Replace(input, string.Empty);

//            ////substitute for common elements in text
//            //result = System.Web.HttpUtility.HtmlEncode(result).Replace("'", "&#39;");//result.Replace(" & ", " &amp; ").Replace("'", "&#39;").Replace("\"", "&quot;");

//            //result = Regex.Replace(result, @"\s+", " ").Replace(" , ", ", ");

//            //result = result.Trim().Replace("  ", " ").Replace(" , ", ", ");

//            //try
//            //{
//            //    string mappedPath = string.Format(@"d:\source\willcall\{0}\Xml\Gift_Redeem.xml", "ResourcesSts9");
//            //    string file = null;
//            //    //if (!System.IO.File.Exists(mappedPath))
//            //    //    return string.Empty;

//            //    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
//            //    doc.Load(mappedPath);

//            //    System.Xml.XmlElement xml = doc.DocumentElement;

//            //    if (xml != null)
//            //    {
//            //        file = xml.InnerXml.Replace("{0}", _Config._DomainName).Trim();
//            //    }

//            //    file = Regex.Replace(file, @"<(\/)?ul>", Environment.NewLine, RegexOptions.IgnoreCase);
//            //    file = Regex.Replace(file, "<li>", Environment.NewLine, RegexOptions.IgnoreCase);
//            //    file = Regex.Replace(file, regexTag.ToString(), string.Empty);

//                //string g = Utils.ParseHelper.XmlToText(file);

//               // StoreCredit.RedeemGiftCertificate(null, null);

//           // TestUrls();
//                //TestInvoking();
//                //TestLinkSelection();
//                //SalePromotionCollection coll = new SalePromotionCollection();
//                //coll.LoadAndCloseReader(SalePromotion.FetchAll());
//                //SalePromotion prom = coll[0];
//                //EventQ.CreateUserPointEvent_PostSale("matchingCode", "rob@robkurtz.net", prom, "uniqueInvoiceId");

//                //MailerContentCollection coll = new MailerContentCollection();
//                //coll.LoadAndCloseReader(MailerContent.FetchAll());
//                //MailerContent cont = coll[0];
//                //cont.ResetTitle(true);

//                //MailQueue.SendEmail("rob@robkurtz.net", "from me", "rkurtz@willcallTickets.com", null, null, "subject", "the body", "the text", null,
//                //    true, "my new name");
//            //}
//            //catch (Exception ex)
//            //{
//            //    Wcss._Error.LogException(ex);

//            //    Debug.WriteLine(ex.Message);
//            //    Debug.WriteLine(ex.StackTrace);
//            //}
//        //}

//        private static void TestInvoking()
//        {
//            //get a jshow promoter coll from a show
//            Show s = Show.FetchByID(15388);
//            JShowPromoterCollection coll = s.JShowPromoterRecords();
//            //if (_Collection.DeleteFromOrderedCollection(coll.GetList(), 12592))
//            //    coll.SaveAll();
//           //Utils._Collection.
//        }

//        private static void TestLinkSelection()
//        {
//            //string _line = "kjhkf subemlid=3d'87763''";
//            //                int idx = _line.IndexOf("subemlid=");
//            //                char[] badChars = { '=', '\'' };
//            //                string part1 = _line.Substring(idx + 12, 7).Replace("\"", string.Empty).TrimStart(badChars).TrimEnd(badChars);
//            ////                subId = part1;//

//            //                //TestApp.EscCommonSeqInHtml_AvoidInnerHtml("df");
//            System.Text.StringBuilder sb = new System.Text.StringBuilder();

//            sb.AppendLine("<IMG SRC=\"http://www.foxtheatre.com/WillCallResources/mailTemplates/Images/headwidge_017cfe_mapt.gif\" WIDTH=478 HEIGHT=128 BORDER=0 ALT=\"\" USEMAP=\"#headwidge_017cfe_mapt\">");
//            sb.AppendLine("<MAP NAME=\"headwidge_017cfe_mapt\">");
//            sb.AppendLine("<AREA SHAPE=\"rect\" ALT=\"Download the FREE Fox Theatre iPhone app at the iPhone App Store\" COORDS=\"324,8,448,117\" HREF=\"http://itunes.apple.com/WebObjects/MZStore.woa/wa/viewSoftware?id=318777944&mt=8\" TARGET=\"_blank\">");
//            sb.AppendLine("<AREA SHAPE=\"rect\" ALT=\"see us on facebook\" COORDS=\"186,8,282,117\" HREF=\"http://www.facebook.com/foxtheatreboulder\" TARGET=\"_blank\">");
//            sb.AppendLine("<AREA SHAPE=\"rect\" ALT=\"follow us on twitter\" COORDS=\"39,8,132,117\" HREF=\"http://twitter.com/foxtheatreco\" TARGET=\"_blank\">");
//            sb.AppendLine("<AREA SHAPE=\"rect\" ALT=\"Download the FREE Fox Theatre iPhone app at the iPhone App Store\" COORDS=\"333,9,406,58\" HREF=\"http://itunes.apple.com/WebObjects/MZStore.woa/wa/viewSoftware?id=318777944&mt=8\" TARGET=\"_blank\">");
//            sb.AppendLine("<AREA SHAPE=\"rect\" ALT=\"see us on facebook\" COORDS=\"201,9,271,62\" HREF=\"http://www.facebook.com/foxtheatreboulder\" TARGET=\"_blank\">");
//            sb.AppendLine("<AREA SHAPE=\"rect\" ALT=\"follow us on twitter\" COORDS=\"82,9,140,58\" HREF=\"http://twitter.com/foxtheatreco\" TARGET=\"_blank\">");
//            sb.AppendLine("<AREA SHAPE=\"rect\" ALT=\"\" COORDS=\"80,14,143,61\" HREF=\"#\">");
//            sb.AppendLine("</MAP><!--end header-->");

//            sb.AppendLine("");
//            sb.AppendLine("");

//            sb.AppendLine("<a href=\"mailto:freetix@foxtheatre.com   \"   >Free Tickets</a><td class=\"imagecell\">");
//            sb.AppendLine("<a href=\"http://www.foxtheatre.com/Store/ChooseTicket.aspx?sid=15417\">");
//            sb.AppendLine("		<img alt=\"\" width=\"120\" border=\"0\" src=\"http://www.foxtheatre.com/\" />");
//            sb.AppendLine("				</a>");
//            sb.AppendLine("");
//            sb.AppendLine("");
//            sb.AppendLine("		<a href=\"http://www.foxtheatre.com/Store/ChooseTicket.aspx?sid=15417\" ");
//            sb.AppendLine("		 title=\"new line?\">");
//            sb.AppendLine("			<img alt=\"\" width=\"120\" border=\"0\" src=\"http://www.foxtheatre.com/\" />");
//            sb.AppendLine("				</a>");

//            sb.AppendLine("");
//            sb.AppendLine("					Please send all've answers \"rah rah\" to <a style=\"display: inline;\" href=\"mailto:freetix@foxtheatre.com?subject=free tickets\">Free Tickets</a> - DO NOT Reply to this Email!");
        

//            //string text = "<a href=\"mailto:freetix@foxtheatre.com   \"   >Free Tickets</a><td class=\"imagecell\">";
//            //text += "					<a href=\"http://www.foxtheatre.com/Store/ChooseTicket.aspx?sid=15417\">";
//            //text += "						<img alt=\"\" width=\"120\" border=\"0\" src=\"http://www.foxtheatre.com/\" />";
//            //text += "					</a>";
//            //text += "\r\n";
//            //text += "					<a href=\"http://www.foxtheatre.com/Store/ChooseTicket.aspx?sid=15417\" \r\n title=\"new line?\">";
//            //text += "						<img alt=\"\" width=\"120\" border=\"0\" src=\"http://www.foxtheatre.com/\" />";
//            //text += "					</a>";

           

//            ////text += "				</td>";
//            ////text += "			</tr>";
//            ////text += "		</table>";
//            ////text += "<table cellpadding=\"0\" cellspacing=\"0\" class=\"specialinterest\">";
//            ////text += "			<tr><td><h4>Trivia</h4></td></tr>";
//            ////text += "			<tr>";
//            ////text += "				<td class=\"contentcell\">";
//            ////text += "					In the summer of what year did John Dawson and Jerry Garcia start playing in coffeehouses and small clubs?&nbsp; The music they made would become the nucleus for a new band - the New Riders of the Purple Sage. Ten randomly selected winners will receive a pair of tickets to see New Riders of The Purple Sage on Tuesday, April 21st!&nbsp; This show is 21+.";
//            ////text += "					<div>";
//            //text += "						Please send all've answers \"rah rah\" to <a style=\"display: inline;\" href=\"mailto:freetix@foxtheatre.com?subject=free tickets\">Free Tickets</a> - DO NOT Reply to this Email!";
//            ////text += "					</div>";
//            ////text += "				</td>";

//            //string result = Utils.ParseHelper.ParseJSON(sb.ToString());//ConfigureBodyLinks_Html(text);
//            string result1 = ConfigureBodyLinks_Html(sb.ToString());


//            //int i = 0;
//        }
        
//        private static string EvaluateLink(Match m)
//        {
//            string EmailLetterName = "myemailletter";

//            string start = m.Groups["linkStart"].Value;
//            string link = m.Groups["linkProper"].Value.TrimEnd('.');
//            string end = (m.Groups["linkEnd"] != null) ? m.Groups["linkEnd"].Value : string.Empty;

//            Regex storeSite = new Regex(string.Format(@"{0}", Wcss._Config._DomainName.Replace("www.", string.Empty)));
//            //leave for testing
//            //Regex storeSite = new Regex(string.Format(@"{0}", "foxtheatre.com"));

//            Match isStoreLink = storeSite.Match(link);

//            //only append a querystring to those links on our site
//            string qsIdName = "seid";
//            string googleCampaign = string.Format("utm_source=wctmlr&utm_medium=email&utm_campaign={0}",
//                System.Web.HttpUtility.HtmlEncode(EmailLetterName));
//            string newLink = string.Empty;

//            if (link.ToLower().IndexOf("mailto:") != -1)
//            {
//                //dont include the other "qs" var for a subject - just add the name
//                newLink = string.Format("{0}{1}{2}{3}{4}{5}", start, link,
//                    (link.ToLower().IndexOf("?") == -1) ? "?" : string.Empty,
//                    (link.ToLower().IndexOf("subject=") == -1) ? "subject=" : " - ",
//                    EmailLetterName, end);
//            }
//            else
//                newLink =
//                    (!isStoreLink.Success) ?
//                    string.Format("{0}{1}{2}{3}{4}", start, link, (link.IndexOf("?") != -1) ? "&" : "?", googleCampaign, end) :
//                    string.Format("{0}http://{1}/Sd.aspx?{2}={3}&url={4}&{5}{6}", start, Wcss._Config._DomainName, qsIdName, "123456",
//                        System.Web.HttpUtility.HtmlEncode(link), googleCampaign, end);

//            return newLink;
//            /*
//            //we have start and end because the quoting could be different
//            string start = m.Groups["linkStart"].Value;
//            string link = m.Groups["linkProper"].Value.Trim().TrimEnd('.');
//            string end = (m.Groups["linkEnd"] != null) ? m.Groups["linkEnd"].Value : string.Empty;

//            Regex storeSite = new Regex(string.Format(@"{0}", Wcss._Config._DomainName.Replace("www.", string.Empty)));

//            Match isStoreLink = storeSite.Match(link);

//            //only append a querystring to those links on our site
//            string qsIdName = "seid";

//            string newLink = string.Empty;
            
//            if(link.ToLower().IndexOf("mailto:") != -1)
//            {
//                newLink = string.Format("{0}{1}{2}{3}{4}{5}", start, link, 
//                    (link.ToLower().IndexOf("?") == -1) ? "?" : string.Empty, 
//                    (link.ToLower().IndexOf("subject=") == -1) ? "subject=" : " - ", 
//                    "the Name", end);
//            }
//            else            
//                newLink = (isStoreLink.Success) ? string.Format("{0}{1}{2}{3}={4}{5}", start, link, (link.IndexOf("?") != -1) ? "&" : "?", qsIdName, "9786", end) :
//                    string.Format("{0}http://{1}/Sd.aspx?{2}={3}&url={4}{5}", start, Wcss._Config._DomainName, qsIdName, "9786", System.Web.HttpUtility.HtmlEncode(link), end);

//            return newLink;*/
//        }

//        private static string ConfigureBodyLinks_Html(string txt)
//        {
//            //Debug.WriteLine(url);
//            //Regex getLinks = new Regex(@"(?<linkStart><A[^>]*?HREF\s*=\s*[""']?)(?<linkProper>[^'"">]+?)(?<linkEnd>[ '""]?\s*>)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
//            //Regex getLinks = new Regex(@"(?<linkStart>HREF\s*=\s*[""']?)(?<linkProper>[^'"">]+?)(?<linkEnd>[ '""]?>)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
//            //Regex getLinks = new Regex(@"(?<linkStart><A[^>]*??HREF\s*=\s*[""']?[mailto]?)(?<linkProper>[^'"" >]+?)(?<linkEnd>[ '""]?>)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
//            //     <a style=\"display: inline;\" href=\"mailto:freetix@foxtheatre.com?subject=free tickets\">Free Tickets</a>

//            Regex getLinks = new Regex(@"(?<linkStart><A[^>^(REA)]*?HREF\s*=\s*[""']?)(?<linkProper>[^'"">]+?)(?<linkEnd>[ '""]?\s*>)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

//            MatchCollection coll = getLinks.Matches(txt);

//            Regex getLinks2 = new Regex(@"(?<linkStart> HREF\s*=\s*[""']?)(?<linkProper>[^#'"">]+?)(?<linkEnd>[ '""]+?)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

//            MatchCollection coll2 = getLinks2.Matches(txt);

//            //Regex getLinks2 = new Regex(@"(?<linkStart><A[^>^(REA)]*?HREF\s*=\s*[""']?)(?<linkProper>[^'"">]+?)(?<linkEnd>[ '""]?\s*>)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

//            //MatchCollection coll2 = getLinks2.Matches(txt);


//            string result = getLinks2.Replace(txt, new MatchEvaluator(EvaluateLink));

//            //Debug.WriteLine(result);

//            return result;

//        }


//        public static Regex regexTag = new Regex("(?<tagBody><(\"[^\"]*\"|\'[^\']*\'|[^\'\">])*>)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
//        //public static string regexTag = "<(\"[^\"]*\"|\'[^\']*\'|[^\'\">])*>*";//, RegexOptions.IgnoreCase | RegexOptions.Multiline);
//        //public static Regex regexTag = new Regex("<([A-Z][A-Z0-9]*)\\b[^>]*>(.*?)</\\1>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
//        public static void EscCommonSeqInHtml_AvoidInnerHtml(string input)
//        {
//            //take out the tags and mark the position

//            Debug.WriteLine("");
//            Debug.WriteLine("");
//            Debug.WriteLine(regexTag.ToString());


//            Debug.WriteLine("");
//            Debug.WriteLine("----GROUPS-------------");

//            Debug.WriteLine(input);

//            string result = regexTag.Replace(input, new MatchEvaluator(TestApp.EvaluateLinkage));
//            Debug.WriteLine(result);

//            result = result.Replace(" & ", " &amp; ").Replace("'", "&#39;").Replace("\"", "&quot;");
//            Debug.WriteLine(result);

//            //now substitute from the list back into the string
//            if (tags.Count > 0)
//            {
//                int count = tags.Count;

//                for (int i = 1; i <= count; i++)
//                {
//                    string tagHolder = string.Format("<${0}>", i);
//                    string tag = tags[i - 1];
//                    result = result.Replace(tagHolder, tag);
//                }
//            }

//            Debug.WriteLine("\r\nfinal result: \r\n");
//            Debug.WriteLine(result);

//            Debug.WriteLine("");
//            Debug.WriteLine("");

//        }
//        protected static List<string> tags = new List<string>();
//        public static string EvaluateLinkage(Match m)
//        {
//            //we have start and end because the quoting could be different
//            string start = m.Groups["tagBody"].Value;
//            tags.Add(start);

//            //only append a querystring to those links on our site
//            return start.Replace(start, string.Format("<${0}>", tags.Count));
//        }
//        public static void TestServiceComputation()
//        {
//            Debug.WriteLine(ComputeServiceFee(1).ToString());
//            Debug.WriteLine(ComputeServiceFee(5).ToString());
//            Debug.WriteLine(ComputeServiceFee(10).ToString());
//            Debug.WriteLine(ComputeServiceFee(24).ToString());
//            Debug.WriteLine(ComputeServiceFee(23.95m).ToString());
//            Debug.WriteLine(ComputeServiceFee(11.11m).ToString());
//        }
//        public static decimal ComputeServiceFee(decimal unitPrice)
//        {
//            decimal pct = .235M;
//            decimal roundup = .05M;

//            Debug.Write(string.Format("{0} * {1} ==> {2} ==> ", unitPrice, pct, unitPrice * pct));
//            decimal baseFee = decimal.Round(unitPrice * pct, 2);
//            Debug.Write(string.Format("baseFee: {0} ==>", baseFee));

//            decimal result = Math.Round(Math.Round((baseFee / roundup) + roundup, 0) * roundup, 2);
//            Debug.WriteLine(result);

//            return result;
//        }
//        //public static string EvaluateLink(Match m)
//        //{
//        //    //we have start and end because the quoting could be different
//        //    string start = m.Groups["linkStart"].Value;
//        //    string link = m.Groups["linkProper"].Value;
//        //    string end = m.Groups["linkEnd"].Value;

//        //    Regex storeSite = new Regex(string.Format(@"{0}", Wcss._Config._DomainName.Replace("www.", string.Empty)));

//        //    Match isStoreLink = storeSite.Match(link);

//        //    //only append a querystring to those links on our site
//        //    string newLink = (isStoreLink.Success) ? string.Format("{0}{1}{2}{3}={4}{5}", start, link, (link.IndexOf("?") != -1) ? "&" : "?", "myQsVar", _seid, end) :
//        //        string.Format("{0}http://{1}/Sd.aspx?seid={2}&url={3}{4}", start, Wcss._Config._DomainName, _seid, System.Web.HttpUtility.HtmlEncode(link), end);

//        //    return newLink;
//        //}
//        //public static string _seid = string.Empty;
//        //public static void TestRegexQueryString()
//        //{
//        //    int seid = 2765;

//        //    _seid = seid.ToString();

//        //    string url = @"<a href=""willcalltickets.com"">the store</a> kjsdhfkjhfdksdf\r\n kjshdkfjh s<a href= ""http://www.foxtheatre.com/index.aspx?sid=1098""></a> yadda yadda <a href=""sts9store.com"">the store</a>";

//        //    Debug.WriteLine(url);

//        //    Regex getLinks = new Regex(@"(?<linkStart><A[^>]*?HREF\s*=\s*[""']?)(?<linkProper>[^'"" >]+?)(?<linkEnd>[ '""]?>)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

//        //    string result = getLinks.Replace(url, new MatchEvaluator(TestApp.EvaluateLink));

//        //    Debug.WriteLine(result);

//        //}
//        public static void TestUrls()
//        {
//            bool isValid = IsValidUrl("www.goodone");
//            isValid = IsValidUrl("www.goodone.com"); 
//            isValid = IsValidUrl("http://www.goodone");
//            isValid = IsValidUrl("http://www.goodone/");
//            isValid = IsValidUrl("http://www.goodone.com");
//            isValid = IsValidUrl("http://www.goodone.com/");
//            isValid = IsValidUrl("http://www.goodone.com/kjahskjhd.asp");
//            isValid = IsValidUrl("http://www.goodone.com/kjahskjhd.asp?as=0");
//            isValid = IsValidUrl("localhost");
//            isValid = IsValidUrl("http://localhost");

//        }
//        //public static Regex regexUrl = new Regex(@"(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=()\'\\]*)?", RegexOptions.IgnoreCase);
//        public static Regex regexUrl = new Regex(@"(http(s)?://)?((([\w-]+\.)+[\w-]+)|localhost)(/[\w- ./?%&=()\'\\]*)?", RegexOptions.IgnoreCase);
//        public static bool IsValidUrl(string url)
//        {   
//            if (url.IndexOf("%") != -1)
//                url = System.Web.HttpUtility.UrlDecode(url);

//            if (url == null || url.Trim().Length == 0) return false;

//            Match mUrl = regexUrl.Match(url);

//            if (mUrl.Length < url.Length) return false;

//            return true;
//        }
//        public static int GetRandomInRange(int rangeStart, int rangeEnd)
//        {
//            long tix = System.DateTime.Now.Ticks;
//            int ticks = (int)tix;
//            //Convert.ToInt32(tix);

//            //bool yes = int.TryParse(tix.ToString(), out ticks);

//            System.Random RandNum = new System.Random(ticks);

//            return RandNum.Next(rangeStart, rangeEnd);
//        }
//        public static string RetrieveUniqueId()
//        {
//            return string.Format("{0:x}", long.Parse(DateTime.Now.Ticks.ToString()));
//        }
//    }
//}
