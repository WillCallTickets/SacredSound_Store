using System;
using System.Text.RegularExpressions;

namespace Utils
{
	/// <summary>
	/// Summary description for BarCode.
	/// </summary>
    public class Shipping
    {
        public static bool IsPoBoxAddress(string streetAddress)
        {
            //from http://regexlib.com/RETester.aspx?regexp_id=2245
            //
            //Regex regexPOB = new Regex(@"\b[P|p]?(OST|ost)?\.?\s*[O|o|0]?(ffice|FFICE)?\.?\s*[B|b][O|o|0]?[X|x]?\.?\s+[#]?(\d+)\b", RegexOptions.IgnoreCase);
            Regex regexPOB = new Regex(@"\bp?(ost)?\.?\s*[o|0]?(ffice)?\.?\s*b[o|0]?[x]?\.?\s+[#]?(\d+)\b", RegexOptions.IgnoreCase);

			if(streetAddress.Trim().Length == 0) return false;

			Match mPOB = regexPOB.Match(streetAddress);

			if (mPOB.Length < streetAddress.Length) return false;

			return true;
		}

        public static bool IsContinentalUsShipment(string country, string state)
        {
            country = country.ToLower().Trim();
            state = state.ToLower().Trim();

            return (country == "us" || country == "usa" || country == "united states") && (state != "hi" && state != "hawaii" && state != "ak" && state != "alaska");
        }
    }
}
