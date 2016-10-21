using System;
using System.Web;

namespace Utils
{
	/// <summary>
	/// Summary description for cookies.
	/// </summary>
	public class SessionCookieManager
	{
		protected System.Web.HttpResponse response;

        public SessionCookieManager() {}

		public SessionCookieManager(System.Web.HttpResponse oResponse)
		{
			response = oResponse;
		}

		public static void ClearCookies()
		{
			HttpContext.Current.Request.Cookies.Clear();
			HttpContext.Current.Response.Cookies.Clear();			
		}

		protected string getCookie(string sCookieSet, string sCookieName)
		{
			HttpCookie c = HttpContext.Current.Request.Cookies[sCookieSet];
			if (c == null)
				return "";
			else
				return c.Values[sCookieName];
		}
        public string getCookie(string sCookieName)
        {
            HttpCookie c = HttpContext.Current.Request.Cookies[sCookieName];
            if (c == null)
                return string.Empty;
            
            return c.Value;
        }

        protected void setCookieInCookieSet(string sCookieSet, string sCookieName, string sCookieValue)
        {
            HttpCookie c = HttpContext.Current.Response.Cookies[sCookieSet];
            if (c == null)
            {
                c = new HttpCookie(sCookieSet);
                HttpContext.Current.Response.Cookies.Add(c);
            }

            c.Values.Remove(sCookieName);
            c.Values.Add(sCookieName, sCookieValue);


            HttpCookie d = HttpContext.Current.Request.Cookies[sCookieSet];
            if (d == null)
            {
                d = new HttpCookie(sCookieSet);
                HttpContext.Current.Request.Cookies.Add(c);
            }

            d.Values.Remove(sCookieName);
            d.Values.Add(sCookieName, sCookieValue);
        }
        protected void setSessionCookie(string sCookieName, string sCookieValue)
        {
            setSessionCookieInPath(null, sCookieName, sCookieValue);
        }
        protected void setSessionCookieInPath(string path, string sCookieName, string sCookieValue)
        {
            if (path == null || path.Trim().Length == 0)
                path = "/";

            if (!path.StartsWith("/"))
                path = string.Format("/{0}", path);

            HttpCookie c = HttpContext.Current.Response.Cookies[sCookieName];
            if (c == null)
            {
                c = new HttpCookie(sCookieName);
                c.Path = path;
                HttpContext.Current.Response.Cookies.Add(c);
            }

            if (c.Path != path)
                c.Path = path;

            c.Value = sCookieValue;

            HttpCookie d = HttpContext.Current.Request.Cookies[sCookieName];
            if (d == null)
            {
                d = new HttpCookie(sCookieName);
                d.Path = path;
                HttpContext.Current.Request.Cookies.Add(d);
            }

            if (d.Path != path)
                HttpContext.Current.Request.Cookies[sCookieName].Path = path;

            d.Value = sCookieValue;
        }
        protected void setPersistentCookie(string sCookieName, string sCookieValue)
        {
            HttpCookie c = HttpContext.Current.Response.Cookies[sCookieName];
            if (c == null)
            {
                c = new HttpCookie(sCookieName);
                c.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(c);
            }

            c.Value = sCookieValue;
            if(c.Expires == DateTime.MinValue)
                c.Expires = DateTime.Now.AddYears(1);

            HttpCookie d = HttpContext.Current.Request.Cookies[sCookieName];
            if (d == null)
            {
                d = new HttpCookie(sCookieName);
                d.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Request.Cookies.Add(c);
            }

            d.Value = sCookieValue;
            if (d.Expires == DateTime.MinValue)
                d.Expires = DateTime.Now.AddYears(1);
        }
        protected void setExpiryCookie(string sCookieName, string sCookieValue, int timeInMinutes)
        {
            HttpCookie c = HttpContext.Current.Response.Cookies[sCookieName];
            if (c == null)
            {
                c = new HttpCookie(sCookieName);
                c.Expires = DateTime.Now.AddMinutes(timeInMinutes);
                HttpContext.Current.Response.Cookies.Add(c);
            }

            c.Value = sCookieValue;
            if (c.Expires == DateTime.MinValue)
                c.Expires = DateTime.Now.AddMinutes(timeInMinutes);

            HttpCookie d = HttpContext.Current.Request.Cookies[sCookieName];
            if (d == null)
            {
                d = new HttpCookie(sCookieName);
                d.Expires = DateTime.Now.AddMinutes(timeInMinutes);
                HttpContext.Current.Request.Cookies.Add(c);
            }

            d.Value = sCookieValue;
            if (d.Expires == DateTime.MinValue)
                d.Expires = DateTime.Now.AddMinutes(timeInMinutes);
        }
	}
}


