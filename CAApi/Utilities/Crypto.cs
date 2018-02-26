using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using CAApi.Models;

namespace CAApi.Utilities
{
    public class Crypto
    {

        // -------------------------------------------------- Private Members
        #region Private Members

        CADBContext _context;
        static byte[] _key;
        byte[] _iv;
        byte[] _upper;
        byte[] _lower;

        #endregion

        // -------------------------------------------------- Constructors
        #region Constructors

        public Crypto(CADBContext context)
        {
            _context = context;
        }

        public void Init()
        {
            Validated = false;
            Random rng = new Random();      // Replace this with a true rng

            _key = new byte[16];
            _iv = new byte[16];

            rng.NextBytes(_key);
            rng.NextBytes(_iv);
        }

        #endregion

        // -------------------------------------------------- Accessors
        #region Accessors

        public static bool Validated { get; set; }

        public string Password
        {
            set
            {
                SHA256Managed sha = new SHA256Managed();
                _lower = new byte[16];
                _upper = new byte[16];

                sha.Initialize();
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(value));
                for(int i=0; i<16; i++)
                {
                    _lower[i] = hash[i];
                    _upper[i] = hash[i + 16];
                }
            }
        }
        
        public byte[] IV
        {
            get
            {
                return _iv;
            }
            set
            {
                _iv = value;
            }
        }

        #endregion

        // -------------------------------------------------- Public Methods
        #region Public Methods

        public byte[] EncryptKey()
        {
            return ExcryptBytes(_key, _lower, _upper);
        }

        public byte[] EncryptString(string plainText)
        {
            return ExcryptString(plainText, _key, _iv);
        }

        public byte[] EncryptData(byte[] data)
        {
            return ExcryptBytes(data, _key, _iv);
        }

        public void DecryptKey(byte[] encrypedKey)
        {
            _key = DecryptBytes(encrypedKey, _lower, _upper);
        }

        public string DecryptToString(byte[] cipherText)
        {
            return DecryptString(cipherText, _key, _iv);
        }

        public byte[] DecryptToData(byte[] cipherData)
        {
            return DecryptBytes(cipherData, _key, _iv);
        }

        #endregion

        // -------------------------------------------------- Private Methods
        #region Private Methods

        private byte[] ExcryptString(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            using (AesManaged aesAlg = new AesManaged())
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

        private byte[] ExcryptBytes(byte[] plainBytes, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            AesManaged aesAlg = new AesManaged
            {
                Key = Key,
                IV = IV
            };
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (BinaryWriter swEncrypt = new BinaryWriter(csEncrypt))
                    {
                        //swEncrypt.Write(plainBytes);
                        swEncrypt.Write(plainBytes,0, plainBytes.Length);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
            return encrypted;
        }

        private string DecryptString(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;

            using (AesManaged aesAlg = new AesManaged())
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
            return plaintext;
        }

        private byte[] DecryptBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            byte[] unencryptedData;
            char[] result = new char[16];

            AesManaged aesAlg = new AesManaged
            {
                Key = Key,
                IV = IV
            };
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (BinaryReader srDecrypt = new BinaryReader(csDecrypt))
                    {
                        unencryptedData = srDecrypt.ReadBytes((int)msDecrypt.Length);
                    }
                }
            }
            return unencryptedData;
        }

        #endregion

    }
}
