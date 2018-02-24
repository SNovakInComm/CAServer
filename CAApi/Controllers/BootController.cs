using System;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CAApi.Models;
using System.IO;
using System.Text;

namespace CAApi.Controllers
{

    [ApiVersion("1.0")]
    [Route("/Boot")]
    public class BootController : Controller
    {
        const string _validationString = "Valid100";
        CADBContext _context;

        public BootController(CADBContext context) { _context = context; }

        [HttpPost("{token}", Name = nameof(PostBootToken))]
        public async Task<IActionResult> PostBootToken(string token, CancellationToken ct)
        {
            AesCryptoServiceProvider crypto = new AesCryptoServiceProvider();
            SHA256CryptoServiceProvider sha = new SHA256CryptoServiceProvider();

            var count = await _context.Boot.CountAsync(ct);

            if (count == 0)
            {
                // Replace this with a true rng
                Random rng = new Random();

                byte[] key = new byte[16];
                byte[] IV = new byte[16];

                byte[] lower = new byte[16];
                byte[] upper = new byte[16];

                sha.Initialize();
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(token));

                for (int i = 0; i < 16; i++)
                {
                    rng.NextBytes(key);
                    rng.NextBytes(IV);
                    lower[i] = hash[i];
                    upper[i] = hash[i + 16];
                }

                var encryptedValidationString = EncryptStringToBytes_Aes(_validationString, key, IV);
                var encryptedKey = EncryptBytesToBytes_Aes(key, lower, upper);
                var decrypted = DecryptBytesFromBytes_Aes(encryptedKey, lower, upper);

                return Ok(decrypted);
            }
            else
            {
                //var decrypted = DecryptStringFromBytes_Aes(result, key, key);
                return Ok(">0");
            }


        }
        
        private byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        private byte[] EncryptBytesToBytes_Aes(byte[] plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }
            return plaintext;
        }

        static byte[] DecryptBytesFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext;

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return Encoding.UTF8.GetBytes(plaintext);
        }
    }
}