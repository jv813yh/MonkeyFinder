using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace MonkeyFinder.ViewModels
{
    [QueryProperty("Monkey", "Monkey")]
    public partial class MonkeyDetailsViewModel : BaseViewModel
    {
        [ObservableProperty]
        private Monkey _monkey;

        // Global API for getting the map
        private readonly IMap _map;
        public MonkeyDetailsViewModel(IMap map)
        {
            _map = map;
        }

        /// <summary>
        /// Method to
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OpenMapAsync()
        {
            try
            {
                if(IsBusy)
                {
                    return;
                }

                IsBusy = true;


                // Open the map with the monkey's location
                await _map.OpenAsync(_monkey.Latitude, _monkey.Longitude,
                    new MapLaunchOptions
                    {
                        Name = _monkey.Name,
                        NavigationMode = NavigationMode.None
                    });

                //var url = new Uri($"https://www.google.com/maps?q={_monkey.Latitude},{_monkey.Longitude}");
                //await Launcher.OpenAsync(url);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: {0}", ex);
                await Shell.Current.DisplayAlert("Error", $"Unable to open map: {ex}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
