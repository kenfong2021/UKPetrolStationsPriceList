using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FuelPrice.Models
{
    public class Prices
    {
        [JsonProperty(nameof(E10))]
        public decimal E10 { get; set; }
        [JsonProperty(nameof(B7))]
        public decimal B7 { get; set; }       
    }
}
