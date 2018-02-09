using System;
namespace CAApi.Models
{
    public class CertificateInfo : Resource
    {
        public string ID { get; set; }
        public string Hash { get; set; }
        public string CountryName { get; set; }
        public string StateOrProvinceName { get; set; }
        public string LocalityName { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationalUnitName { get; set; }
        public string CommonName { get; set; }
        public string IssueDate { get; set; }
        public string ExpireDate { get; set; }
    }
}
