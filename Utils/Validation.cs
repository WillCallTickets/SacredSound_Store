using System;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Utils
{
    public class Validation
    {
        private static readonly Regex regAllowableInput = new Regex(@"^[a-zA-Z0-9.#_\-\'\s]*$", RegexOptions.IgnoreCase);
        public static readonly Regex regexNumbersOnly = new Regex(@"^[0-9]*$", RegexOptions.IgnoreCase);
        /// <summary>
        /// This does allow single quotes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidAlphaInput(string input)
        {
            //only allow alpha numeric chars
            return regAllowableInput.Match(input).Success;
        }
        
        public static System.Drawing.Color indicatorColor = System.Drawing.Color.LightPink;
        private static string GetWebControlDefaultValue(WebControl ctrl)
        {
            PropertyInfo pi = ctrl.GetType().GetProperty("Text");
            if (pi == null)
                pi = ctrl.GetType().GetProperty("SelectedValue");

            if(pi != null)
                return pi.GetValue(ctrl, null).ToString();

            return null;
        }
        public static string ValInput_Email(System.Collections.Generic.List<string> errors, string fieldName, 
            WebControl ctrl, bool isRequired, bool indicateControl)
        {
            int errorCount = errors.Count;
            string val = GetWebControlDefaultValue(ctrl);

            ValInput_Email(errors, fieldName, val, isRequired);

            if (indicateControl && (errorCount != errors.Count))
                ctrl.BackColor = indicatorColor;

            return val;
        }
        public static string ValInput_Email(System.Collections.Generic.List<string> errors, string fieldName, 
            string val, bool isRequired)
        {
            if (isRequired && val.Length == 0)
                errors.Add(string.Format("{0} is required.", fieldName));


            if ((isRequired || (!isRequired && val.Length > 0)) &&
                (!Validation.IsValidEmail(val)))
                errors.Add(string.Format("{0} is not a valid email value.", fieldName));

            return val;
        }

        public static string ValInput_Alpha(System.Collections.Generic.List<string> errors, string fieldName, WebControl ctrl, bool isRequired, bool indicateControl)
        {
            int errorCount = errors.Count;
            string val = GetWebControlDefaultValue(ctrl);

            ValInput_Alpha(errors, fieldName, val, isRequired);

            if (indicateControl && (errorCount != errors.Count))
                ctrl.BackColor = indicatorColor;

            return val;
        }
        public static string ValInput_Alpha(System.Collections.Generic.List<string> errors, string fieldName, string val, bool isRequired)
        {
            if (isRequired && val.Length == 0)
                errors.Add(string.Format("{0} is required.", fieldName));

            if ((isRequired || (!isRequired && val.Length > 0)) &&
                (!Validation.IsValidAlphaInput(val)))
                errors.Add(string.Format("{0} is not a valid alphanumeric value.", fieldName));

            return val;
        }

        public static string ValInput_ByPattern(System.Collections.Generic.List<string> errors, string fieldName, WebControl ctrl, bool isRequired, bool indicateControl, Regex reg)
        {
            int errorCount = errors.Count;
            string val = GetWebControlDefaultValue(ctrl);

            ValInput_ByPattern(errors, fieldName, val, isRequired, reg);

            if (indicateControl && (errorCount != errors.Count))
                ctrl.BackColor = indicatorColor;

            return val;
        }
        public static string ValInput_ByPattern(System.Collections.Generic.List<string> errors, string fieldName, string val, bool isRequired, Regex reg)
        {
            if (isRequired && val.Length == 0)
                errors.Add(string.Format("{0} is required.", fieldName));

            if ((isRequired || (!isRequired && val.Length > 0)) &&
                (!reg.Match(val).Success))
                errors.Add(string.Format("{0} is not a valid value.", fieldName));

            return val;
        }



        public static bool ValInput_ServerValidation_IsValid(System.Collections.Generic.List<string> errors, System.Web.UI.WebControls.CustomValidator validator)
        {
            if (errors.Count > 0)
            {
                if (validator != null)
                {
                    validator.IsValid = false;
                    validator.ErrorMessage = Utils.Validation.DisplayValidationErrors(errors);
                }

                errors.Clear();

                return false;
            }

            return true;
        }












        public static void ValidateRequiredField(System.Collections.Generic.List<string> errors, string requiredFieldName, string value)
        {
            if (value == null || value.Trim().Length == 0)
                errors.Add(string.Format("{0} is required", requiredFieldName));
        }
        /// <summary>
        /// validates a decimal - nulls are not evaluated - this is not for required values
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="requiredFieldName"></param>
        /// <param name="value"></param>
        public static void ValidateNumericField(System.Collections.Generic.List<string> errors, string requiredFieldName, string value)
        {
            //we pass nulls here - we are not checking for required - just value
            if (value != null && value.Trim().Length > 0 && (!Utils.Validation.IsDecimal(value)))
                errors.Add(string.Format("{0} must be a numeric value", requiredFieldName));
        }
        /// <summary>
        /// nulls are not evaluated - this is not for required values
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="requiredFieldName"></param>
        /// <param name="value"></param>
        public static void ValidateIntegerField(System.Collections.Generic.List<string> errors, string requiredFieldName, string value)
        {
            //we pass nulls here - we are not checking for required - just value
            if (value != null && value.Trim().Length > 0 && (!Utils.Validation.IsInteger(value)))
                errors.Add(string.Format("{0} must be an integer value", requiredFieldName));
        }
        public static void ValidateIntegerRange(System.Collections.Generic.List<string> errors, string requiredFieldName, string value, int start, int end)
        {
            //we pass nulls here - we are not checking for required - just value
            if (value != null && value.Trim().Length > 0 && Utils.Validation.IsInteger(value))
            {
                int val = int.Parse(value);
                if(val < start || val > end)
                    errors.Add(string.Format("{0} must be a value between {1} and {2}", requiredFieldName, start.ToString(), end.ToString()));
            }   
        }
        /// <summary>
        /// takes the list of errors and forms them into an UNORDERED (UL) list
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static string DisplayValidationErrors(System.Collections.Generic.List<string> errors)
        {
            if (errors.Count > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<ul>");

                foreach (string s in errors)
                    sb.AppendFormat("<li>{0}</li>", s);

                sb.Append("</ul>");

                return sb.ToString();
            }

            return null;
        }

        public static bool IncurredErrors(System.Collections.Generic.List<string> errors, System.Web.UI.WebControls.CustomValidator validator)
        {
            if (errors.Count > 0)
            {
                if (validator != null)
                {
                    validator.IsValid = false;
                    validator.ErrorMessage = Utils.Validation.DisplayValidationErrors(errors);
                }

                errors.Clear();

                return true;
            }

            return false;
        }
        public static bool IncurredErrors(System.Collections.Generic.List<string> errors, System.Web.UI.WebControls.Label lbl)
        {
            if (errors.Count > 0)
            {
                lbl.Text = Utils.Validation.DisplayValidationErrors(errors);
                lbl.Visible = true;

                errors.Clear();

                return true;
            }

            return false;
        }

        #region Regexes

        //public static Regex regexFileName = new Regex(@"[A-Z0-9_]+([-+.][A-Z0-9_]+)*", RegexOptions.IgnoreCase);

        public static Regex regexUrl = new Regex(@"(http(s)?://)?((([\w-]+\.)+[\w-]+)|localhost)(/[\w- ./?%&=()\'\\]*)?", RegexOptions.IgnoreCase);
        //public static Regex regexUrl = new Regex(@"(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=()\'\\]*)?", RegexOptions.IgnoreCase);

        public static Regex regexEmail = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
        public static Regex regexPassword = new Regex(@"[\*\-_\w!]{5,20}", RegexOptions.IgnoreCase);
        public static Regex regexCC = new Regex(@"^\d{4,20}$", RegexOptions.IgnoreCase);

        public static Regex regexInts = new Regex(@"^(-)?\d+$", RegexOptions.IgnoreCase);
        public static Regex regexDecimals = new Regex(@"^(-)?\d+(\.\d\d)?$", RegexOptions.IgnoreCase);

        public static Regex regexValidImageFile = new Regex("^([A-Z0-9_]+([-+.][A-Z0-9_]+)*){0,254}.(gif|jpg|jpeg|png)$", RegexOptions.IgnoreCase);
        public static Regex regexValidFileNameOnly = new Regex("^([A-Z0-9_]+([-+.][A-Z0-9_]+)*){0,254}", RegexOptions.IgnoreCase); 
        //public static Regex regexValidImageFile = new Regex("^([^\\\\./:\\*\\\\?\\\"\\'<>\\|]{1}[^\\/:\\*\\?\\\"\\'<>\\|]{0,254})+\\.(gif|jpg|jpeg|png)$", RegexOptions.IgnoreCase);
        public static Regex regexValidMusicFile = new Regex(@"^([A-Z0-9_\s]+([-+.][A-Z0-9_\s]+)*){0,254}.(mp3)$", RegexOptions.IgnoreCase);
        //public static Regex regexGUID = new Regex("^[A-F0-9]{32}$|^({|\\()?[A-F0-9]{8}-([A-F0-9]{4}-){3}[A-F0-9]{12}(}|\\))?$|({)?[0xA-F0-9]{3,10}(, {0,1}[0xA-F0-9]{3,6}){2}, {0,1}({)([0xA-F0-9]{3,4}, {0,1}){7}[0xA-F0-9]{3,4}(}})$", 
        //    RegexOptions.IgnoreCase);

        //public bool GuidTryParse(string s, out Guid result)
        //{
        //    bool parseResult = false;
        //    result = Guid.Empty;
        //    if (!string.IsNullOrEmpty(s))
        //    {
        //        Match match = regexGUID.Match(s);
        //        if (match.Success)
        //        {
        //            result = new Guid(s);
        //            parseResult = true;
        //        }
        //    }
        //    return parseResult;
        //}

        //public static Regex regexValidMusicFile = new Regex("^([^\\\\./:\\*\\\\?\\\"\\'<>\\|]{1}[^\\/:\\*\\?\\\"\\'<>\\|]{0,254})+\\.(mp3)$", RegexOptions.IgnoreCase);
        //public static Regex regexHtmlStructureTags = new Regex(@"<(/)!doctype|<(/)?html|<(/)?head|<(/)?body|<(/)?title|<(/)?link|<(/)?style|<(/)?meta", RegexOptions.IgnoreCase);
        public static Regex regexHtmlStructureTags = new Regex(@"<(/)!doctype|<(/)?html|<(/)?head|<(/)?body|<(/)?title|<(/)?link|<(/)?meta", RegexOptions.IgnoreCase);

        #endregion

        public static bool DatesSpanMoreThanOneDay(List<DateTime> dates)
        {
            if(dates.Count > 1)
                dates.Sort();

            //this just says - if the lastdate is greater than the first
            //there is no exhaustive calculation being done here
            return (dates[dates.Count - 1].Date >= dates[0].AddDays(1).Date);
        }
        /// <summary>
        /// Returns TimeSpan of minvalue if not valid
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static TimeSpan IsValidTimeInput(string time)
        {
            string parsed = time.Trim();//.Replace(":", string.Empty);

            //if there is no : - than we assume in hours
            if (parsed.IndexOf(":") == -1 && parsed.Length > 0)
            {
                if (Utils.Validation.IsInteger(time))
                {
                    int hour = int.Parse(time);
                    if (hour > 11) hour -= 12;
                    return TimeSpan.FromHours(double.Parse(hour.ToString()));//keep on 12 hour time
                }
            }
            else if (parsed.IndexOf(":") != -1 && parsed.Length > 3)
            {
                string[] comps = time.Split(':');
                string hour = comps[0];
                string minute = comps[1];
                //we need an integer on the front end
                //we also need an integer less than 59 on the backend
                if (Utils.Validation.IsInteger(hour) && Utils.Validation.IsInteger(minute) && int.Parse(minute) < 60)
                {
                    int hora = int.Parse(hour);
                    if (hora > 11) hora -= 12;
                    //keep on 12 hour time
                    return TimeSpan.FromHours(double.Parse(hora.ToString()))
                        .Add(TimeSpan.FromMinutes(double.Parse(minute)));
                }
            }

            return TimeSpan.MinValue;
        }
        public static bool IsValidGuid(string guidString)
        {
            try
            {
                Guid guid = new Guid(guidString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidDateFormat(string date)
        {
            try
            {
                DateTime d = DateTime.Parse(date);
                string[] dateParts = date.Split('/');

                if (dateParts.Length != 3) return false;

                if (d.Month != int.Parse(dateParts[0].ToString()) && d.Day != int.Parse(dateParts[1].ToString()) &&
                    d.Year != int.Parse(dateParts[2].ToString())
                    )
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool Validate12HourTime(string sTime)
        {
            //it would be caught anyway - but to avoid unnecessary looping
            if (sTime.Replace(":", "").Length == 0) return false;

            //also to cut down on unnecessary looping
            sTime.TrimEnd(':');
            string[] splits = sTime.Split(':');

            //make sure parts of time are valid
            if (splits.Length > 3) return false;
            else if (splits.Length > 0)
            {
                foreach (string s in splits)
                    if (s.Length > 0 && !Validation.IsInteger(s)) return false;

                if (int.Parse(splits[0]) > 12) return false;
                //if(int.Parse(splits[1]) > 60) return false;				
            }

            try
            {
                DateTime.Parse(sTime);
                return true;
            }
            catch (Exception)
            {
                try
                {
                    DateTime.Parse(sTime + ":00");
                    return true;
                }
                catch (Exception) { }
            }

            return false;
        }

        /// <summary>
        /// verifies that a password conforms to password rules
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static bool VerifyPassword(string pass)
        {
            if (pass == null || pass.Trim().Length == 0) return false;

            Match mPass = regexPassword.Match(pass);

            if (mPass.Length < pass.Length) return false;

            return true;
        }


        public static bool ContainsHtmlStructureTags(string txt)
        {
            if (txt == null || txt.Trim().Length == 0) return false;

            Match mStructure = regexHtmlStructureTags.Match(txt);

            return (mStructure.Captures.Count > 0);//true if there are any matches
        }

        public static bool IsValidEmail(string eml)
        {   
            if (eml == null || eml.Trim().Length == 0) return false;

            Match mEmail = regexEmail.Match(eml);

            if (mEmail.Length < eml.Length) return false;

            return true;
        }
        public static bool IsValidFileNameOnly(string fileNameOnlyNoExtensionNoDot)
        {
            if (fileNameOnlyNoExtensionNoDot == null || fileNameOnlyNoExtensionNoDot.Trim().Length == 0) return false;

            Match mFileName = regexValidFileNameOnly.Match(fileNameOnlyNoExtensionNoDot);

            if (mFileName.Length < fileNameOnlyNoExtensionNoDot.Length) return false;

            return true;
        }
        public static bool IsValidImageFile(string fileName)
        {
            if (fileName == null || fileName.Trim().Length == 0) return false;

            Match mFileName = regexValidImageFile.Match(fileName);

            if (mFileName.Length < fileName.Length) return false;

            return true;
        }
        public static bool IsValidMusicFile(string fileName)
        {
            if (fileName == null || fileName.Trim().Length == 0) return false;

            Match mFileName = regexValidMusicFile.Match(fileName);

            if (mFileName.Length < fileName.Length) return false;

            return true;
        }

        /// <summary>
        /// Use in conjunction with FormatUrlFromString
        /// </summary>
        /// <param name="url"></param>
        /// <param name="securePageRequired"></param>
        /// <returns></returns>
        public static bool IsValidUrl(string url)
        {
            return IsValidUrl(url, false);
        }
        public static bool IsValidUrl(string url, bool securePageRequired)
        {
            if (url == null)
                return false;

            url = System.Web.HttpUtility.UrlDecode(url);
            
            if (url == null || url.Trim().Length == 0) return false;

            url = Utils.ParseHelper.FormatUrlFromString(url, (url.ToLower().StartsWith("http:")), securePageRequired);

            Match mUrl = regexUrl.Match(url);

            if (mUrl.Length < url.Length) return false;

            return true;
        }

        public static List<string> ValidArrayOfEmails(List<string> emailList)
        {
            List<string> valids = new List<string>();

            foreach (string s in emailList)
                if (IsValidEmail(s)) valids.Add(s);

            return valids;
        }

        /// <summary>
        /// Searches the given list for invalid emails and returns in a list. If specified, location information will also return, 
        /// within the line, information on the index of the invalid occurrence
        /// </summary>
        /// <param name="emailList"></param>
        /// <returns></returns>
        public static List<string> InvalidArrayOfEmails(List<string> emailList)
        {
            return InvalidArrayOfEmails(emailList, false);
        }
        public static List<string> InvalidArrayOfEmails(List<string> emailList, bool provideLocationInformation)
        {
            List<string> invalids = new List<string>();
            
            for (int i = 0; i < emailList.Count; i++)
            {
                string s = emailList[i];
                if (!IsValidEmail(s))
                    invalids.Add(string.Format("{0}{1}", (provideLocationInformation) ? string.Format("line({0}) ", i + 1) : string.Empty, s));

            }

            //foreach (string s in emailList)
            //    if (!VerifyEmail(s)) invalids.Add(s);

            return invalids;
        }

        public static bool IsInteger(string input)
        {
            try
            {
                int x = int.Parse(input);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public static bool IsValBoolean(string inputNum)
        {
            try
            {
                bool x = (inputNum == "1");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public static bool IsBoolean(string input)
        {
            try
            {
                bool x = bool.Parse(input);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool IsDecimal(string input)
        {
            try
            {
                decimal x = decimal.Parse(input);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool IsDate(string input)
        {
            try
            {
                DateTime x = DateTime.Parse(input);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
