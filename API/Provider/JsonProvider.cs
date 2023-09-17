using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Provider
{
    public class JsonProvider
    {

        //Deserialize
        private JsonSerializerOptions deserializerOption = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        public T Deserialize<T>(string str)
        {
            return JsonSerializer.Deserialize<T>(str, deserializerOption);
        }

        //Serialize

        private JsonSerializerOptions serializerOption = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj,serializerOption);
        }

    }
}