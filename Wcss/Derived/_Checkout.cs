using System;
using System.Data;
using System.Text;
using System.Collections.Generic;

namespace Wcss
{
    public partial class _Checkout
    {
        public static int CreateInvoiceForFlow(int invoiceId, string invoiceKey, decimal chargeTotal, string marketingKey, Vendor vendor,
            string aspnetUserId, int customerId, int cashewId, string creator, string purchaseEmail)
        {
            StringBuilder sb = new StringBuilder();    

            sb.Append("BEGIN TRANSACTION ");

            if (invoiceId == 0)//if we dont have a current invoice
            {
                sb.Append("INSERT INTO Invoice([ApplicationId], [TVendorId],[UniqueId],[PurchaseEmail],[UserId],[CustomerId],[dtInvoiceDate],");
                sb.Append("[Creator],[OrderType],[mBalanceDue],[InvoiceStatus],[TCashewId],[MarketingKeys]) ");
                sb.Append("VALUES (@appId, @vendorId, @invoiceKey, @purchaseEmail, @aspnetUserId, @customerId, getDate(), ");
                sb.Append("@creator, 'Purchase', @amountDue, 'NotPaid', @cashewId, @mrktKey) ");
                sb.Append("SET @invoiceId = SCOPE_IDENTITY() ");
            }
            else//we only need to update the invoice - cartitems are the same - most likely a failed trans
            {
                // Ensure that invoice has latest total!!
                sb.Append("UPDATE Invoice SET [TCashewId] = @cashewId, [mBalanceDue] = @amountDue WHERE [Id] = @invoiceId ");
            }

            sb.Append("COMMIT TRANSACTION ");

            //this is all we want back
            sb.Append("SELECT @invoiceId RETURN ");

            //init the command object
            Wcss._DatabaseCommandHelper cmd = new Wcss._DatabaseCommandHelper(sb.ToString());

            //invoiceid must be first to later reference its value
            cmd.AddCmdParameter("vendorId", vendor.Id, DbType.Int32);
            cmd.AddCmdParameter("invoiceId", invoiceId, DbType.Int32);
            cmd.AddCmdParameter("invoiceKey", invoiceKey, DbType.String);
            cmd.AddCmdParameter("purchaseEmail", purchaseEmail, DbType.String);
            cmd.AddCmdParameter("aspnetUserId", aspnetUserId, DbType.String);
            cmd.AddCmdParameter("customerId", customerId, DbType.Int32);
            cmd.AddCmdParameter("appId", _Config.APPLICATION_ID, DbType.Guid);
            cmd.AddCmdParameter("creator", creator, DbType.String);
            cmd.AddCmdParameter("amountDue", chargeTotal, DbType.Decimal);
            cmd.AddCmdParameter("cashewId", cashewId, DbType.Int32);
            cmd.AddCmdParameter("mrktKey", marketingKey, DbType.String);

            return cmd.PerformQuery("CheckoutInvoiceAndItems");
        }

        public static int CheckoutBillShip(int invoiceId, string invoiceKey, string aspnetUserId, int customerId,
            string blCompanyName, string blFirstName, string blLastName,
            string blAddress1, string blAddress2, string blCity, string blState, string blPostalCode, string blCountry, string blPhone,
            bool sameAsBilling,
            string shpCompany, string shpFirstName, string shpLastName,
            string shpAddress1, string shpAddress2, string shpCity, string shpState, string shpPostalCode, string shpCountry, string shpPhone
            )
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("IF EXISTS (SELECT * FROM InvoiceBillShip WHERE [TInvoiceId] = @invoiceId) BEGIN ");
            sb.Append("UPDATE InvoiceBillShip ");
            sb.Append("SET [blCompany] = @blCompany,[blFirstName] = @blFirst,[blLastName] = @blLast,[blAddress1] = @blAddress1, ");
            sb.Append("[blAddress2] = @blAddress2,[blCity] = @blCity,[blStateProvince] = @blState,[blPostalCode] = @blZip, ");
            sb.Append("[blCountry] = @blCountry,[blPhone] = @blPhone,[bSameAsBilling] = @sameAsBilling, ");
            sb.Append("[CompanyName] = @company,[FirstName] = @first,[LastName] = @last,[Address1] = @address1,[Address2] = @address2, ");
            sb.Append("[City] = @city,[StateProvince] = @state,[PostalCode] = @zip,[Country] = @country,[Phone] = @phone ");
            sb.Append("WHERE [TInvoiceId] = @invoiceId ");
            sb.Append("END ");

            sb.Append("ELSE BEGIN ");
            sb.Append("INSERT INTO InvoiceBillShip([tInvoiceId],[UserId],[CustomerId],[blCompany],[blFirstName],[blLastName],[blAddress1],[blAddress2], ");
            sb.Append("[blCity],[blStateProvince],[blPostalCode],[blCountry],[blPhone],[bSameAsBilling], ");
            sb.Append("[CompanyName],[FirstName],[LastName],[Address1],[Address2],[City],[StateProvince],[PostalCode],[Country],[Phone]) ");
            sb.Append("VALUES (@invoiceId,@userId,@customerId,@blCompany,@blFirst,@blLast,@blAddress1,@blAddress2, ");
            sb.Append("@blCity,@blState,@blZip,@blCountry,@blPhone,@sameAsBilling, ");
            sb.Append("@company,@first,@last,@address1,@address2,@city,@state,@zip,@country,@phone) ");
            sb.Append("END SELECT 0 RETURN ");

            //init the command object
            Wcss._DatabaseCommandHelper cmd = new Wcss._DatabaseCommandHelper(sb.ToString());

            cmd.AddCmdParameter("invoiceId", invoiceId, DbType.Int32);
            cmd.AddCmdParameter("invoiceKey", invoiceKey, DbType.String);
            cmd.AddCmdParameter("userId", aspnetUserId, DbType.String);
            cmd.AddCmdParameter("customerId", customerId, DbType.Int32);
            //billing
            cmd.AddCmdParameter("blCompany", blCompanyName, DbType.String);
            cmd.AddCmdParameter("blFirst", blFirstName, DbType.String);
            cmd.AddCmdParameter("blLast", blLastName, DbType.String);
            cmd.AddCmdParameter("blAddress1", blAddress1, DbType.String);
            cmd.AddCmdParameter("blAddress2", blAddress2, DbType.String);
            cmd.AddCmdParameter("blCity", blCity, DbType.String);
            cmd.AddCmdParameter("blState", blState, DbType.String);
            cmd.AddCmdParameter("blZip", blPostalCode, DbType.String);
            cmd.AddCmdParameter("blCountry", blCountry, DbType.String);
            cmd.AddCmdParameter("blPhone", blPhone, DbType.String);
            //shipping
            cmd.AddCmdParameter("sameAsBilling", (sameAsBilling) ? 1 : 0, DbType.Boolean);
            cmd.AddCmdParameter("company", shpCompany ?? string.Empty, DbType.String);
            cmd.AddCmdParameter("first", shpFirstName, DbType.String);
            cmd.AddCmdParameter("last", shpLastName, DbType.String);
            cmd.AddCmdParameter("address1", shpAddress1, DbType.String);
            cmd.AddCmdParameter("address2", shpAddress2, DbType.String);
            cmd.AddCmdParameter("city", shpCity, DbType.String);
            cmd.AddCmdParameter("state", shpState.ToUpper(), DbType.String);
            cmd.AddCmdParameter("zip", shpPostalCode, DbType.String);
            cmd.AddCmdParameter("country", shpCountry.ToUpper(), DbType.String);
            cmd.AddCmdParameter("phone", shpPhone, DbType.String);

            return cmd.PerformQuery("CheckoutBillShip");
        }

        public static int CheckoutAuthNet(Invoice invoice, string aspnetUserId, int customerId, string nameOnCard, string ipAddress, decimal chargeTotal, 
            string authDesc, string invoiceProducts,
            string userName, 
            string blCompanyName, string blFirstName, string blLastName,
            string blAddress1, string blAddress2, string blCity, string blState, string blPostalCode, string blCountry, string blPhone,             
            string shpCompany, string shpFirstName, string shpLastName,
            string shpAddress1, string shpAddress2, string shpCity, string shpState, string shpPostalCode, string shpCountry, string shpPhone
            )
        {
            StringBuilder sb = new StringBuilder();

            //formulate billing and shipping address
            string billingAddie = string.Format("{0} {1}", blAddress1 ?? string.Empty, blAddress2 ?? string.Empty).Trim();
            billingAddie = (billingAddie.Length > 60) ? billingAddie.Substring(0, 50) : billingAddie;

            string shippingAddie = string.Format("{0} {1}", shpAddress1 ?? string.Empty, shpAddress2 ?? string.Empty).Trim();
            shippingAddie = (shippingAddie.Length > 60) ? shippingAddie.Substring(0, 50) : shippingAddie;


            sb.Append("DECLARE @authNetId int SET @authNetId = 0 ");
            sb.Append("  BEGIN TRANSACTION ");

            //SAVE SAME ORDER LIST AS AUTH DESCRIPTION 
            sb.AppendFormat("\r\nUPDATE [Invoice] SET [vcProducts] = '{0}' WHERE [Id] = @invoiceId \r\n ", invoiceProducts);

            sb.Append("INSERT INTO AuthorizeNet ([ApplicationId],[InvoiceNumber],[bAuthorized],[TInvoiceId],[UserId],[CustomerId], ");
            sb.Append("[Method],[TransactionType],[mAmount],[mTaxAmount],[Description],[iDupeSeconds], ");
            sb.Append("[Email],[FirstName],[LastName],[NameOnCard],[Company],[BillingAddress],[City],[State],[Zip], ");
            sb.Append("[Country],[Phone],[IpAddress],[ShipToFirstName],[ShipToLastName],[ShipToCompany], ");
            sb.Append("[ShipToAddress],[ShipToCity],[ShipToState],[ShipToZip],[ShipToCountry]) ");
            sb.Append("VALUES (@appId,@invoiceKey,@authd,@invoiceId,@userId,@customerId,@method,@type,@amountDue,@taxDue, ");
            sb.Append("@description,@dupes,@email,@blFirst,@blLast,@nameOnCard,@blCompany,@blAddress, ");
            sb.Append("@blCity,@blState,@blZip,@blCountry,@blPhone,@ip,@first,@last,@company,@address, ");
            sb.Append("@city,@state,@zip,@country) ");

            sb.Append("  COMMIT TRANSACTION ");
            sb.Append(" SET @authNetId = SCOPE_IDENTITY() ");
            sb.Append(" SELECT @authNetId ");

            //init the command object
            Wcss._DatabaseCommandHelper cmd = new Wcss._DatabaseCommandHelper(sb.ToString());
            
            cmd.AddCmdParameter("appId", invoice.ApplicationId, DbType.Guid);
            cmd.AddCmdParameter("invoiceId", invoice.Id, DbType.Int32);
            cmd.AddCmdParameter("invoiceKey", invoice.UniqueId, DbType.String);
            cmd.AddCmdParameter("userId", aspnetUserId, DbType.String);
            cmd.AddCmdParameter("customerId", customerId, DbType.Int32);
            cmd.AddCmdParameter("nameOnCard", nameOnCard, DbType.String);
            cmd.AddCmdParameter("ip", ipAddress, DbType.String);
            cmd.AddCmdParameter("authd", 0, DbType.Boolean);
            cmd.AddCmdParameter("taxDue", 0, DbType.Decimal);
            cmd.AddCmdParameter("method", "CC", DbType.String);
            cmd.AddCmdParameter("type", "auth_capture", DbType.String);
            cmd.AddCmdParameter("amountDue", chargeTotal, DbType.Decimal);
            //---------------------------------------------------------------------------------------------
            cmd.AddCmdParameter("description", System.Text.RegularExpressions.Regex.Replace(authDesc, @"\s+", " ").Trim(), DbType.String);
            cmd.AddCmdParameter("dupes", _Config._AuthorizeNetDuplicateSeconds, DbType.Int32);
            cmd.AddCmdParameter("email", userName, DbType.String);
            //billing
            cmd.AddCmdParameter("blCompany", blCompanyName, DbType.String);
            cmd.AddCmdParameter("blFirst", blFirstName, DbType.String);
            cmd.AddCmdParameter("blLast", blLastName, DbType.String);
            cmd.AddCmdParameter("blAddress", billingAddie, DbType.String);
            cmd.AddCmdParameter("blCity", blCity, DbType.String);
            cmd.AddCmdParameter("blState", blState, DbType.String);
            cmd.AddCmdParameter("blZip", blPostalCode, DbType.String);
            cmd.AddCmdParameter("blCountry", blCountry, DbType.String);
            cmd.AddCmdParameter("blPhone", blPhone, DbType.String);
            //shipping
            cmd.AddCmdParameter("company", shpCompany, DbType.String);
            cmd.AddCmdParameter("first", shpFirstName, DbType.String);
            cmd.AddCmdParameter("last", shpLastName, DbType.String);
            cmd.AddCmdParameter("address", shippingAddie, DbType.String);
            cmd.AddCmdParameter("city", shpCity, DbType.String);
            cmd.AddCmdParameter("state", shpState, DbType.String);
            cmd.AddCmdParameter("zip", shpPostalCode, DbType.String);
            cmd.AddCmdParameter("country", shpCountry, DbType.String);
            cmd.AddCmdParameter("phone", shpPhone, DbType.String);


            int authId = cmd.PerformQuery("CheckoutAuthNet");
            if (authId < 10000)
            {
                string error = string.Format("{2}{0}CheckoutAuth could not be completed {0}returnValue: {1}{0}invoiceId: {3}{0}nameOnCard: {4}{0}ip: {5}{0}",
                    Utils.Constants.NewLine, authId, DateTime.Now, invoice.Id, nameOnCard, ipAddress);

                _Error.LogException(new Exception(error));

                throw new Exception("Error ocurred during CheckoutAuthNet");
            }

            return authId;
        }

    }
}
