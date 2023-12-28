using DevExpress.Maui.Scheduler;
using FuelPrice.Models;
using FuelPrice.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
 

namespace FuelPrice.ViewModels
{
    public class FuelDataGridViewModel : BaseViewModel
    {

        public DXObservableCollection<Station> Stations { get; set; } = new DXObservableCollection<Station>();
        readonly IFuelDataStore<Station> FuelDataRepository = new FuelDataStore();
        bool isRefreshing = false;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    OnPropertyChanged("IsRefreshing");
                }
            }
        }

        ICommand pullToRefreshCommand = null;
        public ICommand PullToRefreshCommand
        {
            get { return pullToRefreshCommand; }
            set
            {
                if (pullToRefreshCommand != value)
                {
                    pullToRefreshCommand = value;
                    OnPropertyChanged("PullToRefreshCommand");
                }
            }
        }

        public FuelDataGridViewModel()
        {
            Title = "FuelDataGrid";
            Stations = new DXObservableCollection<Station>();
            PullToRefreshCommand = new Command(ExecutePullToRefreshCommand);
        }

        void ExecutePullToRefreshCommand()
        {
            Task.Factory.StartNew(() => {
                Thread.Sleep(3000);
                MainThread.BeginInvokeOnMainThread(() => {
                    OnAppearing();
                    IsRefreshing = false;
                });
            });
        }

        async public void OnAppearing()
        { 
            ObservableCollection<Station> stations = await FuelDataRepository.GetItemsAsync(true);
            Stations.Clear();
            foreach (Station station in stations)
            {
                Stations.Add(station);                 
            }
        }
    }
}