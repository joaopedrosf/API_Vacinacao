using System.Text.Json.Serialization;

namespace vacinacao_backend.Models.Enums {
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EnumPeriodicidade {
        Dias,
        Semanas,
        Meses,
        Anos
    }
}
