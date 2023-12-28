using System.Collections.ObjectModel;

namespace FuelPrice.Models
{
    public class Station
    {
        public string site_id { get; set; }
        public string brand { get; set; }
        public string address { get; set; }
        public string postcode { get; set; }
        public Location location { get; set; }
        public Prices prices { get; set; }
        public decimal petrol { get; set; }
        public decimal diesel { get; set; }
        public double distance { get; set; }
    }

    public class Stations
    {
        public ObservableCollection<Station> stations { get; set; }
    }

}