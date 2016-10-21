using System;
using System.Configuration;
using System.Collections.Generic;

namespace Wcss
{
    public partial class _Config
    {
        private static string _fbintegration = null;
        public static string FB_RENDERINTEGRATION(int showId, string showUrl)
        {
            if (_fbintegration == null)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    
                sb.AppendLine("    window.fbAsyncInit = function() {");
                sb.AppendFormat("        FB.init({{appId: '{0}', status: false, cookie: true, xfbml: true}});", _Config._FacebookIntegration_App_Id);
                sb.AppendLine();
                sb.AppendLine("        FB.Event.subscribe('edge.create', function(href, widget) { ");
                sb.AppendFormat("            exec_CallFBService(\"{0}\", fbSuccess, fbError); ", _Enums.FB_Api.FB_Like.ToString());
                sb.AppendLine("        });");
                sb.AppendLine();
                sb.AppendLine("        FB.Event.subscribe('edge.remove', function(href, widget) { ");
                sb.AppendFormat("            exec_CallFBService(\"{0}\", fbSuccess, fbError); ", _Enums.FB_Api.FB_UnLike.ToString());
                sb.AppendLine("        });");
                sb.AppendLine();
                sb.AppendLine("    };");
                sb.AppendLine();
                sb.AppendLine("    (function() {");
                sb.AppendLine("        var e = document.createElement('script'); ");
                sb.AppendLine("        e.async = true;");
                sb.AppendLine("        e.src = document.location.protocol + '//connect.facebook.net/en_US/all.js';");
                sb.AppendLine("        document.getElementById('fb-root').appendChild(e);");
                sb.AppendLine("    }());");
                sb.AppendLine();
                sb.AppendLine();


                sb.AppendLine("function exec_CallFBService(fn, successFn, errorFn) { ");
                sb.AppendFormat("    var pagePath = '{0}'; ", showUrl);
                sb.AppendFormat("    var showidx = '{0}'; ", showId.ToString());
                sb.AppendLine();

                sb.AppendLine("    paramList = '{ \"entityId\" : \"' + showidx + '\", \"entityLink\" : \"' + pagePath + '\" }'; ");

                sb.AppendLine("    $.ajax({ ");
                sb.AppendLine("        type: 'POST', ");

                sb.AppendLine("        url: '/Services/FBService.asmx/' + fn, ");
                sb.AppendLine("        contentType: 'application/json; charset=utf-8', ");
                sb.AppendLine("        data: paramList, ");
                sb.AppendLine("        dataType: 'json', ");
                sb.AppendLine("        success: successFn, ");
                sb.AppendLine("        error: errorFn ");
                sb.AppendLine("    }); ");
                sb.AppendLine("} ");

                //see /JQueryUI/stuHover.js for more on these functions
                sb.AppendLine("function fbSuccess(result) {  ");
                //sb.AppendLine("    alert('FB success complete!');  ");
                sb.AppendLine("} ");
                sb.AppendLine("function fbError(xhr, ajaxOptions, thrownError) {  ");
                //sb.AppendLine("    alert('FB error complete!');  ");
                sb.AppendLine("} ");


                _fbintegration = sb.ToString();
            }

            return _fbintegration;
        }

        private static string _fblike = null;
        public static string FB_RENDERLIKECONTROL
        {
            get
            {
                if (_fblike == null)
                {
                    System.Text.StringBuilder ret = new System.Text.StringBuilder();

                    string likelayout = _Config._FacebookIntegration_Like_Layout;
                    string likeaction = _Config._FacebookIntegration_Like_Action;
                    string showfaces = _Config._FacebookIntegration_Like_ShowFaces.ToString().ToLower();
                    string colorscheme = _Config._FacebookIntegration_Like_ColorScheme;
                    int width = _Config._FacebookIntegration_Like_Width;
                    int height = _Config._FacebookIntegration_Like_Height;

                    ret.Append("<span class=\"fb-control\">");

                    ret.AppendFormat("<fb:like ref=\"main_event\" layout=\"{0}\" show_faces=\"{1}\" action=\"{2}\"{3} colorscheme=\"{4}\"{5} ",
                        likelayout, showfaces, likeaction, 
                        (width > 0) ? string.Format(" width=\"{0}\"", width.ToString()) : string.Empty,                        
                        colorscheme, 
                        (height > 0) ? string.Format(" height=\"{0}\"", height.ToString()) : string.Empty);
                    ret.AppendFormat("expr:href='data:post.url' ></fb:like>");

                    ret.AppendFormat("</span>");

                    _fblike = ret.ToString();
                }

                return _fblike;
            }
        }

        public static void TestFacebook()
        {
            string val;
            int idx;
            bool bbb;


            val = _Config._FacebookIntegration_App_AdminList;
            val = _Config._FacebookIntegration_App_ApiKey;
            val = _Config._FacebookIntegration_App_Id;
            val = _Config._FacebookIntegration_App_Name;
            val = _Config._FacebookIntegration_App_Secret;
            val = _Config._FacebookIntegration_App_Url;
            val = _Config._FacebookIntegration_Like_Action;
            bbb = _Config._FacebookIntegration_Like_Active;
            val = _Config._FacebookIntegration_Like_ColorScheme;
            idx = _Config._FacebookIntegration_Like_Height;
            val = _Config._FacebookIntegration_Like_Layout;
            //bbb = _Config._FacebookIntegration_Like_RenderAsIFrame;
            bbb = _Config._FacebookIntegration_Like_ShowFaces;
            idx = _Config._FacebookIntegration_Like_Width;

            val = _Config._Facebook_RSVP_LinkText;
            //bbb = _Config._Facebook_RSVP_Shows_Active;
            bbb = _Config._Facebook_RSVP_ShowDates_Active;
        }

        #region RSVP linking

        public static string _Facebook_RSVP_LinkText
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebook_rsvp_linktext" && match.ValueX != null );
                    });
                if (config != null && config.Id > 0 && config.ValueX != null && config.ValueX.Trim().Length > 0)
                    return config.ValueX;
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.FB_Integration, _Enums.ConfigDataTypes._string, 50,
                        "Facebook_RSVP_LinkText",
                        "Sets the text for the RSVP link",
                        "RSVP");

                    return config.ValueX;
                }
            }
        }
        //public static bool _Facebook_RSVP_Shows_Active
        //{
        //    get
        //    {
        //        SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
        //            delegate(SiteConfig match)
        //            {
        //                return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
        //                    match.Name.ToLower() == "facebook_rsvp_shows_active" && match.ValueX != null &&
        //                    Utils.Validation.IsBoolean(match.ValueX));
        //            });
        //        if (config != null && config.Id > 0)
        //            return bool.Parse(config.ValueX);
        //        else
        //        {
        //            config = _Config.AddNewConfig(_Enums.SiteConfigContext.FB_Integration, _Enums.ConfigDataTypes._boolean, 5,
        //                "Facebook_RSVP_Shows_Active",
        //                "Enables rsvp linking for shows.",
        //                "true");

        //            return bool.Parse(config.ValueX);
        //        }
        //    }
        //}
        public static bool _Facebook_RSVP_ShowDates_Active
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebook_rsvp_showdates_active" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.FB_Integration, _Enums.ConfigDataTypes._boolean, 5,
                        "Facebook_RSVP_ShowDates_Active",
                        "Enables rsvp linking for showdates.",
                        "true");

                    return bool.Parse(config.ValueX);
                }
            }
        }

        #endregion

        #region Like Button

        public static bool _FacebookIntegration_Like_Active
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_like_active" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.FB_Integration, _Enums.ConfigDataTypes._boolean, 5,
                        "FacebookIntegration_Like_Active",
                        "The master switch for allowing the facebook like button.",
                        "false");

                    return bool.Parse(config.ValueX);
                }
            }
        }
        

        //public static bool _FacebookIntegration_Like_RenderAsIFrame
        //{
        //    get
        //    {
        //        SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
        //            delegate(SiteConfig match)
        //            {
        //                return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
        //                    match.Name.ToLower() == "facebookintegration_like_renderasiframe" && match.ValueX != null &&
        //                    Utils.Validation.IsBoolean(match.ValueX));
        //            });
        //        if (config != null && config.Id > 0)
        //            return bool.Parse(config.ValueX);
        //        else
        //        {
        //            config = new SiteConfig();
        //            config.ApplicationId = _Config.APPLICATION_ID;
        //            config.Context = _Enums.SiteConfigContext.FB_Integration.ToString();
        //            config.DataType = _Enums.ConfigDataTypes._boolean.ToString().TrimStart('_');
        //            //config.Description = 
        //            config.DtStamp = DateTime.Now;
        //            config.MaxLength = 5;
        //            config.Name = "FacebookIntegration_Like_RenderAsIFrame";
        //            config.Description = "Toggles the like control from an IFrame to an XFBML implementation. XFBML does not allow hiding comments";
        //            config.ValueX = "false";
        //            config.Save();

        //            _Lookits.SiteConfigs.Add(config);

        //            return bool.Parse(config.ValueX);
        //        }
        //    }
        //}
        public static string _FacebookIntegration_Like_Layout
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_like_layout" && match.ValueX != null );
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.FB_Integration, _Enums.ConfigDataTypes._string, 50,
                        "FacebookIntegration_Like_Layout",
                        "Valid values are standard (minwidth: 225 - default 450x35 - 450x80 with photos), button_count (minwidth: 90 - default 90x20) and box_count (minwidth: 55 - default 55x65).",
                        "standard");

                    return config.ValueX;
                }
            }
        }
        public static string _FacebookIntegration_Like_Action
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_like_action" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.FB_Integration, _Enums.ConfigDataTypes._string, 50,
                        "FacebookIntegration_Like_Action",
                        "Valid values are like or recommend",
                        "like");

                    return config.ValueX;
                }
            }
        }
        public static string _FacebookIntegration_Like_ColorScheme
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_like_colorscheme" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.FB_Integration, _Enums.ConfigDataTypes._string, 50,
                        "FacebookIntegration_Like_ColorScheme",
                        "Valid values are light or dark",
                        "light");

                    return config.ValueX;
                }
            }
        }
        public static bool _FacebookIntegration_Like_ShowFaces
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_like_showfaces" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.FB_Integration, _Enums.ConfigDataTypes._boolean, 5,
                        "FacebookIntegration_Like_ShowFaces",
                        "Toggles the like control to show users pictures.",
                        "true");

                    return bool.Parse(config.ValueX);
                }
            }
        }
        public static int _FacebookIntegration_Like_Width
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_like_width" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.FB_Integration, _Enums.ConfigDataTypes._int, 10,
                        "FacebookIntegration_Like_Width",
                        "The width of the control. See docs for layout dimensions.",
                        "600");

                    return int.Parse(config.ValueX);
                }
            }
        }
        public static int _FacebookIntegration_Like_Height
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_like_height" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.FB_Integration, _Enums.ConfigDataTypes._int, 10,
                        "FacebookIntegration_Like_Height",
                        "The height of the control. See docs for layout dimensions.",
                        "35");

                    return int.Parse(config.ValueX);
                }
            }
        }

        #endregion

        #region App settings

        public static string _FacebookIntegration_App_Id
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_app_id" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.FB_Integration, _Enums.ConfigDataTypes._string, 50,
                        "FacebookIntegration_App_Id",
                        "The application id.",
                        "195855283782750");

                    return config.ValueX;
                }
            }
        }

        public static string _FacebookIntegration_App_Secret
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_app_secret" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.FB_Integration, _Enums.ConfigDataTypes._string, 50,
                        "FacebookIntegration_App_Secret",
                        "The application secret.",
                        "008f7be8b66b22f97d35ebb7d3654f21");

                    return config.ValueX;
                }
            }
        }

        public static string _FacebookIntegration_App_Name
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_app_name" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.FB_Integration, _Enums.ConfigDataTypes._string, 256,
                        "FacebookIntegration_App_Name",
                        "The registered application name.",
                        "Store");

                    return config.ValueX;
                }
            }
        }

        public static string _FacebookIntegration_App_Url
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_app_url" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.FB_Integration, _Enums.ConfigDataTypes._string, 256,
                        "FacebookIntegration_App_Url",
                        "The registered application's url.",
                        @"http://sts9store.com/");

                    return config.ValueX;
                }
            }
        }

        public static string _FacebookIntegration_App_ApiKey
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_app_apikey" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.FB_Integration, _Enums.ConfigDataTypes._string, 50,
                        "FacebookIntegration_App_ApiKey",
                        "The api key.",
                        "a839cd24db099de282ac78811c65a89a");

                    return config.ValueX;
                }
            }
        }

        public static string _FacebookIntegration_App_AdminList
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.FB_Integration.ToString().ToLower() &&
                            match.Name.ToLower() == "facebookintegration_app_adminlist" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.FB_Integration, _Enums.ConfigDataTypes._string, 1024,
                        "FacebookIntegration_App_AdminList",
                        "The list of admin ids - comma separated.",
                        "100000624633880");

                    return config.ValueX;
                }
            }
        }


        #endregion
    }
}
