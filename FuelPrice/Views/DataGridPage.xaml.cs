using DevExpress.Maui.DataGrid;
using FuelPrice.Models;
using FuelPrice.ViewModels;
using System.Collections.ObjectModel;

namespace FuelPrice.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataGridPage : ContentPage
    {
        private CancellationTokenSource _cancelTokenSource;
        FuelDataGridViewModel ViewModel { get; }

        public ObservableCollection<Station> PStations { get; set; } = new ObservableCollection<Station>();
        public DataGridPage()
        {           
            InitializeComponent();
            BindingContext = ViewModel = new FuelDataGridViewModel();
            WireEvents();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
            ViewModel.IsBusy = true;
        }
 
        #region Private

        private void WireEvents()
        {
            grid.DoubleTap += DataGridView_DoubleTap;
        }


        #endregion

        #region Events
        async void DataGridView_DoubleTap(object sender, DataGridGestureEventArgs e)
        {
            if (e.Item != null)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    IsBusy = true;
                    GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                    _cancelTokenSource = new CancellationTokenSource();
                    var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                    var item = (Station)grid.GetRowItem(e.RowHandle);
                    var daddr = $"{item.location.latitude.ToString()},{item.location.longitude.ToString()}";
                    var saddr = $"{location.Latitude},{location.Longitude}";

                    if (DeviceInfo.Current.Platform == DevicePlatform.iOS || DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
                    {
                        var link = string.Format($"http://maps.apple.com/?daddr={daddr}&saddr={saddr}");
                        await Launcher.OpenAsync(link);
                    }
                    else if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                    {

                        var link = string.Format($"http://maps.google.com/?daddr={daddr}&saddr={saddr}");
                        await Launcher.OpenAsync(link);
                    }
                    IsBusy = false;

                });

            }
        }
         
        #endregion
    }
}