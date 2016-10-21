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
    public class ControllerBase : ApiController
    {
        protected Wcss._ContextBase ctx = new _ContextBase();
        private long publishDate;

        public ControllerBase()
        {
            //ping it - make sure it is populated
            //publish date ensures that we have the proper cache dependencies in place
            publishDate = ctx.Api_PublishDate;
        }

        public void API_RemoveCacheObjects()
        {
            string f = "lk";
        }

        //public void RefreshSelectiveAPIObjects()
        //{
        //    long oldPublishDate = ctx.Api_PublishDate;

        //    //do for shows




        //    //do for merch
            
        //    //do for downloads

        //    //loop thru objects and if the object publish date is newer than the old publish date
        //    //refresh the object





        //    //refresh publish date
        //    ctx.Api_PublishDate = DateTime.Now.Ticks;
        //}
    }
}
