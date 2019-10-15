//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com
//All rights reserved.
using System.Security.Cryptography.X509Certificates;

namespace Kooboo.Data.Server
{
    public static class SslCertificate
    {
        static SslCertificate()
        {
            var bytes = Kooboo.Data.Embedded.EmbeddedHelper.GetBytes("kooboo.pfx", typeof(Kooboo.Data.Models.WebSite));

            var cert = new X509Certificate2(bytes, "kooboo");

            DefaultCert = cert;
        }

        public static X509Certificate2 DefaultCert { get; set; }
    }
}