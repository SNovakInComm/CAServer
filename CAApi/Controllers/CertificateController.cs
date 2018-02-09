using System;
using System.Threading;
using System.Threading.Tasks;
using CAApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CAApi.Controllers
{
    [Route("/[controller]")]
    [ApiVersion("1.0")]
    public class CertificateController : Controller
    {
        [HttpGet(Name = nameof(GetCerts))]
        public IActionResult GetCerts()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{CertID}", Name = nameof(GetCertById))]
        public IActionResult GetCertById(String CertID, CancellationToken ct)
        {
            var certificate = new CertificateInfo
            {
                Href = Url.Link(nameof(GetCertById), new { CertID = CertID }),
                ID = CertID.ToString(),
                Hash = "AAAABBBBAAAABBBB",
                CountryName = "US",
                LocalityName = "Portland",
                StateOrProvinceName = "Oregon",
                OrganizationName = "The Best Organization",
                OrganizationalUnitName = "DEV",
                CommonName = "Some Guy",
                IssueDate = "today",
                ExpireDate = "tomorrow"
            };

            return Ok(certificate);
        }
    }

}
