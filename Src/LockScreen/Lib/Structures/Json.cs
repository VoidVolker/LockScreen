using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LockScreen.Lib.Structures
{
    /// <summary>
    /// Abstract class for converting from and to JSON
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Json<T>
    {
        /// <summary>
        /// To JSON settings
        /// </summary>
        [JsonIgnore]
        public JsonSerializerSettings ToSettings = JsonSettings.Settings;

        /// <summary>
        /// From JSON settings
        /// </summary>
        [JsonIgnore]
#pragma warning disable CA2211 // Поля, не являющиеся константами, не должны быть видимыми
        public static JsonSerializerSettings FromSettings = JsonSettings.Settings;
#pragma warning restore CA2211 // Поля, не являющиеся константами, не должны быть видимыми

        /// <summary>
        /// Convert object to JSON
        /// </summary>
        /// <returns></returns>
        public string ToJson(JsonSerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(this, settings ?? ToSettings);
        }

        /// <summary>
        /// Parse JSON to selected type
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T FromJson(string json, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject<T>(json, settings ?? FromSettings);
        }

        /// <summary>
        /// Convertation error storage. Required for using in multi projects over several assemblies. It's a simplest solution to catch errors in that case.
        /// </summary>
        /// <remarks>
        /// Если в процессе конвертации данных произошла ошибка - она сохраняется в данную переменную<br/>
        /// Данная пеерменная необходима для отлова ошибок, возникающих в загруженных сборках. 
        /// Т.е., если основной код в из одной сборки, а конвертируемый класс из другой сборки,
        /// то ошибка не отлавливается вышестоящими try/catch. И повторный выброс той же ошибки 
        /// не меняет поведение, т.е. надо создавать новое исключение с данными из существующего.<br/>
        /// Отлов этой ошибки в <code>JsonSettings</code>: добавить в свойство <code>Error</code> 
        /// делегат для обработки ошибки, а в делегате <code>e.ErrorContext.Handled = true;</code>.<br/>
        /// Есть более общее решение для использования в <code>JObject.ToObject(serializer)</code>: 
        /// Создаем новый <code>JsonSerializer</code>, в нем вешаем делегат на событие <code>Error</code>, 
        /// а в делегате <code>e.ErrorContext.Handled = true;</code> и далее куда-то сохраняем детали об ошибке.<br/>
        /// Таким образом, в двух решениях из надо выделять отдельный объект под ошибки и как-то либо его прокидывать
        /// в сериализатор, либо в сериализаторе выделять поле и на каждый запрос создавать новый сериализатор. 
        /// В общем, проще всего именно в целевой объект записать ошибку/ошибки.
        /// </remarks>
        [JsonIgnore]
        public ErrorContext DeserializationError { get; private set; } = null;

        /// <summary>
        /// Deserialization error callback for Newtonsoft.Json lib
        /// </summary>
        /// <param name="_"></param>
        /// <param name="errorContext"></param>
        [OnError]
        internal void OnDeserializationError(StreamingContext _, ErrorContext errorContext)
        {
            errorContext.Handled = true;
            DeserializationError = errorContext;
        }
    }
}
