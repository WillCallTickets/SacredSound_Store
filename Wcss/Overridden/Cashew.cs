using System;
using System.Web.Security;
using System.Xml.Serialization;
using SubSonic;

namespace Wcss
{
    public partial class Cashew 
    {
        /// <summary>
        /// Always create the cassius even if the CC is not required!!!!
        /// 
        /// Checks to see if there is an existing record of the card. If not found, it creates a new record. It 
        /// returns the values of Cashew Id, aspnet_Users Id and the customer Id for further use in the orderprocess
        /// Encrypts using the user id as the key - even if the username changes - the userid will not
        /// 
        /// Note that the fullCard number is never used past this method. Only the last four digits. All data passed 
        /// to SP is encrypted
        /// </summary>
        public static void CheckoutCashew(string userName, string nameOnCard, string fullCardNumber, string expiryMonth, 
            string expiryYear, ref string aspnetUserId, ref int customerId, ref int cashewId)
        {
            userName = userName.ToLower();

            //get membership
            MembershipUser mem = Membership.GetUser(userName);

            //get user id and assign to return var
            aspnetUserId = mem.ProviderUserKey.ToString();

            //encrypt on substring of user id 
            string encryptor = FormatEncryptor(aspnetUserId);

            string eName = Utils.Crypt.Encrypt(nameOnCard, encryptor);

            //if the card number is invalid then fill in info with expired data
            if(fullCardNumber.Trim().Length == 0)
                fullCardNumber = "";
            string eCardNum = Utils.Crypt.Encrypt(
                (fullCardNumber.Trim().Length == 0) ? "1111" : 
                fullCardNumber.Substring((fullCardNumber.Length - 4), 4), encryptor);
            string eMonth = Utils.Crypt.Encrypt((fullCardNumber.Trim().Length == 0) ? "1" : expiryMonth, encryptor);
            string eYear = Utils.Crypt.Encrypt((fullCardNumber.Trim().Length == 0) ? "1900" : expiryYear, encryptor);

            try
            {
                //do the stored proc and return the aspnet user id and the customer id
                using (System.Data.IDataReader reader = SPs.TxCheckoutCashew(aspnetUserId, eName, eCardNum, eMonth, eYear).GetReader())
                {
                    while (reader.Read())
                    {   
                        customerId = (int)reader["CustomerId"];
                        cashewId = (int)reader["CashewId"];

                        reader.NextResult();
                    }

                    reader.Close();
                }
            }
            catch (System.Data.SqlClient.SqlException sex)
            {
                _Error.LogException(sex);
                throw new Exception(string.Format("CheckoutCashew Sql Error.\r\n{0}\r\n{1}", sex.Message, sex.StackTrace));
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                throw new Exception(string.Format("CheckoutCashew Error.\r\n{0}\r\n{1}", ex.Message, ex.StackTrace));
            }

            //evaluate return vals
            if (aspnetUserId == null || customerId == 0 || cashewId == 0)
            {
                string error = string.Format("{0}Checkout failure at CASHEW{0}aspnetUserId: {1}{0}customerId: {2}{0}cashewId: {3}",
                    Utils.Constants.NewLine, (aspnetUserId == null) ? "null" : aspnetUserId, customerId, cashewId);
                
                _Error.LogException(new Exception(error));
            }
        }

        public static string FormatEncryptor(string userId) { return userId.Replace("-",string.Empty).Substring(0, 16).Trim(); }

        private string _encryptor = null;
        private string Encryptor { get { if (_encryptor == null) _encryptor = FormatEncryptor(this.UserId.ToString()); return _encryptor; } }

        [XmlAttribute("IsExpiryValid")]
        public bool IsExpiryValid
        {
            get
            {
                DateTime cardExpiry = DateTime.Parse(string.Format("{0}/1/{1}", this.Month, this.Year)).AddMonths(1);

                if (cardExpiry.Date < DateTime.Now.Date)//the less than puts us into the previous month
                    return false;

                return true;
            }
        }

        [XmlAttribute("LastFour")]
        public string LastFour
        {
            get
            {
                return this.Number;
            }
        }
        [XmlAttribute("Number")]
        public string Number
        {
            get 
            {
                //handle cards that have been processed and manually expired for PCI compliance
                if (this.ENumber == "-1")
                    return "-1";

                string num = string.Empty;

                try
                {
                    num = Utils.Crypt.Decrypt(this.ENumber, Encryptor);
                }
                catch(Exception ex)
                {
                    _Error.LogException(ex);
                    num = "-1";
                }

                if (num.Length > 4)
                    return num.Substring((num.Length - 4), 4);

                return num;
            }
            set
            {
                string lastFour;

                if (value.Length > 4)//make sure we are only storing the last four digits
                    lastFour = value.Substring((value.Length - 4), 4);
                else
                    lastFour = value;

                this.ENumber = Utils.Crypt.Encrypt(lastFour, Encryptor);
            }
        }
        [XmlAttribute("Month")]
        internal string Month
        {
            get
            {
                //handle cards that have been processed and manually expired for PCI compliance
                if (this.EMonth == "-1")
                    return "-1";

                try
                {
                    return Utils.Crypt.Decrypt(this.EMonth, Encryptor);
                }
                catch(Exception ex)
                {
                    _Error.LogException(ex);
                    return "-1";
                }
            }
            set
            {
                this.EMonth = Utils.Crypt.Encrypt(value, Encryptor);
            }
        }
        [XmlAttribute("Year")]
        internal string Year
        {
            get
            {
                //handle cards that have been processed and manually expired for PCI compliance
                if (this.EYear == "-1")
                    return "-1";

                try
                {
                    return Utils.Crypt.Decrypt(this.EYear, Encryptor);
                }
                catch(Exception ex)
                {
                    _Error.LogException(ex);
                    return "1900";
                }
            }
            set
            {
                this.EYear = Utils.Crypt.Encrypt(value, Encryptor);
            }
        }
        [XmlAttribute("Name")]
        public string Name
        {
            get
            {
                //handle cards that have been processed and manually expired for PCI compliance
                if (this.EName == "-1")
                    return "-1";

                return Utils.Crypt.Decrypt(this.EName, Encryptor);
            }
            set
            {
                this.EName = Utils.Crypt.Encrypt(value, Encryptor);
            }
        }
        [XmlAttribute("CassInfo")]
        public string CassInfo
        {
            get
            {
                return string.Format("{0}_{1}_{2}", this.Number, this.Month, this.Year).ToLower();
            }
        }
        [XmlAttribute("ExpiryDate_Effective")]
        public DateTime ExpiryDate_Effective
        {
            get
            {
                return DateTime.Parse(string.Format("{0}/1/{1}", this.Month, this.Year)).AddMonths(1).AddMinutes(-1);
            }
        }        
    }
}
