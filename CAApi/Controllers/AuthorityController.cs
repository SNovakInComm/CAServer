using System;
using System.IO;
using System.Diagnostics;
using CAApi.Models;

namespace CAApi.Controllers
{
    public class AuthorityController
    {

        // ------------------------- Private Members
        private string _password;
        private string _currentKey;


        // ------------------------- Constructors


        // ------------------------- Accessors

        public string Password { set { _password = value; } }

        // for testing... remove this
        public string Key { get { return _currentKey;  } }

        // ------------------------- Public Methods

        public bool CreateKey()
        {

            return true;
        }

        public bool CreateKey(KeyDescriptor keyDescription)
        {
            string arguments = "genrsa";
            
            if(keyDescription.Algorithm != null)
                arguments += " -" + keyDescription.Algorithm;
            arguments += " -passout pass:" + _password;
            arguments += " " + keyDescription.Length;

            Process openssl = new Process();
            openssl.StartInfo.UseShellExecute = false;
            openssl.StartInfo.RedirectStandardOutput = true;
            openssl.StartInfo.FileName = "openssl.exe";
            openssl.StartInfo.Arguments = arguments;
            openssl.Start();

            _currentKey = openssl.StandardOutput.ReadToEnd();
            openssl.WaitForExit();

            return true;
        }

        // ------------------------- Private Methods



    }
}
