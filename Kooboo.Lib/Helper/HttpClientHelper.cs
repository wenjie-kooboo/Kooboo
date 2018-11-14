﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace Kooboo.Lib.Helper
{
    public static class HttpClientHelper
    {
        private static HttpClient _client;
       
        public static HttpClient Client
        {
            get
            {
                return _client;
            }
        }

        private static CookieContainer _cookieContainer;

        static HttpClientHelper()
        {
            _cookieContainer = new CookieContainer();
            _client = CreateHttpClient(_cookieContainer);

        }

        public static void SetCookieContainer(CookieContainer cookieContainer,string url)
        {
            Uri uri;
            if(Uri.TryCreate(url,UriKind.Absolute,out uri))
            {
                var cookies = cookieContainer.GetCookies(uri);
                foreach (var cookieObj in cookies)
                {
                    var cookie = cookieObj as Cookie;
                    _cookieContainer.Add(uri, cookie);
                }
            }
        }
        private static HttpClient CreateHttpClient(CookieContainer cookieContainer)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                CookieContainer = cookieContainer
            };

            handler.Proxy = null;
            handler.AllowAutoRedirect = true;

            HttpClient client = new HttpClient(handler);
            client.Timeout = new TimeSpan(0, 0, 45);

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36");
            client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");

            return client;
        }
    }
}
