using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Lib.DataTypes.Structures
{
    /// <summary>
    /// JSON settings
    /// </summary>
    public static class JsonSettings
    {
        /// <summary>
        /// JSON serializer settings
        /// </summary>
        public static readonly JsonSerializerSettings Settings = new()
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,     // Пропускаем аттрибуты
            DateParseHandling = DateParseHandling.None,                     // Выключаем парсинг дат
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,                   // Пропускаем пустые значения (API-вызов при создании запроса сам заполняет нужные поля)
            MissingMemberHandling = MissingMemberHandling.Ignore,
            Formatting = Formatting.Indented,

            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal },
                new StringEnumConverter() { NamingStrategy = new CamelCaseNamingStrategy() }
            },

            ContractResolver = new CamelCasePropertyNamesContractResolver()


            //Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
            //{
            //    //errors.Add(args.ErrorContext.Error.Message);
            //    args.ErrorContext.Handled = true;
            //}
        };

        /// <summary>
        /// JSON serializer settings with null and empty
        /// </summary>
        public static readonly JsonSerializerSettings SettingsWithNull = new()
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,     // Пропускаем аттрибуты
            DateParseHandling = DateParseHandling.None,                     // Выключаем парсинг дат
            DefaultValueHandling = DefaultValueHandling.Include,
            NullValueHandling = NullValueHandling.Include,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            Formatting = Formatting.Indented,

            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal },
                new StringEnumConverter() { NamingStrategy = new CamelCaseNamingStrategy() }
            },

            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}
