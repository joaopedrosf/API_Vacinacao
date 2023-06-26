using System.Text.Json.Serialization;

namespace vacinacao_backend.Models.Enums {
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EnumSituacao {
        Agendado,
        Realizado,
        Cancelado
    }
}
