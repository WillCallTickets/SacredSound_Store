using System;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Utils
{
	public class ListFiller
	{
        private static List<ListItem> stateProvinces = new List<ListItem>();

        public static List<ListItem> StateProvinces
        {
            get
            {
                if (stateProvinces == null || stateProvinces.Count == 00)
                {   
                    stateProvinces.Add(new ListItem("Alaska", "AK"));
                    stateProvinces.Add(new ListItem("Alabama", "AL"));
                    stateProvinces.Add(new ListItem("Arkansas", "AR"));
                    stateProvinces.Add(new ListItem("Arizona", "AZ"));
                    stateProvinces.Add(new ListItem("California", "CA"));
                    stateProvinces.Add(new ListItem("Colorado", "CO"));
                    stateProvinces.Add(new ListItem("Connecticut", "CT"));
                    stateProvinces.Add(new ListItem("D.C.", "DC"));
                    stateProvinces.Add(new ListItem("Delaware", "DE"));
                    stateProvinces.Add(new ListItem("Florida", "FL"));
                    stateProvinces.Add(new ListItem("Georgia", "GA"));
                    stateProvinces.Add(new ListItem("Hawaii", "HI"));
                    stateProvinces.Add(new ListItem("Idaho", "ID"));
                    stateProvinces.Add(new ListItem("Illinois", "IL"));
                    stateProvinces.Add(new ListItem("Indiana", "IN"));
                    stateProvinces.Add(new ListItem("Iowa", "IA"));
                    stateProvinces.Add(new ListItem("Kansas", "KS"));
                    stateProvinces.Add(new ListItem("Kentucky", "KY"));
                    stateProvinces.Add(new ListItem("Louisiana", "LA"));
                    stateProvinces.Add(new ListItem("Massachusetts", "MA"));
                    stateProvinces.Add(new ListItem("Maryland", "MD"));
                    stateProvinces.Add(new ListItem("Maine", "ME"));
                    stateProvinces.Add(new ListItem("Michigan", "MI"));
                    stateProvinces.Add(new ListItem("Minnesota", "MN"));
                    stateProvinces.Add(new ListItem("Missouri", "MO"));
                    stateProvinces.Add(new ListItem("Mississippi", "MS"));
                    stateProvinces.Add(new ListItem("Montana", "MT"));
                    stateProvinces.Add(new ListItem("North Carolina", "NC"));
                    stateProvinces.Add(new ListItem("North Dakota", "ND"));
                    stateProvinces.Add(new ListItem("Nebraska", "NE"));
                    stateProvinces.Add(new ListItem("New Hampshire", "NH"));
                    stateProvinces.Add(new ListItem("New Jersey", "NJ"));
                    stateProvinces.Add(new ListItem("New Mexico", "NM"));
                    stateProvinces.Add(new ListItem("Nevada", "NV"));
                    stateProvinces.Add(new ListItem("New York", "NY"));
                    stateProvinces.Add(new ListItem("Ohio", "OH"));
                    stateProvinces.Add(new ListItem("Oklahoma", "OK"));
                    stateProvinces.Add(new ListItem("Oregon", "OR"));
                    stateProvinces.Add(new ListItem("Pennsylvania", "PA"));
                    stateProvinces.Add(new ListItem("Rhode Island", "RI"));
                    stateProvinces.Add(new ListItem("South Carolina", "SC"));
                    stateProvinces.Add(new ListItem("South Dakota", "SD"));
                    stateProvinces.Add(new ListItem("Tennessee", "TN"));
                    stateProvinces.Add(new ListItem("Texas", "TX"));
                    stateProvinces.Add(new ListItem("Utah", "UT"));
                    stateProvinces.Add(new ListItem("Virgina", "VA"));
                    stateProvinces.Add(new ListItem("VI", "VI"));
                    stateProvinces.Add(new ListItem("Vermont", "VT"));
                    stateProvinces.Add(new ListItem("Washington", "WA"));
                    stateProvinces.Add(new ListItem("Wisconsin", "WI"));
                    stateProvinces.Add(new ListItem("West Virgina", "WV"));
                    stateProvinces.Add(new ListItem("Wyoming", "WY"));

                    stateProvinces.Add(new ListItem("Alberta", "AB"));
                    stateProvinces.Add(new ListItem("British Columbia", "BC"));
                    stateProvinces.Add(new ListItem("Manitoba", "MB"));
                    stateProvinces.Add(new ListItem("New Brunswick", "NB"));
                    stateProvinces.Add(new ListItem("Newfoundland and Labrador", "NF"));
                    stateProvinces.Add(new ListItem("Nova Scotia", "NS"));
                    stateProvinces.Add(new ListItem("Northwest Territories", "NT"));
                    stateProvinces.Add(new ListItem("Nunavut", "NU"));
                    stateProvinces.Add(new ListItem("Ontario", "ON"));
                    stateProvinces.Add(new ListItem("Price Edward Is.", "PE"));
                    stateProvinces.Add(new ListItem("Quebec", "PQ"));
                    stateProvinces.Add(new ListItem("Saskatchewan", "SK"));
                    stateProvinces.Add(new ListItem("Yukon Territory", "YK"));

                    stateProvinces.Add(new ListItem("Puerto Rico", "PR"));
                    stateProvinces.Add(new ListItem("US Virgin Islands", "VI"));
                    /*
                    stateProvinces.Add(new ListItem("Canal Zone", "CZ"));
                    stateProvinces.Add(new ListItem("Guam", "GU"));
                    stateProvinces.Add(new ListItem("Military", "MY"));
                     */
                }

                return stateProvinces;
            }
        }

        private static List<ListItem> countries = new List<ListItem>();

        public static List<ListItem> CountryListing
        {
            get
            {
                if (countries == null || countries.Count == 00)
                {
                    countries.Add(new ListItem("United States", "US"));
                    countries.Add(new ListItem("Canada", "CA"));
                    countries.Add(new ListItem("United Kingdom", "GB"));
                    //countries.Add(new ListItem("Andorra", "AD"));
                    //countries.Add(new ListItem("United Arab Emirates", "AE"));
                    //countries.Add(new ListItem("Afghanistan", "AF"));
                    //countries.Add(new ListItem("Antigua & Barbuda", "AG"));
                    //countries.Add(new ListItem("Anguilla", "AI"));
                    //countries.Add(new ListItem("Albania", "AL"));
                    //countries.Add(new ListItem("Armenia", "AM"));
                    //countries.Add(new ListItem("Netherlands Antilles", "AN"));
                    //countries.Add(new ListItem("Angola", "AO"));
                    //countries.Add(new ListItem("Antarctica", "AQ"));
                    //countries.Add(new ListItem("Argentina", "AR"));
                    //countries.Add(new ListItem("American Samoa", "AS"));
                    //countries.Add(new ListItem("Austria", "AT"));
                    //countries.Add(new ListItem("Australia", "AU"));
                    //countries.Add(new ListItem("Aruba", "AW"));
                    //countries.Add(new ListItem("Azerbaijan", "AZ"));
                    //countries.Add(new ListItem("Bosnia and Herzegovina", "BA"));
                    //countries.Add(new ListItem("Barbados", "BB"));
                    //countries.Add(new ListItem("Bangladesh", "BD"));
                    //countries.Add(new ListItem("Belgium", "BE"));
                    //countries.Add(new ListItem("Burkina Faso", "BF"));
                    //countries.Add(new ListItem("Bulgaria", "BG"));
                    //countries.Add(new ListItem("Bahrain", "BH"));
                    //countries.Add(new ListItem("Burundi", "BI"));
                    //countries.Add(new ListItem("Benin", "BJ"));
                    //countries.Add(new ListItem("Bermuda", "BM"));
                    //countries.Add(new ListItem("Brunei Darussalam", "BN"));
                    //countries.Add(new ListItem("Bolivia", "BO"));
                    //countries.Add(new ListItem("Brazil", "BR"));
                    //countries.Add(new ListItem("Bahama", "BS"));
                    //countries.Add(new ListItem("Bhutan", "BT"));
                    //countries.Add(new ListItem("Bouvet Island", "BV"));
                    //countries.Add(new ListItem("Botswana", "BW"));
                    //countries.Add(new ListItem("Belarus", "BY"));
                    //countries.Add(new ListItem("Belize", "BZ"));
                    //countries.Add(new ListItem("Cocos (Keeling) Islands", "CC"));
                    //countries.Add(new ListItem("Central African Republic", "CF"));
                    //countries.Add(new ListItem("Congo", "CG"));
                    //countries.Add(new ListItem("Switzerland", "CH"));
                    //countries.Add(new ListItem("Cote D'ivoire - Ivory Coast", "CI"));
                    //countries.Add(new ListItem("Cook Islands", "CK"));
                    //countries.Add(new ListItem("Chile", "CL"));
                    //countries.Add(new ListItem("Cameroon", "CM"));
                    //countries.Add(new ListItem("China", "CN"));
                    //countries.Add(new ListItem("Colombia", "CO"));
                    //countries.Add(new ListItem("Costa Rica", "CR"));
                    //countries.Add(new ListItem("Cuba", "CU"));
                    //countries.Add(new ListItem("Cape Verde", "CV"));
                    //countries.Add(new ListItem("Christmas Island", "CX"));
                    //countries.Add(new ListItem("Cyprus", "CY"));
                    //countries.Add(new ListItem("Czech Republic", "CZ"));
                    //countries.Add(new ListItem("Germany", "DE"));
                    //countries.Add(new ListItem("Djibouti", "DJ"));
                    //countries.Add(new ListItem("Denmark", "DK"));
                    //countries.Add(new ListItem("Dominica", "DM"));
                    //countries.Add(new ListItem("Dominican Republic", "DO"));
                    //countries.Add(new ListItem("Algeria", "DZ"));
                    //countries.Add(new ListItem("Ecuador", "EC"));
                    //countries.Add(new ListItem("Estonia", "EE"));
                    //countries.Add(new ListItem("Egypt", "EG"));
                    //countries.Add(new ListItem("Western Sahara", "EH"));
                    //countries.Add(new ListItem("Eritrea", "ER"));
                    //countries.Add(new ListItem("Spain", "ES"));
                    //countries.Add(new ListItem("Ethiopia", "ET"));
                    //countries.Add(new ListItem("Finland", "FI"));
                    //countries.Add(new ListItem("Fiji", "FJ"));
                    //countries.Add(new ListItem("Falkland Islands (Malvinas)", "FK"));
                    //countries.Add(new ListItem("Micronesia", "FM"));
                    //countries.Add(new ListItem("Faroe Islands", "FO"));
                    //countries.Add(new ListItem("France", "FR"));
                    //countries.Add(new ListItem("France, Metropolitan", "FX"));
                    //countries.Add(new ListItem("Gabon", "GA"));
                    //countries.Add(new ListItem("Grenada", "GD"));
                    //countries.Add(new ListItem("Georgia", "GE"));
                    //countries.Add(new ListItem("French Guiana", "GF"));
                    //countries.Add(new ListItem("Ghana", "GH"));
                    //countries.Add(new ListItem("Gibraltar", "GI"));
                    //countries.Add(new ListItem("Greenland", "GL"));
                    //countries.Add(new ListItem("Gambia", "GM"));
                    //countries.Add(new ListItem("Guinea", "GN"));
                    //countries.Add(new ListItem("Guadeloupe", "GP"));
                    //countries.Add(new ListItem("Equatorial Guinea", "GQ"));
                    //countries.Add(new ListItem("Greece", "GR"));
                    //countries.Add(new ListItem("South Georgia and the South Sandwich Islands", "GS"));
                    //countries.Add(new ListItem("Guatemala", "GT"));
                    //countries.Add(new ListItem("Guam", "GU"));
                    //countries.Add(new ListItem("Guinea-Bissau", "GW"));
                    //countries.Add(new ListItem("Guyana", "GY"));
                    //countries.Add(new ListItem("Hong Kong", "HK"));
                    //countries.Add(new ListItem("Heard & McDonald Islands", "HM"));
                    //countries.Add(new ListItem("Honduras", "HN"));
                    //countries.Add(new ListItem("Croatia", "HR"));
                    //countries.Add(new ListItem("Haiti", "HT"));
                    //countries.Add(new ListItem("Hungary", "HU"));
                    //countries.Add(new ListItem("Indonesia", "ID"));
                    //countries.Add(new ListItem("Ireland", "IE"));
                    //countries.Add(new ListItem("Israel", "IL"));
                    //countries.Add(new ListItem("India", "IN"));
                    //countries.Add(new ListItem("British Indian Ocean Territory", "IO"));
                    //countries.Add(new ListItem("Iraq", "IQ"));
                    //countries.Add(new ListItem("Islamic Republic of Iran", "IR"));
                    //countries.Add(new ListItem("Iceland", "IS"));
                    //countries.Add(new ListItem("Italy", "IT"));
                    //countries.Add(new ListItem("Jamaica", "JM"));
                    //countries.Add(new ListItem("Jordan", "JO"));
                    countries.Add(new ListItem("Japan", "JP"));
                    //countries.Add(new ListItem("Kenya", "KE"));
                    //countries.Add(new ListItem("Kyrgyzstan", "KG"));
                    //countries.Add(new ListItem("Cambodia", "KH"));
                    //countries.Add(new ListItem("Kiribati", "KI"));
                    //countries.Add(new ListItem("Comoros", "KM"));
                    //countries.Add(new ListItem("St. Kitts and Nevis", "KN"));
                    //countries.Add(new ListItem("Korea, Democratic People's Republic of", "KP"));
                    //countries.Add(new ListItem("Korea, Republic of", "KR"));
                    //countries.Add(new ListItem("Kuwait", "KW"));
                    //countries.Add(new ListItem("Cayman Islands", "KY"));
                    //countries.Add(new ListItem("Kazakhstan", "KZ"));
                    //countries.Add(new ListItem("Lao People's Democratic Republic", "LA"));
                    //countries.Add(new ListItem("Lebanon", "LB"));
                    //countries.Add(new ListItem("Saint Lucia", "LC"));
                    //countries.Add(new ListItem("Liechtenstein", "LI"));
                    //countries.Add(new ListItem("Sri Lanka", "LK"));
                    //countries.Add(new ListItem("Liberia", "LR"));
                    //countries.Add(new ListItem("Lesotho", "LS"));
                    //countries.Add(new ListItem("Lithuania", "LT"));
                    //countries.Add(new ListItem("Luxembourg", "LU"));
                    //countries.Add(new ListItem("Latvia", "LV"));
                    //countries.Add(new ListItem("Libyan Arab Jamahiriya", "LY"));
                    //countries.Add(new ListItem("Morocco", "MA"));
                    //countries.Add(new ListItem("Monaco", "MC"));
                    //countries.Add(new ListItem("Moldova, Republic of", "MD"));
                    //countries.Add(new ListItem("Madagascar", "MG"));
                    //countries.Add(new ListItem("Marshall Islands", "MH"));
                    //countries.Add(new ListItem("Mali", "ML"));
                    //countries.Add(new ListItem("Mongolia", "MN"));
                    //countries.Add(new ListItem("Myanmar", "MM"));
                    //countries.Add(new ListItem("Macau", "MO"));
                    //countries.Add(new ListItem("Northern Mariana Islands", "MP"));
                    //countries.Add(new ListItem("Martinique", "MQ"));
                    //countries.Add(new ListItem("Mauritania", "MR"));
                    //countries.Add(new ListItem("Monserrat", "MS"));
                    //countries.Add(new ListItem("Malta", "MT"));
                    //countries.Add(new ListItem("Mauritius", "MU"));
                    //countries.Add(new ListItem("Maldives", "MV"));
                    //countries.Add(new ListItem("Malawi", "MW"));
                    //countries.Add(new ListItem("Mexico", "MX"));
                    //countries.Add(new ListItem("Malaysia", "MY"));
                    //countries.Add(new ListItem("Mozambique", "MZ"));
                    //countries.Add(new ListItem("Namibia", "NA"));
                    //countries.Add(new ListItem("New Caledonia", "NC"));
                    //countries.Add(new ListItem("Niger", "NE"));
                    //countries.Add(new ListItem("Norfolk Island", "NF"));
                    //countries.Add(new ListItem("Nigeria", "NG"));
                    //countries.Add(new ListItem("Nicaragua", "NI"));
                    //countries.Add(new ListItem("Netherlands", "NL"));
                    //countries.Add(new ListItem("Norway", "NO"));
                    //countries.Add(new ListItem("Nepal", "NP"));
                    //countries.Add(new ListItem("Nauru", "NR"));
                    //countries.Add(new ListItem("Niue", "NU"));
                    //countries.Add(new ListItem("New Zealand", "NZ"));
                    //countries.Add(new ListItem("Oman", "OM"));
                    //countries.Add(new ListItem("Panama", "PA"));
                    //countries.Add(new ListItem("Peru", "PE"));
                    //countries.Add(new ListItem("French Polynesia", "PF"));
                    //countries.Add(new ListItem("Papua New Guinea", "PG"));
                    //countries.Add(new ListItem("Philippines", "PH"));
                    //countries.Add(new ListItem("Pakistan", "PK"));
                    //countries.Add(new ListItem("Poland", "PL"));
                    //countries.Add(new ListItem("St. Pierre & Miquelon", "PM"));
                    //countries.Add(new ListItem("Pitcairn", "PN"));
                    //countries.Add(new ListItem("Puerto Rico", "PR"));
                    //countries.Add(new ListItem("Portugal", "PT"));
                    //countries.Add(new ListItem("Palau", "PW"));
                    //countries.Add(new ListItem("Paraguay", "PY"));
                    //countries.Add(new ListItem("Qatar", "QA"));
                    //countries.Add(new ListItem("Reunion", "RE"));
                    //countries.Add(new ListItem("Romania", "RO"));
                    //countries.Add(new ListItem("Russian Federation", "RU"));
                    //countries.Add(new ListItem("Rwanda", "RW"));
                    //countries.Add(new ListItem("Saudi Arabia", "SA"));
                    //countries.Add(new ListItem("Solomon Islands", "SB"));
                    //countries.Add(new ListItem("Seychelles", "SC"));
                    //countries.Add(new ListItem("Sudan", "SD"));
                    //countries.Add(new ListItem("Sweden", "SE"));
                    //countries.Add(new ListItem("Singapore", "SG"));
                    //countries.Add(new ListItem("St. Helena", "SH"));
                    //countries.Add(new ListItem("Slovenia", "SI"));
                    //countries.Add(new ListItem("Svalbard & Jan Mayen Islands", "SJ"));
                    //countries.Add(new ListItem("Slovakia", "SK"));
                    //countries.Add(new ListItem("Sierra Leone", "SL"));
                    //countries.Add(new ListItem("San Marino", "SM"));
                    //countries.Add(new ListItem("Senegal", "SN"));
                    //countries.Add(new ListItem("Somalia", "SO"));
                    //countries.Add(new ListItem("Suriname", "SR"));
                    //countries.Add(new ListItem("Sao Tome & Principe", "ST"));
                    //countries.Add(new ListItem("El Salvador", "SV"));
                    //countries.Add(new ListItem("Syrian Arab Republic", "SY"));
                    //countries.Add(new ListItem("Swaziland", "SZ"));
                    //countries.Add(new ListItem("Turks & Caicos Islands", "TC"));
                    //countries.Add(new ListItem("Chad", "TD"));
                    //countries.Add(new ListItem("French Southern Territories", "TF"));
                    //countries.Add(new ListItem("Togo", "TG"));
                    //countries.Add(new ListItem("Thailand", "TH"));
                    //countries.Add(new ListItem("Tajikistan", "TJ"));
                    //countries.Add(new ListItem("Tokelau", "TK"));
                    //countries.Add(new ListItem("Turkmenistan", "TM"));
                    //countries.Add(new ListItem("Tunisia", "TN"));
                    //countries.Add(new ListItem("Tonga", "TO"));
                    //countries.Add(new ListItem("East Timor", "TP"));
                    //countries.Add(new ListItem("Turkey", "TR"));
                    //countries.Add(new ListItem("Trinidad & Tobago", "TT"));
                    //countries.Add(new ListItem("Tuvalu", "TV"));
                    //countries.Add(new ListItem("Taiwan, Province of China", "TW"));
                    //countries.Add(new ListItem("Tanzania, United Republic of", "TZ"));
                    //countries.Add(new ListItem("Ukraine", "UA"));
                    //countries.Add(new ListItem("Uganda", "UG"));
                    //countries.Add(new ListItem("United States Minor Outlying Islands", "UM"));
                    //countries.Add(new ListItem("Uruguay", "UY"));
                    //countries.Add(new ListItem("Uzbekistan", "UZ"));
                    //countries.Add(new ListItem("Vatican City State (Holy See)", "VA"));
                    //countries.Add(new ListItem("St. Vincent & the Grenadines", "VC"));
                    //countries.Add(new ListItem("Venezuela", "VE"));
                    //countries.Add(new ListItem("British Virgin Islands", "VG"));
                    //countries.Add(new ListItem("United States Virgin Islands", "VI"));
                    //countries.Add(new ListItem("Viet Nam", "VN"));
                    //countries.Add(new ListItem("Vanuatu", "VU"));
                    //countries.Add(new ListItem("Wallis & Futuna Islands", "WF"));
                    //countries.Add(new ListItem("Samoa", "WS"));
                    //countries.Add(new ListItem("Yemen", "YE"));
                    //countries.Add(new ListItem("Mayotte", "YT"));
                    //countries.Add(new ListItem("Yugoslavia", "YU"));
                    //countries.Add(new ListItem("South Africa", "ZA"));
                    //countries.Add(new ListItem("Zambia", "ZM"));
                    //countries.Add(new ListItem("Zaire", "ZR"));
                    //countries.Add(new ListItem("Zimbabwe", "ZW"));
                }

                return countries;
            }
        }

		private static string[] _countries = new string[] { 
			"Afghanistan", "Albania", "Algeria", "American Samoa", "Andorra", 
			"Angola", "Anguilla", "Antarctica", "Antigua And Barbuda", "Argentina", 
			"Armenia", "Aruba", "Australia", "Austria", "Azerbaijan",
			"Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus",
			"Belgium", "Belize", "Benin", "Bermuda", "Bhutan",
			"Bolivia", "Bosnia Hercegovina", "Botswana", "Bouvet Island", "Brazil",
			"Brunei Darussalam", "Bulgaria", "Burkina Faso", "Burundi", "Byelorussian SSR",
			"Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands",
			"Central African Republic", "Chad", "Chile", "China", "Christmas Island",
			"Cocos (Keeling) Islands", "Colombia", "Comoros", "Congo", "Cook Islands",
			"Costa Rica", "Cote D'Ivoire", "Croatia", "Cuba", "Cyprus",
			"Czech Republic", "Czechoslovakia", "Denmark", "Djibouti", "Dominica",
			"Dominican Republic", "East Timor", "Ecuador", "Egypt", "El Salvador",
			"England", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia",
			"Falkland Islands", "Faroe Islands", "Fiji", "Finland", "France",
			"Gabon", "Gambia", "Georgia", "Germany", "Ghana",
			"Gibraltar", "Great Britain", "Greece", "Greenland", "Grenada",
			"Guadeloupe", "Guam", "Guatemela", "Guernsey", "Guiana",
			"Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Heard Islands",
			"Honduras", "Hong Kong", "Hungary", "Iceland", "India",
			"Indonesia", "Iran", "Iraq", "Ireland", "Isle Of Man",
			"Israel", "Italy", "Jamaica", "Japan", "Jersey",
			"Jordan", "Kazakhstan", "Kenya", "Kiribati", "Korea, South",
			"Korea, North", "Kuwait", "Kyrgyzstan", "Lao People's Dem. Rep.", "Latvia",
			"Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein",
			"Lithuania", "Luxembourg", "Macau", "Macedonia", "Madagascar",
			"Malawi", "Malaysia", "Maldives", "Mali", "Malta",
			"Mariana Islands", "Marshall Islands", "Martinique", "Mauritania", "Mauritius",
			"Mayotte", "Mexico", "Micronesia", "Moldova", "Monaco",
			"Mongolia", "Montserrat", "Morocco", "Mozambique", "Myanmar",
			"Namibia", "Nauru", "Nepal", "Netherlands", "Netherlands Antilles",
			"Neutral Zone", "New Caledonia", "New Zealand", "Nicaragua", "Niger",
			"Nigeria", "Niue", "Norfolk Island", "Northern Ireland", "Norway",
			"Oman", "Pakistan", "Palau", "Panama", "Papua New Guinea",
			"Paraguay", "Peru", "Philippines", "Pitcairn", "Poland",
			"Polynesia", "Portugal", "Puerto Rico", "Qatar", "Reunion",
			"Romania", "Russian Federation", "Rwanda", "Saint Helena", "Saint Kitts",
			"Saint Lucia", "Saint Pierre", "Saint Vincent", "Samoa", "San Marino",
			"Sao Tome and Principe", "Saudi Arabia", "Scotland", "Senegal", "Seychelles",
			"Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands",
			"Somalia", "South Africa", "South Georgia", "Spain", "Sri Lanka",
			"Sudan", "Suriname", "Svalbard", "Swaziland", "Sweden",
			"Switzerland", "Syrian Arab Republic", "Taiwan", "Tajikista", "Tanzania",
			"Thailand", "Togo", "Tokelau", "Tonga", "Trinidad and Tobago",
			"Tunisia", "Turkey", "Turkmenistan", "Turks and Caicos Islands", "Tuvalu",
			"Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States",
			"Uruguay", "Uzbekistan", "Vanuatu", "Vatican City State", "Venezuela",
			"Vietnam", "Virgin Islands", "Wales", "Western Sahara", "Yemen",
			"Yugoslavia", "Zaire", "Zambia", "Zimbabwe"};

		/// <summary>
		/// Returns an array with all countries
		/// </summary>     
		public static StringCollection GetCountries()
		{
			StringCollection countries = new StringCollection();
			countries.AddRange(_countries);
			return countries;
		}
		public static SortedList GetCountries(bool insertEmptySelectionItem)
		{
			SortedList countries = new SortedList();
			if (insertEmptySelectionItem)
				countries.Add("", "Please select one...");
			foreach (String country in _countries)
				countries.Add(country, country);
			return countries;
		}

		public static void Hours1To12(DropDownList ddl)
		{
			ddl.Items.Clear();
			for(int i=1;i<=12;i++)
				ddl.Items.Add(new ListItem(i.ToString()));
		}

		public static void MinutesOnTheQuarter(DropDownList ddl)
		{
			ddl.Items.Clear();
			ddl.Items.Add(new ListItem("00"));
			ddl.Items.Add(new ListItem("15"));
			ddl.Items.Add(new ListItem("30"));
			ddl.Items.Add(new ListItem("45"));
		}
		public static void AmPm(DropDownList ddl)
		{
			ddl.Items.Clear();
			ddl.Items.Add(new ListItem("AM"));
			ddl.Items.Add(new ListItem("PM"));
		}
	}
	
	
	/// <summary>
	/// Summary description for Helper.
	/// </summary>
	public class Helper
	{
		public Helper()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        /// <summary>
        /// This class mimics the STL pair class
        /// It basically allows a tuple to be represented
        /// as an object
        /// </summary>
        /// <note>This cass is very similar to the KeyValuePair struct.
        /// However, that class cannot be inherited from</note>
        public class Pair<T, U>
        {
            public Pair() { }

            public Pair(T first, U second)
            {
                m_first = first;
                m_second = second;
            }

            virtual public T First
            {
                get { return m_first; }
            }

            virtual public U Second
            {
                get { return m_second; }
            }

            #region [private]
            T m_first;
            U m_second;
            #endregion
        };


        /// <summary>
        /// This class mimics the STL pair class
        /// It basically allows a tuple to be represented
        /// as an object
        /// </summary>
        /// <note>This cass is very similar to the KeyValuePair struct.
        /// However, that class cannot be inherited from</note>
        public class Triplet<T, U, V>
        {
            public Triplet() { }

            public Triplet(T first, U second, V third)
            {
                m_first = first;
                m_second = second;
                m_third = third;
            }

            virtual public T First
            {
                get { return m_first; }
            }

            virtual public U Second
            {
                get { return m_second; }
            }

            virtual public V Third
            {
                get { return m_third; }
            }

            #region [private]
            T m_first;
            U m_second;
            V m_third;
            #endregion
        };
        public class Quartet<T, U, V, W>
        {
            public Quartet() { }

            public Quartet(T first, U second, V third, W fourth)
            {
                m_first = first;
                m_second = second;
                m_third = third;
                m_fourth = fourth;
            }

            virtual public T First
            {
                get { return m_first; }
            }

            virtual public U Second
            {
                get { return m_second; }
            }

            virtual public V Third
            {
                get { return m_third; }
            }

            virtual public W Fourth
            {
                get { return m_fourth; }
            }

            #region [private]
            T m_first;
            U m_second;
            V m_third;
            W m_fourth;
            #endregion
        };

        /// <summary>
        /// Adds the onfocus and onblur attributes to all input controls found in the specified parent,
        /// to change their apperance with the control has the focus
        /// </summary>
        public static void SetInputControlsHighlight(System.Web.UI.Control container, string className, bool onlyTextBoxes)
        {
            foreach (System.Web.UI.Control ctl in container.Controls)
            {
                if ((onlyTextBoxes && ctl is TextBox) || ctl is TextBox || ctl is DropDownList ||
                    ctl is ListBox || ctl is CheckBox || ctl is RadioButton ||
                    ctl is RadioButtonList || ctl is CheckBoxList)
                {
                    WebControl wctl = ctl as WebControl;
                    wctl.Attributes.Add("onfocus", string.Format("this.className = '{0}';", className));
                    wctl.Attributes.Add("onblur", "this.className = '';");
                }
                else
                {
                    if (ctl.Controls.Count > 0)
                        SetInputControlsHighlight(ctl, className, onlyTextBoxes);
                }
            }
        }

        /// <summary>
        /// so far, tested only for a 0 to whatever range. For 0 to 1, use values of 0,2
        /// </summary>
        /// <param name="rangeStart">starting number of the range</param>
        /// <param name="rangeCount">the number of elements that can be within the result set</param>
        /// <returns></returns>
        public static int GetRandomInRange(int rangeStart, int rangeCount)
        {
            long tix = System.DateTime.Now.Ticks;
            int ticks = (int)tix;

            System.Random RandNum = new System.Random(ticks);

            return RandNum.Next(rangeStart, rangeCount);
        }

        /// <summary>
        /// ex: converts 15 to .15
        /// </summary>
        public static decimal ConvertIntToPercent(int val)
        {
            return (decimal)(val * (.01));
        }

		public static string Parse_DateForTableInsert(DateTime date)
		{
			return date.ToString("MM/dd/yyyy hh:mmtt");
		}

		/// <summary>
		/// valid chars 0-9, A-Z, a-z
		/// </summary>
		/// <param name="str"></param>
		/// <param name="finalLength"></param>
		/// <returns></returns>
		public static string Parse_AlphaOnly(string str)
		{
			StringBuilder sb = new StringBuilder();
			foreach(char c in str)
			{
				int ascii = Helper.Asc(c.ToString());

				if((ascii >= 48 && ascii <= 57) || (ascii >= 65 && ascii <= 90) || (ascii >= 97 && ascii <= 122))
					sb.AppendFormat("{0}", c.ToString());
			}

			return sb.ToString().Trim();
		}

		/// <summary>
		/// takes a number (ascii) and returns char
		/// </summary>
		/// <param name="p_intByte"></param>
		/// <returns></returns>
		public static string Chr(int p_intByte)
		{
			if( (p_intByte < 0) || (p_intByte > 255) )
			{
				throw new ArgumentOutOfRangeException("p_intByte", p_intByte,
					"Must be between 0 and 255.");
			}
			byte[] bytBuffer = new byte[]{(byte) p_intByte};
			return Encoding.GetEncoding(1252).GetString(bytBuffer);
		}


		/// <summary>
		/// takes a single char and returns its ascii value
		/// </summary>
		/// <param name="p_strChar"></param>
		/// <returns></returns>
		public static int Asc(string p_strChar)
		{
			if( p_strChar.Length != 1 )
			{
				throw new ArgumentOutOfRangeException("p_strChar", p_strChar,
					"Must be a single character.");
			}
			char[] chrBuffer = {Convert.ToChar(p_strChar)};
			byte[] bytBuffer = Encoding.GetEncoding(1252).GetBytes(chrBuffer);
			return (int) bytBuffer[0];
		}
	}
}
