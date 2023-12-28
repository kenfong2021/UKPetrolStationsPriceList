using System.Collections.ObjectModel;

namespace FuelPrice.Services
{
    public interface IFuelDataStore<T>
    { 
        Task<T> GetItemAsync(string id);

        Task<ObservableCollection<T>> GetItemsAsync(bool forceRefresh = false);
    }

}
