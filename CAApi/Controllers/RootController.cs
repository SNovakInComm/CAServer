using System;
using Microsoft.AspNetCore.Mvc;
using CAApi.Controllers;

namespace CAApi
{
    [Route("/")]
    [ApiVersion("1.0")]
    public class RootController : Controller
    {
        [HttpGet(Name = nameof(GetRoot))]
        public IActionResult GetRoot()
        {
            var response = new
            {
                href = Url.Link(nameof(GetRoot), null), 
                certs = new { href = Url.Link(nameof(CertificateController.GetCerts), null)}
            };

            return Ok(response);
        }
    }
}
