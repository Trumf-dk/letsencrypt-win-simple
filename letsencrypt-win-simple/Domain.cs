using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LetsEncrypt.ACME.Simple
{
    public class Domain
    {
        public string DomainName { get; set; }
        public string Host { get; set; }
        public string WebRootPath { get; set; }
        public bool IsDomain { get; set; }
        public bool HasHttpsBinding { get; set; }
        public bool HasRenewal { get; set; }
        public int DomainId { get; set; }
        public long SiteId { get; set; }
    }
}
