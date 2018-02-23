using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//-aes128 -aes192 -aes256 -aria128 -aria192 -aria256 -camellia128 -camellia192 -camellia256 -des -des3 -idea

namespace CAApi.Controllers
{
    
    public class KeyDescriptor
    {
        public int Length { get; set; }
        public string Algorithm { get; set;  }        
    }
}
