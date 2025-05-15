using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace GestionParcAuto.Classes.CarData
{
    public class CarModel
    {
        [JsonPropertyName("year")]
        public int Year { get; set; }
        [JsonPropertyName("make")]
        public string Make { get; set; }
        [JsonPropertyName("model")]
        public string Model { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
