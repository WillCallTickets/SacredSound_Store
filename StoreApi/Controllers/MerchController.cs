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
    public class MerchController : ControllerBase 
    {
        /// <summary>
        /// convert cached collection into objects to be consumed
        /// </summary>
        /// <returns></returns>
        private List<Merchandise> _list = null;
        private List<Merchandise> List
        {
            get
            {
                if(_list == null)
                {
                    _list = new List<Merchandise>();

                    foreach (Wcss.Merch item in ctx.SaleMerch)
                        _list.Add(new Merchandise(item.Id, item.DtStamp.Ticks, item.DisplayNameWithAttribs, item.Price_Effective));
                }

                return _list;
            }            
        }

        public IEnumerable<Merchandise> GetAll()
        {
            return List;
        }

        public Merchandise GetById(int id)
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