using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Wcss
{
    public partial class _Interface
    {
        public class MercBundler
        {
            /*
            public interface IMerchBundler<T>
            {
                MerchBundleCollection MerchBundleRecords();
                void MerchBundleRecords_Reset();
                void MerchBundle_Add(MerchBundle bundle);
                void MerchBundle_Delete(MerchBundleShowTicket join);
            }

            public static MerchBundleCollection GetMerchBundleRecords<T>(T item)
            {
                MerchBundleShowTicketCollection join = new MerchBundleShowTicketCollection();
                MerchBundleCollection coll = new MerchBundleCollection();

                join.AddRange((MerchBundleShowTicketCollection)item.GetType()
                    .InvokeMember("MerchBundleShowTicketRecords", System.Reflection.BindingFlags.InvokeMethod, null, item, null));

                if (join != null && join.Count > 0)
                {
                    foreach (MerchBundleShowTicket joinTicket in join)
                    {
                        MerchBundle mb = new MerchBundle();
                        mb.CopyFrom(joinTicket.MerchBundleRecord);
                        coll.Add(mb);
                    }

                    if (coll.Count > 1)
                        coll.Sort("IDisplayOrder", true);
                }

                return coll;
            }

            public static void MerchBundle_Add<T>(T item, MerchBundle bundle)
            {
                MerchBundleShowTicket newItem = new MerchBundleShowTicket();
                newItem.DtStamp = DateTime.Now;
                newItem.TMerchBundleId = bundle.Id;

                //determine what to do based on type
                object t = item.GetType();
                switch(t.ToString())
                {
                    case "Wcss.Show":
                        newItem.TShowId = (int)Utils.Reflector.EvaluateExpression(item, "Id");
                        break;
                    case "Wcss.ShowDate":
                        newItem.TShowDateId = (int)Utils.Reflector.EvaluateExpression(item, "Id");
                        break;
                    case "Wcss.ShowTicket":
                        newItem.TShowTicketId = (int)Utils.Reflector.EvaluateExpression(item, "Id");
                        break;
                }

                newItem.Save();

                item.GetType()
                    .InvokeMember("MerchBundleRecords_Reset", System.Reflection.BindingFlags.InvokeMethod, null, item, null);
            }

            public static void MerchBundle_Delete<T>(T item, MerchBundleShowTicket join)
            {
                //get info from the merchbundleshowticket we are trying to delete
                int linkedMerchBundleId = join.TMerchBundleId;

                //delete the join row
                MerchBundleShowTicket.Delete(join.Id);

                //delete the bundle from the collection - have it reorder
                MerchBundleItemCollection coll = new MerchBundleItemCollection();
                coll.AddRange((MerchBundleItemCollection)item.GetType()
                    .InvokeMember("MerchBundleItemRecords", System.Reflection.BindingFlags.InvokeMethod, null, item, null));
                coll.DeleteFromCollection(linkedMerchBundleId);

                //reset the collection
                item.GetType()
                    .InvokeMember("MerchBundleRecords_Reset", System.Reflection.BindingFlags.InvokeMethod, null, item, null);
            }
             * 
             * */
        
        }
    }
}
