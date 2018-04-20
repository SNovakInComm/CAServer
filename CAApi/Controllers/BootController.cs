using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CAApi.Models;
using CAApi.Utilities;

namespace CAApi.Controllers
{

    [ApiVersion("1.0")]
    [Route("/Api/Boot")]
    public class BootController : Controller
    {
        const string _validationString = "ThisIsATotallyValidString100";
        CADBContext _context;

        public BootController(CADBContext context) { _context = context; }

        [HttpPost("{token}", Name = nameof(PostBootToken))]
        public async Task<IActionResult> PostBootToken(string token, CancellationToken ct)
        {
            var count = await _context.Boot.CountAsync(ct);
            Crypto cipher = new Crypto(_context);

            if (count == 0)
            {
                cipher.Init();
                cipher.Password = token;

                var encryptedValidationString = cipher.EncryptString(_validationString);
                var encryptedKey = cipher.EncryptKey();

                _context.Boot.Add(new BootEntity()
                {
                    IV = cipher.IV,
                    MasterKey = encryptedKey,
                    ValidationString = encryptedValidationString
                });
                _context.SaveChanges();

                Crypto.Validated = true;

                return Ok();
            }
            else
            {
                cipher.Init();
                cipher.Password = token;

                var entity = await _context.Boot.FirstAsync();
                try
                {
                    byte[] keyBytes = entity.MasterKey;
                    cipher.DecryptKey(keyBytes);
                    cipher.IV = entity.IV;

                    var decrypted = cipher.DecryptToString(entity.ValidationString);

                    if (decrypted != _validationString)
                        return Unauthorized();

                    Crypto.Validated = true;

                    return Ok();
                }
                catch(System.Security.Cryptography.CryptographicException)
                {
                    return Unauthorized();
                }
            }
        }
    }
}