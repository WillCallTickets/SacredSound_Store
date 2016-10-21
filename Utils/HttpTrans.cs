using System;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

namespace Utils
{
	/// <summary>
	/// This class provides functionality to get/post a page and return results as 
	/// a string.  It also provides a parser which can take a list of regex fields
	/// to match, and returns a structured, field delimited file suitable for 
	/// import to Excel or a DB.
	/// </summary>	
	[Serializable()]
	public class HttpTrans
	{

		private int Timeout;
		private string User = "";
		private string Password = "";
		private string Site;

		private static Regex regexURl	= new Regex(@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?", RegexOptions.IgnoreCase);

		public static bool VerifyURl(string url)
		{
			if(url.Trim().Length == 0) return false;

			Match mURL		= regexURl.Match(url);

			if (mURL.Length < url.Length) return false;

			return true;
		}


		public HttpTrans()
		{
			Timeout = 90000;
		}

		public void SetTimeout( int timeout)
		{
			Timeout = timeout;
		}

		public void SetAuthentication( string user, string pass, string site)
		{
			User = user;
			Password = pass;
			Site = site;
		}

		public String GeneratePostString( String rawPage, String formIdentifier)
		{	
			Regex r = new Regex(@"<FORM.*METHOD=POST>(?<name>.*)</FORM>", RegexOptions.Singleline);
			Match m = r.Match( rawPage, 0);

			
			String formdata = m.Groups[1].ToString();
			
			Regex regName = new Regex(@"<INPUT.*NAME=""?(?<name>[^""]*)""? VALUE", RegexOptions.IgnoreCase );
			Regex regValue= new Regex(@"<INPUT.*VALUE=""(?<value>[^""]*)"".*>", RegexOptions.IgnoreCase);

			Match matchName =  regName.Match( formdata);
			Match matchValue = regValue.Match( formdata);

			String results = "";

			while(( matchName.Length > 0) && (matchValue.Length > 0))
			{
				
				results += HttpUtility.UrlEncode( matchName.Groups[1].ToString());
				results += "=";
				results += HttpUtility.UrlEncode( matchValue.Groups[1].ToString());
				matchName = matchName.NextMatch();
				matchValue = matchValue.NextMatch();
				if (matchName.Length > 0) results += "&";
			}
			
			return results;

		}

		public WebResponse PostGetResponse( string url, string postData, string contentType)
		{
			try
			{
				
				if ((url == null) || (url.Length == 0) || (postData == null) || (postData.Length == 0) ) 
					return null;

				Debug.WriteLine( "Sending " + postData + " to url " + url);

				WebRequest Request = WebRequest.Create(url);
				Request.Timeout = Timeout;
				
				Request.Credentials = CredentialCache.DefaultCredentials;
				
				if (User != "")
				{
					NetworkCredential cred = new NetworkCredential(User, Password, ""); 
					CredentialCache credCache = new CredentialCache();
 
					credCache.Add(new Uri(Site), "Basic", cred);
 
					Request.Credentials = credCache;
				}

				Request.Method = "POST";
				//Request.ContentLength = postData.Length;
				Request.ContentType = contentType;
				Stream RequestStream = Request.GetRequestStream();
				StreamWriter sw = new StreamWriter( RequestStream );
				sw.Write( postData);
				sw.Close();
								
				WebResponse Response = Request.GetResponse();
				return Response;
			}
			catch(Exception e)
			{
				Debug.WriteLine( e.StackTrace );
			}
			return null;

		}
		public WebResponse PostGetResponse( string url, string postData)
		{
			return PostGetResponse( url, postData, "application/x-www-form-urlencoded");
		}

		public String Post( String url, String postData)
		{
			String Result = "";
			try
			{
				if ((url == null) || (url.Length == 0) || (postData == null) || (postData.Length == 0) ) 
					return "";

				WebRequest Request = WebRequest.Create(url);
				Request.Timeout = Timeout;//currently 90000 ms
				Request.Credentials = CredentialCache.DefaultCredentials;
				Request.Method = "POST";
				Request.ContentLength = postData.Length;
				Request.ContentType = "application/x-www-form-urlencoded";
				
                Stream RequestStream = Request.GetRequestStream();
				StreamWriter sw = new StreamWriter( RequestStream );
				sw.Write( postData);
				sw.Close();				
				
				WebResponse Response = Request.GetResponse();
				Stream RespStream = Response.GetResponseStream();
				StreamReader sr = new StreamReader( RespStream );
				Result = sr.ReadToEnd();
			}
			catch(Exception e)
			{
				Debug.WriteLine( e.StackTrace );
			}
			return Result;

		}

		public String Get( String siteUrl, String url )
		{
			String Result = "";
			if ((url == null) || (url.Length == 0))
				return "";
		
			if (!url.StartsWith( siteUrl))
			{
				url = siteUrl + url;
			}

			//Debug.WriteLine("Retrieving " + url);
			WebRequest Request = WebRequest.Create(url);
			Request.Timeout = Timeout;
			Request.Credentials = CredentialCache.DefaultCredentials;
		
			WebResponse Response = Request.GetResponse();
			Stream RespStream = Response.GetResponseStream();
			
			StreamReader sr = new StreamReader( RespStream );
			Result = sr.ReadToEnd();

			//Debug.WriteLine("Bytes retrieved " + Result.Length.ToString() );

			return Result;
		}
	}

}
