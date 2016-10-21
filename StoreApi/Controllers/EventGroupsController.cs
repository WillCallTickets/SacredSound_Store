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
    public class EventGroupsController : ControllerBase
    {
        /// <summary>
        /// convert cached collection into objects to be consumed
        /// </summary>
        /// <returns></returns>
        private List<EventGroup> _list = null;
        private List<EventGroup> List
        {
            get
            {
                if(_list == null)
                {
                    _list = new List<EventGroup>();

                    foreach (Wcss.ShowDate item in ctx.SaleShowDates_All)
                    {
                        var entity = _list.FirstOrDefault((p) => p.Id == item.TShowId);
                        if (entity == null)
                            _list.Add(new EventGroup(item.ShowRecord));
                    }                    
                }

                return _list;
            }            
        }

        public IEnumerable<EventGroup> GetAll()
        {
            return List;
        }

        public EventGroup GetById(int id)
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
