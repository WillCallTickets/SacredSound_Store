using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using System.Data;
using SubSonic;

namespace Wcss
{
    class _Testing
    {
        /*
        public static void TestCode()
        {
            //string[] tableNames = { "Show", "ShowDate", "JShowPromoter", "Promoter", "ShowTicket", "JShowAct", "Act", "Venue" };
            //DataSet ds = SPs.TxViewOnSaleShows().GetDataSet();

            //do not fuck with the given ds.
            //DataSet dsClone = ds.Copy();

            //for (int i = 0; i < tableNames.Length; i++)
            //{
            //    dsClone.Tables[i].TableName = tableNames[i];
            //}

            //VenueCollection vs = new VenueCollection();
            //vs.Load(dsClone.Tables["Venue"]);

            //string f = "l";

            //Southwind.ProductCollection coll2 = new Southwind.ProductCollection().Load(tbl);

            ShowCollection shows = new ShowCollection();
            shows.Where("Name", SubSonic.Comparison.GreaterOrEquals, DateTime.Now.Date.ToString("yyyy/MM/dd"));
            shows.OrderByAsc("Name").Load();

            List<int> sids = new List<int>();
            ArrayList vids = new ArrayList();
            foreach (Show s in shows)
            {
                sids.Add(s.Id);
                if (!vids.Contains(s.TVenueId))
                    vids.Add(s.TVenueId);
            }

            IDataReader rdr = new Query("Venue").IN("Id", vids).ExecuteReader();
            VenueCollection vs = new VenueCollection();
            vs.Load(rdr);

            

            //string g = "l";
            rdr.Close();

            //for(int i=0;i<shows.Count;i++)
            //{
            //    shows[i].Venue = (Venue)vs.Find(shows[i].TVenueId);


            //    string f = "k";
            //}


            foreach (Show s in shows)
            {
                s.Venue = (Venue)vs.Find(s.TVenueId);


                
            }

            string ven = shows[0].VenueRecord.Name;

            //string f = "k";
            //Southwind.ProductCollection coll2 = new Southwind.ProductCollection().Load(tbl);
        }
         * */
    }

 
}
/*
 * 

//load up the product using a multi-return DataSet

//for performance, queue up the 4 SQL calls into one

string sql = "";

 

//Product Main Info

Query q = new Query("vwProduct");

q.AddWhere("productID", productID);

 

//append

sql = q.GetSql()"\r\n";

 

//Images

q = new Query(Commerce.Common.Image.GetTableSchema());

q.AddWhere("productID", productID);

q.OrderBy = OrderBy.Asc("listOrder");

//append

sql += q.GetSql() + "\r\n";

 

//Reviews

q = new Query(ProductReview.GetTableSchema());

q.AddWhere("productID", productID);

q.AddWhere("isApproved", 1);

 

//append

sql += q.GetSql() + "\r\n";

 

 

//Descriptors

q = new Query(ProductDescriptor.GetTableSchema());

q.AddWhere("productID", productID);

q.OrderBy = OrderBy.Asc("listOrder");

 

 

//append

sql += q.GetSql() + "\r\n";

 

QueryCommand cmd = new QueryCommand(sql);

cmd.AddParameter("@productID", productID,DbType.Int32);

cmd.AddParameter("@isApproved", true,DbType.Boolean);

DataSet ds = DataService.GetDataSet(cmd);

*/
/*
//be sure that table names match
               // string[] tableNames = { "Show", "ShowDate", "JShowPromoter", "Promoter", "ShowTicket", "JShowAct", "Act", "Venue" };
                //DataSet ds = SPs.TxViewOnSaleShows().GetDataSet();//.GetReader();

                // do not fuck with the given ds.
                //DataSet dsClone = ds.Copy();

                //for (int i = 0; i < tableNames.Length; i++)
                //{
                //    dsClone.Tables[i].TableName = orderedTableList[i];
                //}
                //dataSet.Merge(dsClone, true, MissingSchemaAction.Ignore);

                //todo: check for same amount of tables

                ////name the tables for easier reference
                //for (int i = 0; i < tableNames.Length; i++)
                //    ds.Tables[i].TableName = tableNames[i];

                //ShowCollection shows = new ShowCollection();
                //List<DataRow> listShow = new List<DataRow>();
                //foreach (DataRow dr in ds.Tables["Show"].Rows)
                //    listShow.Add(dr);

                //shows.AddRange(listShow);

                //foreach(DataRow dr in ds.Tables["Show"].Rows)
                //{
                //    Show s = new Show();
                //    s.Id = (int)dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("Id"));
                //    s.Name = (int)dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("Id"));
                //    s.dtAnnounceDate = (int)dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("Id"));
                //    s.dtDateOnSale = (int)dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("Id"));
                //    s.bActive = (int)dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("Id"));
                //    s.bDisplayOnWeb = (int)dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("Id"));
                //    s.bDisplayOnTicketing = (int)dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("Id"));

                //    shows.Add(s);
                //}
*/