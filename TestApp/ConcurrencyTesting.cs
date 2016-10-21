using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using Wcss;

namespace TestApp
{
    public class ConcurrencyTesting
    {
        public static void StartTest()
        {

            //for (int i = 0; i < 10; i++)
            //{

            //    new Thread(HydrateCacheObject).Start();

            //}

        }

        private static void HydrateCacheObject()
        {

            try
            {
                _ContextBase ctx = new _ContextBase();

                foreach (var item in ctx.SaleShowDates_All)
                {

                    var laxyObject1 = item.ShowTicketRecords()[0].Allotment;

                    var laxyObject3 = item.ShowTicketRecords()[0].Sold;

                }

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex);

            }
        }
    }
}
