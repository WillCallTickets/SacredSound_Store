using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Wcss;

namespace StoreApi.Models
{    
    public class EventGroup
    {
        public int Id { get; set; }
        public long dtStamp { get; set; }
        public string Name { get; set; }
        public string Venue { get; set; }
        public List<Event> EventList { get; set; }

        public EventGroup() { }

        public EventGroup(Show show)
        {
            Id = show.Id;
            dtStamp = show.DtStamp.Ticks;
            Name = show.Name;
            Venue = show.VenueRecord.Name;

            ShowDateCollection coll = new ShowDateCollection();
            coll.AddRange(show.ShowDateRecords());
            foreach (ShowDate sd in coll)
            {
                EventList.Add(new Event(sd));
            }
        }
    }

    public class Event
    {
        public int Id { get; set; }
        public long dtStamp { get; set; }
        public string Name { get; set; }
        public DateTime DateOfShow { get; set; }
        public List<Ticket> TicketList { get; set; }

        public Event() { }

        public Event(ShowDate sd)
        {
            Id = sd.Id;
            dtStamp = sd.DtStamp.Ticks;
            Name = sd.FriendlyUrl;
            DateOfShow = sd.DateOfShow_ToSortBy;

            ShowTicketCollection coll = new ShowTicketCollection();
            coll.AddRange(sd.ShowTicketRecords());
            foreach(ShowTicket st in coll)
            {
                TicketList.Add(new Ticket(st));
            }
        }

        public Event(int idx, long dtstamp, string name, DateTime dateOfShow)
        {
            Id = idx;
            dtStamp = dtstamp;
            Name = name;
            DateOfShow = dateOfShow;
        }
    }

    public class Ticket
    {
        public int Id { get; set; }
        public long dtStamp { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        //public List<Event> EventList { get; set; }

        public Ticket() { }

        public Ticket(ShowTicket st)
        {
            Id = st.Id;
            dtStamp = st.DtStamp.Ticks;
            Name = st.DisplayNameWithAttribsAndDescription;
            Price = st.PerItemPrice;
        }

        public Ticket(int idx, long dtstamp, string name, decimal price)
        {
            Id = idx;
            dtStamp = dtstamp;
            Name = name;
            Price = price;
        }
    }

    public class Merchandise
    {
        public int Id { get; set; }
        public long dtStamp { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Merchandise() { }

        public Merchandise(int idx, long dtstamp, string name, decimal price)
        {
            Id = idx;
            dtStamp = dtstamp;
            Name = name;
            Price = price;
        }
    }

}