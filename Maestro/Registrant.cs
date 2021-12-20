using System;
using System.Text.Json.Serialization;

namespace Maestro
{
    public class Registrant
    {
        [JsonPropertyName("id")] public string AppId { get; set; }
        [JsonPropertyName("address")] public string Address { get; set; }
        [JsonPropertyName("last_ping")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] public DateTime LastPing { get; set; }
    }
}
