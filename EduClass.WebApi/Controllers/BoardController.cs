using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EduClass.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Board")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BoardController : ApiController
    {

    }
}
