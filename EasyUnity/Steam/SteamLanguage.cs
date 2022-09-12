using System;
using System.Collections.Generic;
using System.Linq;
using NoxLibrary;

namespace BlueOakBridge.EasyUnity.Steam
{
    public class SteamLanguage : IByteListAppendable, IEquatable<SteamLanguage>
    {
        public string EnglishName;
        public string NativeName;
        public string APICode;
        public string WebAPICode;

        public SteamLanguage() : this("English", "English", "english", "en") { }
        public SteamLanguage(ByteArray ba) : this(ba.GetString()) { }
        public SteamLanguage(string webAPICode)
        {
            if (TryFindByWebAPICode(webAPICode, out SteamLanguage reference))
            {
                this.EnglishName = reference.EnglishName;
                this.NativeName = reference.NativeName;
                this.APICode = reference.APICode;
                this.WebAPICode = reference.WebAPICode;
            }
            else throw new ArgumentException("[EasyUnity.SteamLanguage] The string provided does not match any SteamLanguage.WebAPICode");
        }
        public SteamLanguage(string englishName, string nativeName, string apiCode, string webAPICode)
        {
            EnglishName = englishName ?? throw new ArgumentNullException(nameof(englishName));
            NativeName = nativeName ?? throw new ArgumentNullException(nameof(nativeName));
            APICode = apiCode ?? throw new ArgumentNullException(nameof(apiCode));
            WebAPICode = webAPICode ?? throw new ArgumentNullException(nameof(webAPICode));
        }

        public static SteamLanguage FindByEnglishName(string englishName) => ALL.Where((SteamLanguage sl) => sl.EnglishName.Equals(englishName)).FirstOrDefault();
        public static SteamLanguage FindByNativeName(string nativeName) => ALL.Where((SteamLanguage sl) => sl.NativeName.Equals(nativeName)).FirstOrDefault();
        public static SteamLanguage FindByAPICode(string apiCode) => ALL.Where((SteamLanguage sl) => sl.APICode.Equals(apiCode)).FirstOrDefault();
        public static SteamLanguage FindByWebAPICode(string webAPICode) => ALL.Where((SteamLanguage sl) => sl.WebAPICode.Equals(webAPICode)).FirstOrDefault();

        public static bool TryFindByEnglishName(string englishName, out SteamLanguage steamLanguage)
        {
            IEnumerable<SteamLanguage> steamLanguages = ALL.Where((SteamLanguage sl) => sl.EnglishName.Equals(englishName));
            if (steamLanguages.Count() == 1)
            {
                steamLanguage = steamLanguages.First();
                return true;
            }
            steamLanguage = default;
            return false;
        }
        public static bool TryFindByNativeName(string nativeName, out SteamLanguage steamLanguage)
        {
            IEnumerable<SteamLanguage> steamLanguages = ALL.Where((SteamLanguage sl) => sl.NativeName.Equals(nativeName));
            if (steamLanguages.Count() == 1)
            {
                steamLanguage = steamLanguages.First();
                return true;
            }
            steamLanguage = default;
            return false;
        }
        public static bool TryFindByAPICode(string apiCode, out SteamLanguage steamLanguage)
        {
            IEnumerable<SteamLanguage> steamLanguages = ALL.Where((SteamLanguage sl) => sl.APICode.Equals(apiCode));
            if (steamLanguages.Count() == 1)
            {
                steamLanguage = steamLanguages.First();
                return true;
            }
            steamLanguage = default;
            return false;
        }
        public static bool TryFindByWebAPICode(string webAPICode, out SteamLanguage steamLanguage)
        {
            IEnumerable<SteamLanguage> steamLanguages = ALL.Where((SteamLanguage sl) => sl.WebAPICode.Equals(webAPICode));
            if (steamLanguages.Count() == 1)
            {
                steamLanguage = steamLanguages.First();
                return true;
            }
            steamLanguage = default;
            return false;
        }

        public override bool Equals(object obj) => obj is SteamLanguage language && Equals(language);
        public bool Equals(SteamLanguage other) => WebAPICode == other.WebAPICode;
        public override int GetHashCode() => 383792610 + EqualityComparer<string>.Default.GetHashCode(WebAPICode);
        public override string ToString() => EnglishName;
        public ByteList AppendBytes(ByteList bl) => bl.Append(WebAPICode);

        public static bool operator ==(SteamLanguage left, SteamLanguage right) => left.Equals(right);
        public static bool operator !=(SteamLanguage left, SteamLanguage right) => !(left == right);

        public static SteamLanguage[] ALL { get; } = new SteamLanguage[]
        {
            new SteamLanguage("Arabic",                 "العربية",                  "arabic",      "ar"),
            new SteamLanguage("Bulgarian",              "български език",           "bulgarian",    "bg"),
            new SteamLanguage("Chinese (Simplified)",   "简体中文",                  "schinese",     "zh-CN"),
            new SteamLanguage("Chinese (Traditional)",  "繁體中文",                  "tchinese",     "zh-TW"),
            new SteamLanguage("Czech",                  "čeština",                  "czech",        "cs"),
            new SteamLanguage("Danish",                 "Dansk",                    "danish",       "da"),
            new SteamLanguage("Dutch",                  "Nederlands",               "dutch",        "nl"),
            new SteamLanguage("English",                "English",                  "english",      "en"),
            new SteamLanguage("Finnish",                "Suomi",                    "finnish",      "fi"),
            new SteamLanguage("French",                 "Français",                 "french",       "fr"),
            new SteamLanguage("German",                 "Deutsch",                  "german",       "de"),
            new SteamLanguage("Greek",                  "Ελληνικά",                 "greek",        "el"),
            new SteamLanguage("Hungarian",              "Magyar",                   "hungarian",    "hu"),
            new SteamLanguage("Italian",                "Italiano",                 "italian",      "it"),
            new SteamLanguage("Japanese",               "日本語",                    "japanese",     "ja"),
            new SteamLanguage("Korean",                 "한국어",                    "koreana",      "ko"),
            new SteamLanguage("Norwegian",              "Norsk",                    "norwegian",    "no"),
            new SteamLanguage("Polish",                 "Polski",                   "polish",       "pl"),
            new SteamLanguage("Portuguese",             "Português",                "portuguese",   "pt"),
            new SteamLanguage("Portuguese-Brazil",      "Português-Brasil",         "brazilian",    "pt-BR"),
            new SteamLanguage("Romanian",               "Română",                   "romanian",     "ro"),
            new SteamLanguage("Russian",                "Русский",                  "russian",      "ru"),
            new SteamLanguage("Spanish-Spain",          "Español-España",           "spanish",      "es"),
            new SteamLanguage("Spanish-Latin America",  "Español-Latinoamérica",    "latam",        "es-419"),
            new SteamLanguage("Swedish",                "Svenska",                  "swedish",      "sv"),
            new SteamLanguage("Thai",                   "ไทย",                      "thai",         "th"),
            new SteamLanguage("Turkish",                "Türkçe",                   "turkish",      "tr"),
            new SteamLanguage("Ukrainian",              "Українська",               "ukrainian",    "uk"),
            new SteamLanguage("Vietnamese",             "Tiếng Việt",               "vietnamese",   "vn"),
        };
    }
}