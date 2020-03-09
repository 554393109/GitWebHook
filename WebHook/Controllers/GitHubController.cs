using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebHook.Controllers
{
    public class GitHubController : BaseController
    {
        // GET: api/GitHub
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/GitHub/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/GitHub
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/GitHub/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/GitHub/5
        public void Delete(int id)
        {
        }
    }
}
