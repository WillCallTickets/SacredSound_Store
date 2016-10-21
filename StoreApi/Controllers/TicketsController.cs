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
    public class TicketsController : ControllerBase
    {
        /// <summary>
        /// convert cached collection into objects to be consumed
        /// </summary>
        /// <returns></returns>
        private List<Ticket> _list = null;
        private List<Ticket> List
        {
            get
            {
                if (_list == null)
                {
                    _list = new List<Ticket>();

                    foreach (Wcss.ShowTicket item in ctx.SaleTickets)
                        _list.Add(new Ticket(item.Id, DateTime.Now.Ticks, item.DisplayNameWithAttribsAndDescription, item.PerItemPrice));
                }

                return _list;
            }
        }

        public IEnumerable<Ticket> GetAll()
        {
            return List;
        }

        public Ticket GetById(int id)
        {
            if (id == 0)
            {
                ResetCache();
                return null;
            }

            var ent = List.FirstOrDefault((p) => p.Id == id);
            if (ent == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return ent;
        }

        public void ResetCache()
        {
            ctx.ResetApiDependency("api call");
        }

        //public IEnumerable<Ticket> GetTicketsByCategory(string category)
        //{
        //    return tickets.Where(
        //        (p) => string.Equals(p.Category, category,
        //            StringComparison.OrdinalIgnoreCase));
        //}
    }
}
