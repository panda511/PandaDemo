using ModelVerify.Models;
using System.Web.Http;

namespace ModelVerify.Controllers
{
    public class PersonController : ApiController
    {
        [HttpPost]
        public Person GetData(Person p)
        {
            return p;
        }
    }
}
