using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using StoreApi.Models;
using Wcss;

namespace StoreApi.Controllers
{
    public class EventsController : ControllerBase
    {
        /// <summary>
        /// convert cached collection into objects to be consumed
        /// </summary>
        /// <returns></returns>
        private List<Event> _list = null;
        private List<Event> List
        {
            get
            {
                if(_list == null)
                {
                    _list = new List<Event>();

                    foreach (Wcss.ShowDate item in ctx.SaleShowDates_All)
                        _list.Add(new Event(item.Id, item.DtStamp.Ticks, item.ListingString, item.DateOfShow_ToSortBy));
                }

                return _list;
            }            
        }

        public IEnumerable<Event> GetAll()
        {
            return List;
        }

        public Event GetById(int id)
        {
            var ent = List.FirstOrDefault((p) => p.Id == id);
            if (ent == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return ent;
        }
    }
}