using System;
using System.Web;
using System.Web.SessionState;
using System.Web.Caching;
using System.Linq;

using Wcss;

namespace WillCallWeb
{	
	/// <summary>
	/// Summary description for SessionContext.
	/// </summary>
	public class WebContext : _ContextBase
	{
        //public void ResetEntireCache()
        //{
        //    base.Publish();
        //}

		#region WebContext, Cache Overhead And Callbacks

		public SessionCookie Scookie;
		public HttpSessionState Session;


		public WebContext(HttpSessionState session)
		{
			Session = session;
			Session.Timeout = 30;
			Scookie = new SessionCookie();
		}

		public WebContext()
		{
			if (HttpContext.Current.Session != null)
			{
				Session = HttpContext.Current.Session;
				Session.Timeout = 30;
				Scookie = new SessionCookie();
			}

			if (this.Scookie == null)
				Scookie = new SessionCookie();
		}

		#endregion

		#region ***Show, Date and Ticket Collections - Utils.StateManager - put here due to reliance on Linq***

		/// <summary>
		/// This helps us determine what is displayable in the moment - determines validity of announcedate. Note
		/// that this is for show and showDates - tickets need to be further checked separately within context of mp key
        /// Displayable is: showdate.IsActive and Show.IsActive && Show.AnnounceDate < DateTime.Now
		/// </summary>
		/// <param name="coll"></param>
		/// <returns></returns>
		public ShowDateCollection OrderedDisplayable_ShowDates
		{
			get
			{
				ShowDateCollection returnColl = new ShowDateCollection();

				if(SaleShowDates_All != null && SaleShowDates_All.Count > 0)
				{
					//get default venue
					//order by venue, then date
					System.Collections.Generic.List<ShowDate> list = new System.Collections.Generic.List<ShowDate>();
					list.AddRange(SaleShowDates_All.GetList().FindAll(delegate(ShowDate match) 
                    {
                        //return (match.IsActive && match.IsGeneralPublicDisplay && match.ShowRecord.IsDisplayable); 
                        return (match.IsActive && match.ShowRecord.IsDisplayable && (!match.IsPrivateShow)); 
                    }
                    ));

					if (_Config._Site_Entity_Mode == _Enums.SiteEntityMode.Venue)
					{
						var sortedColl =
							from listItem in list
							select listItem;

						returnColl.AddRange(sortedColl.OrderBy(x => x.DateOfShow_ToSortBy)
							.ThenBy(x => (x.ShowRecord.VenueRecord.Name_Displayable.ToLower() == _Config._Default_VenueName.ToLower()) ? string.Empty : x.ShowRecord.VenueRecord.NameRoot));

						#region SHOWINGS
						//let clients handle this - showing are multiple showtimes of the same event
						//ie: movie times
						#endregion
					}
					else
					{
						var sortedColl =
							from listItem in list
							orderby listItem.DateOfShow_ToSortBy, listItem.ShowRecord.VenueRecord.Name_Displayable
							select listItem;

						returnColl.AddRange(sortedColl);
					}
				}

				return returnColl;
			}
		}

		/// <summary>
		/// Gets the ordered displayable show dates without any mult showing dupes
		/// </summary>
		public ShowDateCollection OrderedDisplayable_ShowDates_DupesRemoved
		{
			get
			{
				if (Utils.StateManager.Instance.Cache["OrderDisplayable_Dates_NoDupe"] == null)
				{
					ShowDateCollection returnColl = new ShowDateCollection();

					returnColl.AddRange(OrderedDisplayable_ShowDates);

					//MULTIPLES
					//eliminate - mult showdates on the same day - within the same show group
					//find matching showdate and show id
					var mults =
						from listItem in returnColl.GetList()
						group listItem by new { listItem.TShowId, listItem.DateOfShow.AddHours(-3).Date }//offset the date for late night shows
							into myGroup
							where myGroup.Count() > 1
							select new { myGroup.Key.TShowId, myGroup.Key.Date, groupCount = myGroup.Count() };

					if (mults.Count() > 0)
					{
						foreach (object o in mults)
						{
							int showId = (int)Utils.Reflector.EvaluateExpression(o, "TShowId");

							//load collection with matches
							System.Collections.Generic.List<ShowDate> multShowings = new System.Collections.Generic.List<ShowDate>();
							multShowings.AddRange(returnColl.GetList().FindAll(delegate(ShowDate match) { return (match.TShowId == showId); }));
							//sort by date
							multShowings.Sort(new Utils.Reflector.CompareEntities<ShowDate>(Utils.Reflector.Direction.Ascending, "DtDateOfShow"));

							//remove all but first from the dates collection
							int i = 0;
							foreach (ShowDate sd in multShowings)
							{
								if (i++ > 0)
									returnColl.Remove(sd);
							}
						}
					}

					Utils.StateManager.Instance.Cache.Insert("OrderDisplayable_Dates_NoDupe", returnColl, null, DateTime.Now.AddMinutes(1), System.Web.Caching.Cache.NoSlidingExpiration);
				}

				//    return returnColl;
				return (ShowDateCollection)Utils.StateManager.Instance.Cache["OrderDisplayable_Dates_NoDupe"];
			}
		}

		#endregion

		#region Session Objects

		public System.Collections.Generic.List<string> SalePromotion_CouponCodes
		{
			get
			{
				if (Session["CouponCodes"] == null)
					Session["CouponCodes"] = new System.Collections.Generic.List<string>();

				return (System.Collections.Generic.List<string>)Session["CouponCodes"];
			}
			set { Session["CouponCodes"] = value; }
		}
		public SalePromotion CurrentSalePromotion
		{
			get
			{
				if (Session["CurrentSalePromotion"] == null)
					return null;

				if (((SalePromotion)Session["CurrentSalePromotion"]).ApplicationId != _Config.APPLICATION_ID)
					Session.Remove("CurrentSalePromotion");

				return (SalePromotion)Session["CurrentSalePromotion"];
			}
			set
			{
				Session["CurrentSalePromotion"] = value;
			}
		}
		public string CurrentPageException
		{
			get
			{
				if (Session["PageException"] == null)
					return null;

				return (string)Session["PageException"];
			}
			set
			{
				Session["PageException"] = value;
			}
		}
		public string CurrentCartException
		{
			get
			{
				if (Session["CartException"] == null)
					return null;

				return (string)Session["CartException"];
			}
			set
			{
				Session["CartException"] = value;
			}
		}
		/// <summary>
		/// tells us where to redirect to after an old user is update
		/// </summary>
		public string RedirectOnAuth
		{
			get
			{
				return (string)Session["RedirectOnAuth"];
			}
			set
			{
				if (value == null)
					Session.Remove("RedirectOnAuth");
				else
					Session["RedirectOnAuth"] = value;
			}
		}
		public AspnetUsersOld OldUser
		{
			get
			{
				return (AspnetUsersOld)Session["OldUser"];
			}
			set
			{
				if (value == null)
					Session.Remove("OldUser");
				else
					Session["OldUser"] = value;
			}
		}
		public decimal ApplyInvoiceCredit
		{
			get
			{
				if (Session["ApplyInvoiceCredit"] == null)
					return 0;

				return decimal.Round((decimal)Session["ApplyInvoiceCredit"], 2);
			}
			set
			{
				Session["ApplyInvoiceCredit"] = value;
			}
		}
		/// <summary>
		/// Guarantees that we have the correct id for printing barcode. Spoofs users putting in their own querystring
		/// </summary>
		public string BarCodeText { get { return (string)Session["BarCodeText"]; } set { Session["BarCodeText"] = value; } }

		/// <summary>
		/// stores the current captcha code. Dont store in cookie - session is more secure
		/// </summary>
		public string CurrentCaptcha
		{
			get
			{
				if (Session["CurrentCaptcha"] == null)
					Session["CurrentCaptcha"] = Utils.CaptchaImage.GenerateRandomCode();

				return (string)Session["CurrentCaptcha"];
			}
		}
		/// <summary>
		/// resets the code
		/// </summary>
		public void UpdateCaptcha()
		{
			Session["CurrentCaptcha"] = null;
		}
		/// <summary>
		/// this should live as short as possible
		/// </summary>
		public int InvoiceId
		{
			get
			{
				if (Session["InvoiceId"] == null)
					return 0;

				return (int)Session["InvoiceId"];
			}
			set
			{
				Session["InvoiceId"] = value;
			}
		}
		public Invoice SessionInvoice
		{
			get
			{
				if (InvoiceId == 0)
					return null;

				if (InvoiceId > 0 && (Session["SessionInvoice"] == null || ((Invoice)Session["SessionInvoice"]).Id != InvoiceId))
					Session["SessionInvoice"] = new Invoice(InvoiceId);

				if (Session["SessionInvoice"] != null && ((Invoice)Session["SessionInvoice"]).ApplicationId != _Config.APPLICATION_ID)
					Session.Remove("SessionInvoice");

				return (Invoice)Session["SessionInvoice"];
			}
			set
			{
				if (value == null)
					InvoiceId = 0;
				else
					InvoiceId = value.Id;

				Session["SessionInvoice"] = value;
			}
		}

		/// <summary>
		/// Guarantees that we have or haven't attempted authorization at some point. When null - we have not processed.
		/// Values are too sensitive for cookie storage
		/// </summary>
		public string OrderProcessingVariables { get { return (string)Session["OrderProcessingVariables"]; } set { Session["OrderProcessingVariables"] = value; } }

		public int AuthId
		{
			get
			{
				if (Session["AuthId"] == null)
					return 0;

				return (int)Session["AuthId"];
			}
			set
			{
				Session["AuthId"] = value;
			}
		}
		/// <summary>
		/// The current authorization object for authorize.Net
		/// </summary>
		public AuthorizeNet SessionAuthorizeNet
		{
			get
			{
				if (AuthId == 0)
					return null;
				if (AuthId > 0 && (Session["SessionAuthorizeNet"] == null || ((AuthorizeNet)Session["SessionAuthorizeNet"]).Id != AuthId))
					Session["SessionAuthorizeNet"] = new AuthorizeNet(AuthId);

				return (AuthorizeNet)Session["SessionAuthorizeNet"];
			}
			set
			{
				if (value == null)
					AuthId = 0;
				else
					AuthId = value.Id;

				Session["SessionAuthorizeNet"] = value;
			}
		}

		/// <summary>
		/// The current shopping cart
		/// </summary>
		public StoreObjects.ShoppingCart Cart
		{
			get
			{
				if (Session["ShoppingCart"] == null)
					Session["ShoppingCart"] = new StoreObjects.ShoppingCart();

				return (StoreObjects.ShoppingCart)Session["ShoppingCart"];
			}
		}

		#endregion 

		#region Promotion Methods

		public void SalePromotion_CouponCodes_Add(string coupon, bool ignoreCase)
		{
			//if not found - and we have less than say - five coupons
			if (this.SalePromotion_CouponCodes.Count >= _Config._Coupon_MaxNumAllowed)
				throw new Exception(string.Format("You may only apply {0} coupon(s) per order.", _Config._Coupon_MaxNumAllowed));

			//we need to track the two parts 
			string part1 = null;
			string part2 = null;

			//split coupon by dash
			if (coupon.IndexOf("-") != -1)
			{
				string[] parts = coupon.Split('-');
				part1 = parts[0];

				if (parts[1].Trim().Length > 0)
					part2 = parts[1];
			}
			else
				part1 = coupon;

			//only add if sale promo is running, etc - the coupon will only match ONE promo
			int idx = _Lookits.SalePromotions.GetList().FindIndex(delegate(SalePromotion match)
			{
				if (match.IsCurrentlyRunning(this.SalePromotionUnlock) && match.Requires_PromotionCode)
				{
					//cases here - a generic code 
					if (ignoreCase)
						return match.RequiredPromotionCode.ToLower() == part1.ToLower();
					else
						return match.RequiredPromotionCode == coupon;
				}

				return false;
			});

			//the -1 indicates that we have a matching promo for the coupon
			//only add if the code applies to something - match to sale promotions
			if (!this.SalePromotion_CouponCodes.Contains(coupon))
			{
				if (idx != -1)
				{
					this.SalePromotion_CouponCodes.Add(coupon);
					//this.Cart.FullfillPromotions();
				}
				else throw new Exception("The coupon you have entered is not valid. Please check your spelling and try again.");
			}
		}
		public void SalePromotion_CouponCodes_Clear()
		{
			this.SalePromotion_CouponCodes = null;
		}

		#endregion

		#region Cookie objects

		/// <summary>
		/// Use this to setup settings for ckeditor browser only
		/// </summary>
		public bool IsAuthd_CKFinder { get { return this.Scookie._isAuthdCKFinder; } set { this.Scookie._isAuthdCKFinder = value; } }

		public bool SearchLike_Act { get { return this.Scookie._actSearchLike; } set { this.Scookie._actSearchLike = value; } }
		public bool SearchLike_Promoter { get { return this.Scookie._promoterSearchLike; } set { this.Scookie._promoterSearchLike = value; } }
		public bool SearchLike_Venue { get { return this.Scookie._venueSearchLike; } set { this.Scookie._venueSearchLike = value; } }
		public bool SearchLike_GoodOrg { get { return this.Scookie._goodorgSearchLike; } set { this.Scookie._goodorgSearchLike = value; } }
		public bool SearchLike_CharitableOrg { get { return this.Scookie._charitableorgSearchLike; } set { this.Scookie._charitableorgSearchLike = value; } }

        public bool UserIs18OrOlder
        {
            get
            {
                return (UserAgeComplianceDate != DateTime.MaxValue.Date && UserAgeComplianceDate.AddYears(18) <= DateTime.Now);
            }
        }
        public DateTime UserAgeComplianceDate
        {
            get
            {
                return this.Scookie._userAgeComplianceDate;
            }
            set
            {
                this.Scookie._userAgeComplianceDate = value;
            }
        }
		public DateTime UserCalendarDate
		{
			get
			{
				return this.Scookie._calendarDate;
			}
			set
			{
				this.Scookie._calendarDate = value;
				WebContext.OnCalendarDateChanged(null);
			}
		}
		public _Enums.TicketManifestSortCriteria Search_TicketManifestSort
		{
			get
			{
				try
				{
					return this.Scookie._ticketManifestSortCriteria;
				}
				catch (Exception) { }

				return _Enums.TicketManifestSortCriteria.alphabetical;
			}
			set
			{
				this.Scookie._ticketManifestSortCriteria = value;
			}
		}
		public _Enums.ProductContext Orders_RecentSort
		{
			get
			{
				try
				{
					return this.Scookie._ordersRecentSortCriteria;
				}
				catch (Exception) { }

				return _Enums.ProductContext.all;
			}
			set
			{
				this.Scookie._ordersRecentSortCriteria = value;
			}
		}
		/// <summary>
		/// Holds any keys for special handling - referals
		/// </summary>
		public string MarketingProgramKey { get { return this.Scookie.Mp; } set { this.Scookie.Mp = value; } }
		public string SalePromotionUnlock { get { return this.Scookie.Sp; } set { this.Scookie.Sp = value; } }

		/// <summary>
		/// We write the cart items to a cookie for javascript access
		/// </summary>
		public string CurrentCartItems { get { return this.Scookie._cartItems; } set { this.Scookie._cartItems = value; } }
		/// <summary>
		/// We write the removed cart items to a cookie for javascript access
		/// </summary>
		public string CurrentCartRemovals { get { return this.Scookie._removedItems; } set { this.Scookie._removedItems = value; } }

		public DateTime UpcomingShowStartDate
		{
			get
			{
				DateTime usd = Scookie._upcomingShowStartDate;

				//always return the offset if it is todays date
				//if(usd.Date == DateTime.Now.Date)
				//    usd = _Config.SHOWOFFSET_SET(usd);

				return usd; //_Config.SHOWOFFSET_SET(usd).Date;
			}
			set
			{
				Scookie._upcomingShowStartDate = value;
			}
		}

		#endregion

		#region Events

		public class AddShowHistoryEventArgs : EventArgs
		{
			protected int _idx;

			//Default Constructor
			public AddShowHistoryEventArgs()
			{
				_idx = 0;
			}

			//Alt Constructor
			public AddShowHistoryEventArgs(int idx)
			{
				_idx = idx;
			}

			public int ShowHistoryId { get { return _idx; } }
		}

		public delegate void AddShowHistoryEventHandler(object sender, AddShowHistoryEventArgs e);
		public static event AddShowHistoryEventHandler AddShowHistoryChanged;
		public static void OnAddShowHistoryChanged(object sender, int idx)
		{
			if (AddShowHistoryChanged != null)
			{
				AddShowHistoryChanged(sender, new AddShowHistoryEventArgs(idx));
			}
		}

		public delegate void CalendarDateChange_EventHandler(object sender, EventArgs e);
		public static event CalendarDateChange_EventHandler CalendarDateChanged;
		public static void OnCalendarDateChanged(object sender)
		{
			if (CalendarDateChanged != null)
			{
				CalendarDateChanged(sender, new EventArgs());
			}
		}

		#endregion 

		#region MISC

		public string DisplayErrorText(string error)
		{
			return string.Format("<div class=\"errordisplay\">{0}</div>", error);
		}

		public void LogoutUser()
		{
			OrderProcessingVariables = null;

			if (Cart != null && Cart.HasItems)
				Cart.ClearCart();

			Session.RemoveAll();
			Session.Abandon();

			System.Web.Security.FormsAuthentication.SignOut();
		}

		/// <summary>
		/// Sets the order processing variables - follow the parameter list
		/// </summary>
		public void SetProcessingVariables(string cardName, string cardNumber, string expMonth, string expYear, string securityCode)
		{
			this.OrderProcessingVariables = string.Format("{0}~{1}~{2}~{3}~{4}",
				cardName.Replace("~", string.Empty), cardNumber.Replace("~", string.Empty), expMonth.Replace("~", string.Empty),
				expYear.Replace("~", string.Empty), securityCode.Replace("~", string.Empty));
		}

		public Show RetrieveShowRecordFromDatesByShowId(int showIdx, bool showMustBeDisplayable)
		{
			ShowDateCollection coll = new ShowDateCollection();
            if(this.SaleShowDates_All != null)
            {
			    coll.AddRange(this.SaleShowDates_All.GetList().FindAll(delegate(ShowDate match) { return (match.TShowId == showIdx); }));

			    if (coll.Count > 0)
			    {
				    Show s = coll[0].ShowRecord;

				    if (showMustBeDisplayable && (!s.IsDisplayable))
					    return null;

				    return s;
			    }
            }

			return null;
		}

		#endregion

	}
}
