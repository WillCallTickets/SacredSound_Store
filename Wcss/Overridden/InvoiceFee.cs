using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class InvoiceFeeCollection
    {
        public bool DeleteFeeFromCollection(int idx)
        {
            InvoiceFee entity = (InvoiceFee)this.Find(idx);
            if (entity != null)
            {
                try
                {
                    InvoiceFee.Delete(idx);
                    //remove it from the collection
                    this.Remove(entity);
                    //this.SaveAll();
                    return true;
                }
                catch (Exception e)
                {
                    _Error.LogException(e);
                    throw;
                }
            }

            return false;
        }

        public InvoiceFee AddFeeToCollection(bool isOverride, string name, decimal price, string description)
        {
            InvoiceFee newItem = new InvoiceFee();
            newItem.ApplicationId = _Config.APPLICATION_ID;
            newItem.IsOverride = isOverride;
            newItem.Name = name;
            newItem.Description = description;
            newItem.Price = price;

            try
            {
                newItem.Save();
                this.Add(newItem);
                return newItem;
            }
            catch (Exception e)
            {
                _Error.LogException(e);
                throw;
            }
        }

    }

    public partial class InvoiceFee
    {
        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        }
        [XmlAttribute("IsOverride")]
        public bool IsOverride
        {
            get { return this.BOverride; }
            set { this.BOverride = value; }
        }
        [XmlAttribute("Price")]
        public decimal Price
        {
            get { return decimal.Round(this.MPrice,2); }
            set { this.MPrice = value; }
        }
    }
}
