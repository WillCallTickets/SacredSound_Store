using System;
using System.Collections.Generic;
//using System.Linq;
using System.Data;
using System.Text;

namespace Wcss.QueryRow
{
    public class SimpleShowDateListing
    {
        private int _showDateId;
        private DateTime _dateOfShow;
        private string _showNamePart;

        public int ShowDateId { get { return _showDateId; } }
        public DateTime DateOfShow { get { return _dateOfShow; } }
        public string ShowNamePart { get { return _showNamePart; } }

        public SimpleShowDateListing(IDataReader dr) 
        {
            try
            {
                _showDateId = (int)dr.GetValue(dr.GetOrdinal("ShowDateId"));
                _dateOfShow = (DateTime)dr.GetValue(dr.GetOrdinal("DateOfShow"));
                _showNamePart = dr.GetValue(dr.GetOrdinal("ShowNamePart")).ToString();
            }
            catch(Exception ex)
            {
                _Error.LogException(ex);
            }
         }
    }

    public partial class Report_DatesTickets
    {
        //private int _instanceId;
        //public int InstanceId { get { return _instanceId; } }

        private List<SimpleShowDateListing> _simpleShowDateRecords = new List<SimpleShowDateListing>();
        public List<SimpleShowDateListing> SimpleShowDateRecords { get { return _simpleShowDateRecords; } set { _simpleShowDateRecords = value; } }

        private List<ShowDate> _showDateRecords = new List<ShowDate>();
        public List<ShowDate> ShowDateRecords { get { return _showDateRecords; } set { _showDateRecords = value; } }

        private List<ShowTicket> _ticketRecords = new List<ShowTicket>();
        public List<ShowTicket> TicketRecords { get { return _ticketRecords; } set { _ticketRecords = value; } }

        private int _selectedVenueId;
        public int SelectedVenueId { get { return _selectedVenueId; } set { _selectedVenueId = value; } }

        /// <summary>
        /// Returns all possible objects
        /// </summary>
        public static Report_DatesTickets GetShowDatesInRange(int selectedVenueId, 
            DateTime startDate, DateTime endDate, int startRowIndex, int maximumRows)
        {
            return new Report_DatesTickets(selectedVenueId, startDate, endDate, startRowIndex, maximumRows, true, true, true);
        }
        public static int GetShowDatesInRangeCount(int selectedVenueId, DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            int count = 0;

            using (IDataReader dr = SPs.TxGetShowDatesInRangeCount(_Config.APPLICATION_ID, selectedVenueId, 
                startDate.ToString("MM/dd/yyyy hh:mmtt"), endDate.ToString("MM/dd/yyyy hh:mmtt")).GetReader())
            {
                while (dr.Read())
                {
                    count = (int)dr.GetValue(0);
                }

                dr.Close();
            }

            return count;
        }
        //public Report_DatesTickets() { }
        public Report_DatesTickets(int selectedVenueId, DateTime startDate, DateTime endDate, int startRowIndex, int maximumRows,
            bool returnSimpleRow, bool returnShowDateRows, bool returnTicketRows)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            SimpleShowDateRecords.Clear();
            ShowDateRecords.Clear();
            TicketRecords.Clear();

            //fill invoices, items and tickets
            using (IDataReader dr = SPs.TxGetShowDatesInRange(_Config.APPLICATION_ID, _Config._Default_VenueName, 
                selectedVenueId, startDate.ToString("MM/dd/yyyy hh:mmtt"), endDate.ToString("MM/dd/yyyy hh:mmtt"), 
                startRowIndex, maximumRows, returnSimpleRow, returnShowDateRows, returnTicketRows).GetReader())
            {
                while (dr.Read())
                {
                    SimpleShowDateListing row = new SimpleShowDateListing(dr);
                    SimpleShowDateRecords.Add(row);
                }

                dr.NextResult();

                while (dr.Read())
                {
                    ShowDate row = new ShowDate();
                    row.Load(dr);
                    ShowDateRecords.Add(row);
                }

                dr.NextResult();

                while (dr.Read())
                {
                    ShowTicket row = new ShowTicket();
                    row.Load(dr);
                    TicketRecords.Add(row);
                }
            }
        }        
    }
}
