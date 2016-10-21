using System;

namespace Wcss
{
    public class _Enums
    {
        public enum OrdinalContext
        {
            merchcategorie,
            merchdivision,
            merchjoincat
        }
        public enum ActivationWindowContext
        {
            ProductAccess
        }
        public enum ProductAccessProductContext
        {
            merch,
            //showdate,
            ticket
        }

        public enum OrderedContext
        {
            featured,
            NA
        }

        public enum HeaderImageContext
        {
            All = 0,
            Show,
            Merch,
            Cart,
            Checkout,
            Confirm,
            Account,
            Aux,
            About
        }

        public enum MerchDisplayTemplate
        {
            Legacy = 0,
            ThreeColumn,
            ControlsAboveRichText,
            ControlsBelowRichText,
            ControlsToRightOfDescription,
            ControlsToLeftOfDescription
        }

        public enum FB_Api
        {
            FB_Like,
            FB_UnLike,
            Likes
        }
        
        /// <summary>
        /// Tells us if it is a music file, data file, picture, report, etc
        /// </summary>
        public enum DownloadFileContext
        {
            music,
            data,
            picture,
            report,
            _NA
        }
        /// <summary>
        /// Refers to singletrack, fullalbum, side1, side2
        /// </summary>
        public enum DownloadTrackContext
        {   
            track,
            fullalbum,
            side1,
            side2,
            _NA
        }
        /// <summary>
        /// Csv, Mp3, Ogg Vorbis, jpg, tiff
        /// </summary>
        public enum DownloadFormat
        {   
            mp3,
            oggvorbis,
            flac,
            jpg,
            tiff,
            csv,
            _NA
        }

        public enum MailerContentName
        {
            highlight,
            banner,
            merch,
            mainlisting,
            secondarylisting,
            specialinterest,
            custom1,
            custom2
        }

        public enum RequirementContext
        {
            NA,
            merch,
            ticket,
            showdate,
            show,
            merchshipping,
            ticketshipping,
            minmerchpurchase,
            minticketpurchase,
            mintotalpurchase,
            userprovidedcode
        }
        public enum DiscountContext
        {
            merch,
            ticket,
            merchshipping,
            ticketshipping,
            processing,
            servicefees,
            triggeritemonly//tells us there is no rewarde per se - trigger applies a discount perhaps
        }

        public enum PendingOpContext
        {
            storecreditredemption
        }

        public enum PrintTicketItemType
        {
            PickupName,
            PurchaseName,
            NameOnCard,
            LastFourDigits,
            Email,
            InvoiceId,
            Quantity,
            Age,
            Notes,
            ReturnedToSenderRTS,
            ShipDate,
            ShipMethod,
            PhoneBilling, 
            PhoneShipping,            
            PhoneProfile
            //PickupName,PurchaseName,NameOnCard,LastFourDigits,Email,InvoiceId,Quantity,Age,Notes,ReturnedToSenderRTS,ShipDate,ShipMethod
        }

        public enum EmailTemplateContext
        {
            none,
            highlight,
            showlist,
            merchlist,
            trivia,
            briefshowlist
        }

        public enum EmailLetterSiteTemplate
        {
            ChangePasswordEmail_txt,
            ChangeUserName_txt,
            CustomerExchange_html,
            CustomerForgotPassword_html,
            CustomerForgotPassword_txt,
            CustomerRefund_html,
            CustomerEmailTemplate_html,
            MailerSignupNotification_txt,
            Message_txt,
            PasswordRecoveryEmail_txt,
            PasswordResetEmail_txt,
            PurchaseConfirmationEmail_html,
            RegisterEmail_txt,
            ShowDateChange_html
        }

        /// <summary>
        /// The delivery type specifies how the merchandis is tranferred to the user
        /// onsite->a promo code, email - electronic other than an actual download (gift certificates)
        /// </summary>
        public enum DeliveryType
        {
            parcel,
            download,
            giftcertificate,
            activationcode
        }

        public enum ShippingCarrier { UPS, USPS, FEDEX, NOTSPECD }
        public static string[] ShippingMethods = { "USPS First-Class Mail Parcel", "USPS Priority Mail", "UPS Ground", 
            "UPS 3 Day Select", "UPS 2nd Day Air", "UPS Standard UPS", "UPS Worldwide ExpeditedSM", "UPS Worldwide ExpressSM", 
            "UPS Worldwide Express PlusSM" };

        public enum PublishEvent
        {
            Publish,
            PublishAPI,
            EndPublish
        }
        public enum ProcessStatus {
            pending = 0,
            approved,
            denied,
            cancelledbyuser
        }
        public enum TicketInfoChoices
        {
            Tickets,
            Music,
            Info
        }
        public enum WimpyPlayer_Skins
        {
            fox_simple = 0,
            fox_single = 1,
            skin_aiwa,//silver good
            skin_aqua,//just a trace of blue
            skin_blackdawn,//charcoal with eq orange highlighting ***
            skin_blanco,//plain janeic enum TicketManifestSortCriteria
            skin_bop,//ehh
            skin_button,//like wimpy button - make smallalphabetical, purchasedate//, mostrecent
            skin_default,//nice bronze appearance - has pic window
            skin_fruity,//charcoal with blue green highlight
            skin_horizontial,//needs to be wide and thin - not bad<summary>
            skin_ipod,//just like a white ipodallotment and damage are for inventory history only - the rest are for pricing
            skin_itune,//has pic
            skin_itunes_plain,//same as above without pic - should be very wideic enum HistoryInventoryContext
            skin_kenetix,//ok black./charcoal
            skin_micro,//blahAllotment,//
            skin_mini,//best wide and thinDamage,
            skin_mini_classic,//good for one offs - wide and thinAdvancePrice,
            skin_nano,//good for one offs - wide and thin - blockyDosPrice,
            skin_porchtech,//stylish - play with sizingParentPrice,
            skin_plain,//looks similar to defaultChildPrice,
            skin_simple_bar,//good for one off - wide and thinServiceCharge
            skin_wimpy_bar,
            skin_wimpy_horiz,
            skin_wimpy_vert,//thin and tallic enum ViewingMode
            skin_winamp_horiz,//good bg-images need redoing - silverish blue
            skin_winamper,
            fox_simpleblk

        }
        public enum TicketManifestSortCriteria
        {
            alphabetical, purchasedate//, mostrecent
        }

        /// <summary>
        /// allotment and damage are for inventory history only - the rest are for pricing
        /// </summary>
        public enum HistoryInventoryContext
        {
            Allotment,
            Damage,
            AdvancePrice,
            DosPrice,
            ParentPrice,
            ChildPrice,
            ServiceCharge
        }

        public enum ViewingMode
        {
            List,
            Calendar
        }

        /// <summary>
        /// Note that these characters are searched on "x," be sure to make ending letter unique
        /// </summary>
        public enum ItemContextCode
        {
            t,//tickets
            m,//merch
            b,//bundle
            //s,//bundleselection//will show as a merch item
            //p,//ticketpackage
            //c,//linked child invoice item - shipping
            f,//promotionalItem
            //g,//giftcertificate
            y,//charity
            o //for some other time - other
        }

        //this has been passed over the ever more encompassing invoiceItemContext
        //public enum StockCategory
        //{
        //    tickets = 1,
        //    merch = 2
        //}
        public enum InventoryCheck_Context
        {
            Merch,
            MerchPromo,
            TicketPromo,
            Ticket
        }
        /// <summary>
        /// a broader context list for invoice items
        /// </summary>
        public enum ProductContext
        {
            all,
            merch,
            ticket
        }
        
        public enum InvoiceItemContext
        {
            ticket,
            merch,
            processing,
            servicecharge,
            linkedshippingticket,//shipping added after the fact//leave for legacy//no longer used
            shippingticket,
            shippingmerch,
            promotion,
            discount,//promotional
            refund,//for after purchase discount
            damaged,//purchasedThenRemoved only
            //storecredit,//giftcertificate,
            bundle,
            charity,
            notassigned,
            noteitem
        }

        //keep in synch with StateManager _LookupTableNames && _Lookits....
        //MAKE PLURAL!!!!!
        public enum LookupTableNames  
        {
            Ages, 
            CharityListings, 
            Deals,
            Employees,
            FaqCategories,
            FaqItems, //Genres,
            HeaderImages,
            HintQuestions,
            InvoiceFees,  
            MerchBundles,
            MerchCategories,
            MerchColors, MerchDivisions, MerchSizes, 
            ProductAccessors,
            SaleRules, ServiceCharges, ShowStatii, 
            SiteConfigs, MerchImages, Vendors, Subscriptions, SalePromotions
        }

        public enum VendorTypes
        {
            online, 
            boxoffice,
            all
        }

        public enum ConfigDataTypes
        {
            _string,
            _int,
            _decimal,
            _boolean
        }

        public enum SiteConfigContext
        {
            Images,
            FB_Integration,
            Flow,
            Default,
            Ship,
            PageMsg,
            Email,
            Admin,
            Service,
            Download
        }

        public enum EventQStatus
        {
            Pending,
            Processing,
            Success,
            Failed,
            UserNotFound
        }

        public enum EventQContext
        {
            AdminNotification,
            //Inventory,
            Invoice,
            ShowDate,
            ShowTicket,
            User,
            Report,
            Mailer,
            Merch,
            SalePromotion
        }

        public enum EventQVerb
        {
            _Create,
            _Read,
            _Update,
            _Delete,
            _ActAdded,
            _PromotionAdded,
            _TicketAdded,
            AccountUpdate,
            AgeVerify18Submission,
            AuthDecline,
            SentCorrespondence,
            ResendConfirmationEmail,
            CartCleared,
            ChangePickupName,
            ChangeShipDate,
            ChangeShipMethod,
            ChangeShipNotes,
            ChangeNotes,
            ChangeShipAddress,
            ChangeUserName,
            DeliveryCodeReissued,
            IncorrectPasswordHintSubmitted,
            InvalidCoupon,
            InventoryError,
            InventoryNotification,
            InventoryTransferred,
            PasswordReset,
            PromotionNotSelected,
            Publish,
            Refund,
            Role_Delete,
            Role_Add,
            Report_Mailer_Daily,
            TrackingChange,
            RequestPassword,
            StoreCreditAdjustment,
            SubscriptionUpdate,
            StartingDownload,
            SuccessfulDownload,
            UserCreated,
            UserSentContactMessage,
            UserSentRegistrationConfirm,
            UserPurchase,
            UserLogin,
            UserPointsActivity,
            UserUpdate,
            Exchange,
            LotteryStatusUpdate,
            Mailer_Remove,
            Mailer_SignupAwaitConfirm,
            Mailer_FailureNotification,
            Merch_SalePriceChange,
            Checkout_TicketItemsExpired
        }

        public enum SiteEntityMode
        {
            Act,
            Venue,
            Promoter
        }
        
        public enum SessionRequirements
        {
            None,
            Session,
            SSLSession
        }

        public enum AuthenticationTypes
        {
            None,
            WebUser,
            WebAdmin
        }

        public enum EmailFormats
        {
            html,
            text
        }
        public enum GenderTypes
        {
            male,
            female,
            noneSpecified
        }
        public enum SiteRequestVars
        {
            bc,		//barcode to text out
            cat,	//merchCategory
            chk,	//indicates whether or not to bypass the error page
            cwd,	//continue what i was doing - qs for intended url - much like onLoginRedirect
            emd,	//errorMode - crt (cart removals)
            inv,	//inventory number
            mdv,	//merch division
            mite,	//merchItem number
            mo,		//month of show list to display
            p,		//pageToLoad
            sid,	//showid
            sdd,	//showDateId
            tid		//ticketId
        }

        public enum HtmlFormatMode
        {
            none,
            simple,
            all
        }

        public enum FaqCategories
        {
            General,
            Tickets,
            Merch,
            Shipping
        }

        public enum InvoiceStatii
        {
            NotPaid,
            Paid,
            PartiallyRefunded,
            Refunded,
            Void
        }

        public enum TransTypes
        {
            Payment,
            Refund,
            Chargeback,
            Void
        }
        public enum UserPointAction
        {
            added,
            denied,
            redeemed
        }

        public enum FundsTypes
        {
            CreditCard,
            CompanyCheck,
            StoreCredit
        }

        public enum ReferenceSources
        {
            Invoice,
            Admin,
            Other
        }
        public enum PerformedByTypes
        {
            AdminSite,
            CustomerSite
        }

        public enum FundsProcessor
        {
            FirstPay,
            LinkPoint,
            AuthorizeNet,
            Internal
        }

        public enum PurchaseActions
        {
            Purchased,
            NotYetPurchased,
            PurchasedThenRemoved,
            Credited
        }

        public enum ShowDateStatus
        {
            Pending = 0,
            OnSale,
            SoldOut,
            Cancelled,
            Postponed,
            Moved,
            NotActive
        }
    }
}
