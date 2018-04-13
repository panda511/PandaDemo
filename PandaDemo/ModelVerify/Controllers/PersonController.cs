using ModelVerify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ModelVerify.Controllers
{
    public class PersonController : ApiController
    {
        public Person GetData(Person p)
        {
            return p;
        }
    }
}
