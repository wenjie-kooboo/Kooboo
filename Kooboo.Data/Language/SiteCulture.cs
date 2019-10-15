//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com
//All rights reserved.
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.Data.Language
{
    public static class SiteCulture
    {
        private static object _culturelock = new object();

        private static Dictionary<string, string> _initcultures;

        public static Dictionary<string, string> InitCultures
        {
            get
            {
                if (_initcultures == null)
                {
                    lock (_culturelock)
                    {
                        if (_initcultures == null)
                        {
                            List<string> startlist = new List<string>
                            {
                                "zh",
                                "es",
                                "en",
                                "es",
                                "hi",
                                "ar",
                                "pt",
                                "bn",
                                "ru",
                                "ja",
                                "pa",
                                "de",
                                "jv",
                                "ms",
                                "jv",
                                "te",
                                "vi",
                                "ko",
                                "fr",
                                "mr",
                                "ta",
                                "ur",
                                "tr",
                                "it",
                                "th",
                                "nl",
                                "el",
                                "sv"
                            };

                            _initcultures = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                            foreach (var item in startlist)
                            {
                                if (LanguageSetting.ISOTwoLetterCode.ContainsKey(item))
                                {
                                    var value = LanguageSetting.ISOTwoLetterCode[item];
                                    if (!_initcultures.ContainsKey(item))
                                    {
                                        _initcultures.Add(item, value);
                                    }
                                }
                            }
                        }
                    }
                }
                return _initcultures;
            }
        }

        public static Dictionary<string, string> List(Guid siteId)
        {
            Dictionary<string, string> result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var site = GlobalDb.WebSites.Get(siteId);

            if (site != null)
            {
                var setting = GlobalDb.GlobalSetting.Query.Where(o => o.OrganizationId == site.OrganizationId).SelectAll().Find(o => o.Name == "culture");

                if (setting != null)
                {
                    foreach (var item in setting.KeyValues)
                    {
                        result[item.Key] = item.Value;
                    }
                }
            }

            foreach (var item in InitCultures.ToList())
            {
                if (!result.ContainsKey(item.Key))
                {
                    result[item.Key] = item.Value;
                }
            }
            return result;
        }
    }
}