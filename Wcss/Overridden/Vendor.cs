using System;

namespace Wcss
{
    public partial class VendorCollection
    {
        public Vendor GetVendor_Online()
        {
            return this.GetList().Find(delegate(Vendor match) { return (match.Name.ToLower() == "online"); });
        }
        public Vendor GetVendor_BoxOffice()
        {
            return (Vendor)this.GetList().Find(delegate(Vendor match) { return (match.Name.ToLower() == "boxoffice"); });
        }
    }
    public partial class Vendor
    {

    }
}
