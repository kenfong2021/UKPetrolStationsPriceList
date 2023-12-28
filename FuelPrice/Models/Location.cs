using System.Text.Json.Serialization;

namespace FuelPrice.Models
{
    public class Location
    {
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public double latitude { get; set; }
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public double longitude { get; set; }
    }
}