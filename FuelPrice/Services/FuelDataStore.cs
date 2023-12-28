using FuelPrice.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
//using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static DevExpress.Data.Helpers.ExpressiveSortInfo;
using System.Data;

namespace FuelPrice.Services
{
    public class FuelDataStore : IFuelDataStore<Station>
    {
        public ObservableCollection<Station> stations;
        HttpClient client;
        JsonSerializerSettings jsonSerializerSettings;
        private CancellationTokenSource _cancelTokenSource;
 
        public FuelDataStore()
        {
            client = new HttpClient();
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
 
        }

        public async Task<Station> GetItemAsync(string id)
        {
            await GetItemsAsync(true);
            return await Task.FromResult(this.stations.FirstOrDefault(s => s.site_id == id));
        }

        public async Task<ObservableCollection<Station>> GetItemsAsync(bool forceRefresh = false)
        {
            var maxDegreeOfParallelism = Environment.ProcessorCount - 1;
            var parallelOption = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };
            object syncLock = new object();
             
            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            _cancelTokenSource = new CancellationTokenSource();
            var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            stations = new ObservableCollection<Station>();
            var colUrl = new List<string>() 
            { "https://applegreenstores.com/fuel-prices/data.json",
              "https://fuelprices.asconagroup.co.uk/newfuel.json",
              "https://storelocator.asda.com/fuel_prices_data.json",
              "https://www.bp.com/en_gb/united-kingdom/home/fuelprices/fuel_prices_data.json",
              "https://fuelprices.esso.co.uk/latestdata.json",
              "https://www.morrisons.com/fuel-prices/fuel.json",
              "https://fuel.motorfuelgroup.com/fuel_prices_data.json",
              "https://www.rontec-servicestations.co.uk/fuel-prices/data/fuel_prices_data.json",
              "https://api.sainsburys.co.uk/v1/exports/latest/fuel_prices_data.json",
              "https://www.sgnretail.uk/files/data/SGN_daily_fuel_prices.json",
              "https://www.shell.co.uk/fuel-prices-data.html",
              "https://www.tesco.com/fuel_prices/fuel_prices_data.json"
            };

            foreach (var objUrl in colUrl)
            {
                Uri uri = new Uri(objUrl);
                try
                {
                    client = new HttpClient();

                    HttpResponseMessage response = await client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        Stations temp = JsonConvert.DeserializeObject<Stations>(content);

                        Parallel.ForEach(temp.stations, parallelOption, station =>
                        {
                            station.petrol = station.prices.E10;
                            station.diesel = station.prices.B7;
                            station.distance = Math.Round(location.CalculateDistance(station.location.latitude, station.location.longitude, DistanceUnits.Kilometers),1);

                            lock (syncLock)
                            {
                                stations.Add(station);
                            }
                        });
                        
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"\tERROR {0}", ex.Message);
                }

            }

            return stations;
        }
    }
}