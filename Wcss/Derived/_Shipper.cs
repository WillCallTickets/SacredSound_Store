using System;

namespace Wcss
{
    public class _Shipper
    {
        /// <summary>
        /// Returns a date that is the next (soonest) calculated ship date to today's date. Returns the date.Date
        /// </summary>
        private static DateTime _nowShip = DateTime.MinValue;
        public static DateTime NowShip
        {
            get
            {
                if(_nowShip == DateTime.MinValue)
                    _nowShip = Wcss._Shipper.CalculateShipDate(DateTime.Now);

                return _nowShip;
            }
        }


        //public static DateTime NowShip = Wcss._Shipper.CalculateShipDate(DateTime.Now);//.Date is already taken care of

        private static DateTime AdjustShipDate(DateTime sourceDate)
        {
            //always ship the next day
            DateTime shipDate = sourceDate.AddDays(1);

            if (!_Config._Shipping_EstimatedDays_UseLimitedDays)
            {
                //shipDate m-f
                if (shipDate.DayOfWeek == DayOfWeek.Saturday)
                    return shipDate.AddDays(2);

                if (shipDate.DayOfWeek == DayOfWeek.Sunday)
                    return shipDate.AddDays(2);
            }
            else if (_Config._Shipping_EstimatedDays_TTh)
            {
                //ship dates are tues and thursday - actually mon,wed&fri but leeway is added in
                //if shipdate is thurs,friday,saturday,sunday,monday - then tues
                //if tues,wed then thurs
                switch (shipDate.DayOfWeek)
                {
                    case DayOfWeek.Thursday:
                        shipDate = shipDate.AddDays(5);
                        break;
                    case DayOfWeek.Friday:
                        shipDate = shipDate.AddDays(4);
                        break;
                    case DayOfWeek.Saturday:
                        shipDate = shipDate.AddDays(3);
                        break;
                    case DayOfWeek.Sunday:
                    case DayOfWeek.Tuesday:
                        shipDate = shipDate.AddDays(2);
                        break;
                    case DayOfWeek.Monday:
                    case DayOfWeek.Wednesday:
                        shipDate = shipDate.AddDays(1);
                        break;
                }
            }
            else
            {
                //if we are using MWF
                if (shipDate.DayOfWeek == DayOfWeek.Saturday)
                    return shipDate.AddDays(2);

                else if (shipDate.DayOfWeek == DayOfWeek.Sunday || shipDate.DayOfWeek == DayOfWeek.Tuesday || shipDate.DayOfWeek == DayOfWeek.Thursday)
                    return shipDate.AddDays(1);
            }

            return shipDate;
        }
        public static DateTime CalculateShipDate(DateTime sourceDate)
        {
            //TODO: never guarantee to ship the same day - allow this when things get going
            DateTime shipDate = AdjustShipDate(sourceDate);

            //if it is a holiday - then add a day
            //new years, presidents, lincolns, mlk, wasington, memorial, labor, 4th, xmas, thanksgiving, columbus
            if ((shipDate.DayOfYear == 1) || //new years day
                (shipDate.Month == 1 && shipDate.DayOfWeek == DayOfWeek.Monday && shipDate.Day >= 15 && shipDate.Day <= 21) || //mlk - 3rd mon in jan
                (shipDate.Month == 2 && shipDate.DayOfWeek == DayOfWeek.Monday && shipDate.Day >= 15 && shipDate.Day <= 21) || //washington(presidents) - 3rd mon in feb
                (shipDate.Month == 5 && shipDate.DayOfWeek == DayOfWeek.Monday && shipDate.Day >= 24 && shipDate.Day <= 30) || //memorial day
                (shipDate.Month == 7 && shipDate.DayOfWeek == DayOfWeek.Monday && shipDate.Day == 4) || //july 4
                (shipDate.Month == 9 && shipDate.DayOfWeek == DayOfWeek.Monday && shipDate.Day >= 1 && shipDate.Day <= 7) || //labor day
                (shipDate.Month == 10 && shipDate.DayOfWeek == DayOfWeek.Monday && shipDate.Day >= 8 && shipDate.Day <= 14) || //columbus day
                (shipDate.Month == 11 && shipDate.Day == 11) || //veterans
                (shipDate.Month == 11 && shipDate.DayOfWeek == DayOfWeek.Friday && shipDate.Day >= 23 && shipDate.Day <= 29)//thanksgiving - look for the friday after thanks giving

                )
                shipDate = AdjustShipDate(shipDate);//add another ship day

            else if (shipDate.Month == 12)
            {
                //if it is >= 21 && <= 25th and monday or wednes or friday
                //if it is the 25th - ship on jan 2
                if (shipDate.Day >= 21 && shipDate.Day <= 31)//we are guaranteed M W F from above
                    shipDate = AdjustShipDate(DateTime.Parse(string.Format("1/1/{0}", shipDate.Year + 1)));
            }

            return shipDate.Date;
        }
    }
}
