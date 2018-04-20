using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CAApi.Models;

namespace CAApi.Controllers
{
    [Route("/Api/[controller]")]
    [ApiVersion("1.0")]
    public class CertificateController : Controller
    {
        [HttpGet("Generate/{KeyPassword}",  Name = nameof(GenerateCert))]
        public IActionResult GenerateCert(String KeyPassword, CancellationToken ct)
        {
            AuthorityController CA = new AuthorityController(); // Should this be a static object???

            if(KeyPassword.Length < 4 || KeyPassword.Length > 1023)
            {
                ApiError invalidPasswordError = new ApiError()
                {
                    Message = "Invalid Key Passphrase",
                    Detail = "Pass Phrase Must be 4 to 1023 characters in length"
                };
                return BadRequest();
            }

            CA.Password = KeyPassword;
            KeyDescriptor kd = new KeyDescriptor
            {
                Length = 2048,
                Algorithm = "aes256"
            };

            CA.CreateKey(kd);

            var result = new
            {
                ID = KeyPassword,
                Key = CA.Key
            };

            return Ok(result);
        }

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
