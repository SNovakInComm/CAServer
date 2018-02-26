using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CAApi.Models
{
    public class BootEntity
    {
        [Key]
        public byte[] IV { get; set; }

        public byte[] MasterKey { get; set; }

        public byte[] ValidationString { get; set; }

    }
}
