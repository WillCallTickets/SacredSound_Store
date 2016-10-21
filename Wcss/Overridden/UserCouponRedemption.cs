using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class UserCouponRedemption
    {
        [XmlAttribute("DateApplied")]
        public DateTime DateApplied
        {
            get { return (this.DtApplied.HasValue) ? this.DtApplied.Value : DateTime.MaxValue; }
            set { this.DtApplied = value; }
        }
        [XmlAttribute("DiscountAmount")]
        public decimal DiscountAmount
        {
            get { return this.MDiscountAmount; }
            set { this.MDiscountAmount = decimal.Round(value,2); }
        }
        [XmlAttribute("InvoiceAmount")]
        public decimal InvoiceAmount
        {
            get { return this.MInvoiceAmount; }
            set { this.MInvoiceAmount = decimal.Round(value, 2); }
        }
        public static string GetCouponBase(string wholeCoupon)
        {
            string baseCoupon = wholeCoupon;

            if (wholeCoupon.IndexOf('-') != -1)
            {
                string[] parts = wholeCoupon.Split('-');
                if (parts.Length == 2)
                    baseCoupon = parts[0];
            }

            return baseCoupon;
        }
        public static bool IsAllowedRedemption(string userName, string coupon, int maxUses)
        {
            if (maxUses == 0)
                return true;

            //we need to match on root of coupon
            string matchCoupon = coupon;
            if (coupon.IndexOf('-') != -1)
                matchCoupon = coupon.Substring(0, coupon.IndexOf('-'));

            string sql = "SELECT ISNULL(COUNT(cr.[Id]),0) as 'Summ' FROM [UserCouponRedemption] cr WHERE cr.[CodeRoot] = @match ";
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@match", matchCoupon);
            cmd.Parameters.Add("@userName", userName);

            try
            {
                int ret = (int)SubSonic.DataService.ExecuteScalar(cmd);

                //return true;

                if (ret < maxUses)
                    return true;
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }

            return false;
        }
        public static void RecordRedemption(string userName, int promoId, string couponCode, decimal discountAmount, decimal invoiceAmount)
        {
            string sql = "INSERT [UserCouponRedemption](dtApplied, UserId, TSalePromotionId, CouponCode, mDiscountAmount, mInvoiceAmount) ";
            sql += "SELECT @date, u.[UserId], @promoId, @coupon, @discountAmount, @invoiceAmount FROM [Aspnet_Users] u WHERE u.[LoweredUserName] = @userName ";
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@date", DateTime.Now, System.Data.DbType.DateTime);
            cmd.Parameters.Add("@promoId", promoId, System.Data.DbType.Int32);
            cmd.Parameters.Add("@coupon", couponCode);
            cmd.Parameters.Add("@userName", userName);
            cmd.Parameters.Add("@discountAmount", discountAmount, System.Data.DbType.Decimal);
            cmd.Parameters.Add("@invoiceAmount", invoiceAmount, System.Data.DbType.Decimal);

            try
            {
                SubSonic.DataService.ExecuteQuery(cmd);
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }
    }
}
