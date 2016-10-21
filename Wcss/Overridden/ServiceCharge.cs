using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class ServiceCharge
    {
        [XmlAttribute("MaxValue")]
        public decimal MaxValue
        {
            get { return decimal.Round(this.MMaxValue, 2); }
            set { this.MMaxValue = decimal.Round(value, 2); }
        }
        [XmlAttribute("Charge")]
        public decimal Charge
        {
            get { return decimal.Round(this.MCharge, 2); }
            set { this.MCharge = decimal.Round(value, 2); }
        }
        [XmlAttribute("Percentage")]
        public decimal Percentage
        {
            get { return this.MPercentage; }
            set { this.MPercentage = value; }
        }

        public static decimal ComputeTicketServiceFee(decimal unitPrice)
        {   
            decimal fee = 0;

            ServiceChargeCollection coll = new ServiceChargeCollection();
            coll.AddRange(Wcss._Lookits.ServiceCharges);            
            int count = coll.Count;

            if (count > 0)
            {   
                coll.Sort("MMaxValue", true);

                ServiceCharge serviceCharge = coll[count - 1];//assign default
                fee = serviceCharge.Charge;

                for (int i = 0; i < count - 1; i++)
                {
                    if (unitPrice <= coll[i].MaxValue)
                    {
                        serviceCharge = coll[i];
                        fee = serviceCharge.Charge;

                        //do we additionally charge a pct on the fee
                        if (serviceCharge != null && serviceCharge.Percentage > 0)//if (_Config._Service_Percentage > 0)
                        {
                            //ask the question - do we add the service fee onto the ticket price before COMPUTING the service percentage addition? Do we base on $10 or $12 (10 + 2 service)
                            if (_Config._Service_ApplyPercentageToTierFee)
                                unitPrice += fee;

                            //decimal computedFee = decimal.Round(unitPrice * _Config._Service_Percentage, 2);
                            decimal computedFee = decimal.Round(unitPrice * serviceCharge.Percentage, 2);

                            decimal roundup = _Config._Service_Percentage_Roundup;

                            if (roundup > 0)
                            {
                                //do rounding to nearest n 
                                computedFee = Math.Round(Math.Round((computedFee / roundup) + roundup, 0) * roundup, 2);
                            }

                            computedFee = decimal.Round(computedFee, 2);

                            fee += computedFee;
                        }

                        break;
                    }
                }
            }

            return fee;


            //ServiceCharge serviceCharge = (count > 0) ? coll[count-1] : null;

            //decimal fee = (serviceCharge != null) ? serviceCharge.Charge : 0;

            ////assign a default value
            //if(serviceCharge != null)
            //    fee = serviceCharge.Charge;

            ////compare unitprice to max value of the service tier - if the amount is less than the threshhold(maxValue)
            ////then assign the charge
            //for(int i=0; i<count-1;i++)
            //{
            //    if(unitPrice <= coll[i].MaxValue)
            //    {
            //        serviceCharge = coll[i];
            //        fee = serviceCharge.Charge;

            //        //do we additionally charge a pct on the fee
            //        if (serviceCharge != null && serviceCharge.Percentage > 0)//if (_Config._Service_Percentage > 0)
            //        {
            //            //ask the question - do we add the service fee onto the ticket price before COMPUTING the service percentage addition? Do we base on $10 or $12 (10 + 2 service)
            //            if (_Config._Service_ApplyPercentageToTierFee)
            //                unitPrice += fee;

            //            //decimal computedFee = decimal.Round(unitPrice * _Config._Service_Percentage, 2);
            //            decimal computedFee = decimal.Round(unitPrice * serviceCharge.Percentage, 2);

            //            decimal roundup = _Config._Service_Percentage_Roundup;

            //            if (roundup > 0)
            //            {
            //                //do rounding to nearest n 
            //                computedFee = Math.Round(Math.Round((computedFee / roundup) + roundup, 0) * roundup, 2);
            //            }
                            
            //            computedFee = decimal.Round(computedFee, 2);

            //            fee += computedFee;
            //        }
                    
            //        break;
            //    }
            //}            

            //return fee;
        }
    }
}

