using System;
using Microsoft.AspNetCore.Mvc;

namespace CAApi.Controllers
{
    [Route("/Certificates")]
    [ApiVersion("1.0")]
    public class CertificateController : Controller
    {
        [HttpGet(Name = nameof(GetCerts))]
        public IActionResult GetCerts()
        {
            throw new NotImplementedException();
        }

    }
}
