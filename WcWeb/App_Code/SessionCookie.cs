using System;

namespace WillCallWeb
{
	/// <summary>
	/// Summary description for SessionContext.
	/// </summary>
	public class SessionCookie : Utils.SessionCookieManager
	{
		public SessionCookie() : base () { }

        public int _ordinalTabCookie
        {
            get
            {
                string id = base.getCookie("ordinaltab");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("ordinaltab", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("ordinaltab", value.ToString());
            }
        }
        #region Atx Searching

        public bool _isAuthdCKFinder
        {
            get
            {
                string val = base.getCookie("ifk");
                if (val == null || val.Trim().Length == 0 || (!Utils.Validation.IsValBoolean(val)))
                {
                    string defaultValue = "0";
                    base.setPersistentCookie("ifk", defaultValue);
                    return (defaultValue == "1");
                }

                return (val == "1");
            }
            set
            {
                base.setPersistentCookie("ifk", (value) ? "1" : "0");
            }
        }

        public DateTime _upcomingShowStartDate
        {
            get
            {
                string val = base.getCookie("udt");
                if (val == null || val.Trim().Length == 0)
                {
                    DateTime defaultValue = DateTime.Parse(DateTime.Now.ToString("MM/1/yyyy")).Date;
                    base.setSessionCookie("udt", defaultValue.ToString());
                    return defaultValue;
                }

                return DateTime.Parse(val);
            }
            set
            {
                base.setSessionCookie("udt", value.Date.ToString());
            }
        }
        public DateTime _userAgeComplianceDate
        {
            get
            {
                string val = base.getCookie("uacd");
                if (val == null || val.Trim().Length == 0)
                {
                    DateTime defaultValue = DateTime.MaxValue.Date;
                    //base.setExpiryCookie("uacd", defaultValue.ToString("M/d/yyyy"), 60*48);//48 hours
                    base.setSessionCookie("uacd", defaultValue.ToString("M/d/yyyy"));
                    return defaultValue;
                }

                return DateTime.Parse(val);
            }
            set
            {
                base.setSessionCookie("uacd", value.Date.ToString("M/d/yyyy"));
            }
        }
        public DateTime _calendarDate
        {
            get
            {
                string val = base.getCookie("cld");
                if (val == null || val.Trim().Length == 0)
                {
                    DateTime defaultValue = DateTime.Parse(DateTime.Now.ToString("MM/1/yyyy")).Date;
                    base.setSessionCookie("cld", defaultValue.ToString());
                    return defaultValue;
                }

                return DateTime.Parse(val);
            }
            set
            {
                base.setSessionCookie("cld", value.Date.ToString());
            }
        }
        public Wcss._Enums.ViewingMode _viewMode
        {
            get
            {
                string val = base.getCookie("evm");
                if (val == null || val.Trim().Length == 0)
                {
                    string defaultValue = "lst";
                    base.setPersistentCookie("evm", defaultValue);
                    return Wcss._Enums.ViewingMode.List;
                }

                return (val == "lst") ? Wcss._Enums.ViewingMode.List : Wcss._Enums.ViewingMode.Calendar;
            }
            set
            {
                base.setPersistentCookie("evm", (value == Wcss._Enums.ViewingMode.List) ? "lst" : "cal");
            }
        }
        public Wcss._Enums.TicketManifestSortCriteria _ticketManifestSortCriteria
        {
            get
            {
                string val = base.getCookie("tmsc");
                if (val == null || val.Trim().Length == 0)
                {
                    string defaultValue = "apl";
                    val = defaultValue;
                    base.setPersistentCookie("tmsc", val);
                }

                if (val == "apl")
                    return Wcss._Enums.TicketManifestSortCriteria.alphabetical;
                else //if (val == "prch")
                    return Wcss._Enums.TicketManifestSortCriteria.purchasedate;
                //else
                //    return Wcss._Enums.TicketManifestSortCriteria.mostrecent;
            }
            set
            {
                string val = string.Empty;
                if (value == Wcss._Enums.TicketManifestSortCriteria.alphabetical)
                    val = "apl";
                else //if (value == Wcss._Enums.TicketManifestSortCriteria.purchasedate)
                    val = "prch";
                //else
                //    val = "most";

                base.setPersistentCookie("tmsc", val);
            }
        }

        public Wcss._Enums.ProductContext _ordersRecentSortCriteria
        {
            get
            {
                string val = base.getCookie("orsc");
                if (val == null || val.Trim().Length == 0)
                {
                    string defaultValue = "al";
                    val = defaultValue;
                    base.setPersistentCookie("orsc", val);
                }

                if (val == "al")
                    return Wcss._Enums.ProductContext.all;
                else if (val == "mr")
                    return Wcss._Enums.ProductContext.merch;
                else
                    return Wcss._Enums.ProductContext.ticket;
            }
            set
            {
                string val = string.Empty;
                if (value == Wcss._Enums.ProductContext.all)
                    val = "al";
                else if (value == Wcss._Enums.ProductContext.merch)
                    val = "mr";
                else
                    val = "tk";

                base.setPersistentCookie("orsc", val);
            }
        }

        public bool _actSearchLike
        {
            get
            {
                string val = base.getCookie("asl");
                if (val == null || val.Trim().Length == 0 || (!Utils.Validation.IsValBoolean(val)))
                {
                    string defaultValue = "1";
                    base.setPersistentCookie("asl", defaultValue);
                    return (defaultValue == "1");
                }

                return (val == "1");
            }
            set
            {
                base.setPersistentCookie("asl", (value) ? "1" : "0");
            }
        }

        public bool _promoterSearchLike
        {
            get
            {
                string val = base.getCookie("psl");
                if (val == null || val.Trim().Length == 0 || (!Utils.Validation.IsValBoolean(val)))
                {
                    string defaultValue = "1";
                    base.setPersistentCookie("psl", defaultValue);
                    return (defaultValue == "1");
                }

                return (val == "1");
            }
            set
            {
                base.setPersistentCookie("psl", (value) ? "1" : "0");
            }
        }

        public bool _venueSearchLike
        {
            get
            {
                string val = base.getCookie("vsl");
                if (val == null || val.Trim().Length == 0 || (!Utils.Validation.IsValBoolean(val)))
                {
                    string defaultValue = "0";
                    base.setPersistentCookie("vsl", defaultValue);
                    return (defaultValue == "1");
                }

                return (val == "1");
            }
            set
            {
                base.setPersistentCookie("vsl", (value) ? "1" : "0");
            }
        }
        public bool _goodorgSearchLike
        {
            get
            {
                string val = base.getCookie("gsl");
                if (val == null || val.Trim().Length == 0 || (!Utils.Validation.IsValBoolean(val)))
                {
                    string defaultValue = "0";
                    base.setPersistentCookie("gsl", defaultValue);
                    return (defaultValue == "1");
                }

                return (val == "1");
            }
            set
            {
                base.setPersistentCookie("gsl", (value) ? "1" : "0");
            }
        }
        public bool _charitableorgSearchLike
        {
            get
            {
                string val = base.getCookie("dnl");
                if (val == null || val.Trim().Length == 0 || (!Utils.Validation.IsValBoolean(val)))
                {
                    string defaultValue = "0";
                    base.setPersistentCookie("dnl", defaultValue);
                    return (defaultValue == "1");
                }

                return (val == "1");
            }
            set
            {
                base.setPersistentCookie("dnl", (value) ? "1" : "0");
            }
        }

        #endregion

        //current subscriptionEmail
        public int _seid
        {
            get
            {
                string id = base.getCookie("seid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("seid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("seid", value.ToString());
            }
        }
        /// <summary>
        /// donation year context
        /// </summary>
        public int _dyid
        {
            get
            {
                string id = base.getCookie("dyid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("dyid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("dyid", value.ToString());
            }
        }
        /// <summary>
        /// admin merch listing context - 0 for all, 1 for active only
        /// </summary>
        public int _admMlc
        {
            get
            {
                string id = base.getCookie("mlc");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("mlc", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("mlc", value.ToString());
            }
        }
        public int _vwMrc
        {
            get
            {
                string id = base.getCookie("vwmrc");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("vwmrc", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("vwmrc", value.ToString());
            }
        }
        public int _vwDwn
        {
            get
            {
                string id = base.getCookie("vwdwn");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("vwdwn", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("vwdwn", value.ToString());
            }
        }
        public int _vwTik
        {
            get
            {
                string id = base.getCookie("vwtik");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("vwtik", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("vwtik", value.ToString());
            }
        }
        public int _vwDte
        {
            get
            {
                string id = base.getCookie("vwdte");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("vwdte", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("vwdte", value.ToString());
            }
        }
        public int _acid
        {
            get
            {
                string id = base.getCookie("acid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("acid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("acid", value.ToString());
            }
        }
        /// <summary>
        /// promoter id
        /// </summary>
        public int _prid
        {
            get
            {
                string id = base.getCookie("prid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("prid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("prid", value.ToString());
            }
        }
        /// <summary>
        /// charity id
        /// </summary>
        public int _chid
        {
            get
            {
                string id = base.getCookie("chid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("chid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("chid", value.ToString());
            }
        }
        /// <summary>
        /// charity listing id
        /// </summary>
        public int _clid
        {
            get
            {
                string id = base.getCookie("clid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("clid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("clid", value.ToString());
            }
        }
        /// <summary>
        /// showlink id
        /// </summary>
        public int _slid
        {
            get
            {
                string id = base.getCookie("slid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("slid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("slid", value.ToString());
            }
        }
        /// <summary>
        /// tune id
        /// </summary>
        public int _tnid
        {
            get
            {
                string id = base.getCookie("tnid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("tnid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("tnid", value.ToString());
            }
        }
        /// <summary>
        /// Venue id
        /// </summary>
        public int _vnid
        {
            get
            {
                string id = base.getCookie("vnid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("vnid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("vnid", value.ToString());
            }
        }
        public int _Admin_ShowDateId
        {
            get
            {
                string id = base.getCookie("asdid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("asdid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("asdid", value.ToString());
            }
        }
        public int _Admin_JShowPromoterId
        {
            get
            {
                string id = base.getCookie("ajsproid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("ajsproid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("ajsproid", value.ToString());
            }
        }
        public int _Admin_JShowActId
        {
            get
            {
                string id = base.getCookie("ajsactid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("ajsactid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("ajsactid", value.ToString());
            }
        }
        public int _Admin_ShowTicketId
        {
            get
            {
                string id = base.getCookie("astktid");
                if (id == null || id.Trim().Length == 0 || (!Utils.Validation.IsInteger(id)))
                {
                    base.setSessionCookie("astktid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setSessionCookie("astktid", value.ToString());
            }
        }



        /*USER COOKIES*/
		public int _clientAcceptsCookies
		{ 
			get
			{
				string id = base.getCookie("ticks", "clt");
				if(id == null || id.Trim().Length == 0 || (! Utils.Validation.IsInteger(id)))
				{	
					return 0;
				}

				return int.Parse(id);
			}
			set
			{
                base.setCookieInCookieSet("ticks", "clt", value.ToString());
			}
		}

		public string _cartItems
		{
			get
			{
				string itms = base.getCookie("ticks", "crtitms");
				if(itms == null || itms.Trim().Length == 0)
				{
                    base.setCookieInCookieSet("ticks", "crtitms", string.Empty);
					return string.Empty;
				}

				return itms;
			}
			set
			{
                base.setCookieInCookieSet("ticks", "crtitms", value.ToString());
			}
		}

		public string _removedItems
		{
			get
			{
				string itms = base.getCookie("ticks", "remvd");
				if(itms == null || itms.Trim().Length == 0)
				{
                    base.setCookieInCookieSet("ticks", "remvd", string.Empty);
					return string.Empty;
				}

				return itms;
			}
			set
			{
                base.setCookieInCookieSet("ticks", "remvd", value.ToString());
			}
		}

        public int _showID
        { 
            get
            {
                string id = base.getCookie("ticks", "swvwid");
                if(id == null || id.Trim().Length == 0 || (! Utils.Validation.IsInteger(id)))
                {
                    base.setCookieInCookieSet("ticks", "swvwid", "0");
                    return 0;
                }

                return int.Parse(id);
            }
            set
            {
                base.setCookieInCookieSet("ticks", "swvwid", value.ToString());
            }
        }

        /// <summary>
        /// marketing program
        /// </summary>
		public string Mp
		{
			get
			{
				return base.getCookie("ticks", "mp");
			}
			set
			{
                base.setCookieInCookieSet("ticks", "mp", value.Trim());
			}
		}
        /// <summary>
        /// sale promotion
        /// </summary>
        public string Sp
        {
            get
            {
                return base.getCookie("ticks", "sp");
            }
            set
            {
                base.setCookieInCookieSet("ticks", "sp", value.Trim());
            }
        }
		
	}
}
