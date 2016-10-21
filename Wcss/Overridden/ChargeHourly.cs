using System;

namespace Wcss
{
    public partial class ChargeHourly
    {
        public DateTime DatePerformed
        {
            get { return this.DtPerformed; 
            
            
            
            }
            set { this.DtPerformed = value; }
        }

        public decimal Row_Total { get { return this.Hours * this.Rate + this.FlatRate; } }
    }
}

