using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class EmailLetter
    {
        public static string ConvertEmailLetterName(_Enums.EmailLetterSiteTemplate template)
        {
            return template.ToString().Replace("_",".");
        }
    }
}
