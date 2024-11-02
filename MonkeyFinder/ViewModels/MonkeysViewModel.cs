using CommunityToolkit.Mvvm.Input;
using MonkeyFinder.Services.MonkeyServices;
using MonkeyFinder.Views;
using MvvmHelpers.Commands;
using System.Diagnostics;

namespace MonkeyFinder.ViewModels
{
    public partial class MonkeysViewModel : BaseViewModel
    {
        // Service for getting the list of monkeys
        private readonly IMonkeyService _monkeyService;

        // Global API for checking network connectivity
        private readonly IConnectivity _connectivity;

        // Global API for getting the geolocation
        private readonly IGeolocation _geolocation;

        // Observable collection of monkeys
        public ObservableCollection<Monkey> MonkeysObsCollection { get; }

        // Command to get the list of monkeys
        public AsyncCommand GetMonkeysCommand { get; }


        public MonkeysViewModel(IMonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
        {
            Title = "Monkey Finder";
            _monkeyService = monkeyService;
            _connectivity = connectivity;
            _geolocation = geolocation;

            MonkeysObsCollection = new ObservableCollection<Monkey>();
            
            GetMonkeysCommand = new AsyncCommand(LoadMonkeysAsync);
        }


        [RelayCommand]
        private async Task GetClosestMonkeyAsync()
        {
            if (IsBusy || MonkeysObsCollection.Count == 0)
            {
                return;
            }

            try
            {
                // Check if we have permissions to access the location
                if (!await IsPermissionsGrantedAsync())
                {
                    await Shell
                         .Current
                         .DisplayAlert("Permissions Denied", 
                                "Unable to get the closest monkey without location permissions", 
                                "Ok");


                    return;
                }

                IsBusy = true;

                // Get the current location
                var location = await _geolocation.GetLastKnownLocationAsync();

                if(location == null)
                {
                    location = await _geolocation.GetLocationAsync(
                        new GeolocationRequest
                        {
                            DesiredAccuracy = GeolocationAccuracy.Medium,
                            Timeout = TimeSpan.FromSeconds(30)
                        });
                }

                if (location == null)
                {
                    return;
                }

                var firstMonkey = MonkeysObsCollection
                                  .OrderBy(m => location.CalculateDistance(m.Latitude, m.Longitude, DistanceUnits.Kilometers))
                                  .FirstOrDefault();

                if(firstMonkey == null)
                {
                    return;
                }

                // Show the closest monkey in an alert
                await Shell.Current.DisplayAlert("Closest Monkey", $"The closest monkey is {firstMonkey.Name} from {firstMonkey.Location}", "Ok");

                // Get the closest monkey

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"Unable to get closest monkey: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Method for executing the command to go to the details page with the selected monkey
        /// </summary>
        /// <param name="currentMonkey"></param>
        /// <returns></returns>
        [RelayCommand]
        private async Task GoToDetailsAsync(Monkey currentMonkey)
        {
            if(currentMonkey == null)
            {
                return;
            }

            // Go to the details page with the selected monkey
            await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true,
                    new Dictionary<string, object>
                    {
                        { "Monkey", currentMonkey }
                    });
        }

        /// <summary>
        /// Method for executing the command to get the list of monkeys
        /// Check if we have network connectivity
        /// </summary>
        /// <returns></returns>
        private async Task LoadMonkeysAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                // Check if we have network connectivity
                if(_connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Internet issue", "No internet connection available", "Ok");
                    return;
                }

                // Check if we can get the monkeys
                IsBusy = true;

                // Get the list of monkeys
                List<Monkey> monkeys = await _monkeyService.GetMonkeysAsync();

                if(MonkeysObsCollection.Count != 0)
                {
                    MonkeysObsCollection.Clear();
                }

                // Set the list of monkeys to the observable collection
                foreach (var monkey in monkeys)
                {
                    MonkeysObsCollection.Add(monkey);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                //await Application.Current.MainPage.DisplayAlert("Error", $"Unable to get monkes: {ex.Message}", "Ok");
                await Shell.Current.DisplayAlert("Error", $"Unable to get monkes: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Method for checking if the permissions were granted
        /// </summary>
        /// <returns>True if permissions were granted</returns>
        private async Task<bool> IsPermissionsGrantedAsync()
        {
            // Check if we have permissions to access the location
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if(status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            return status == PermissionStatus.Granted;
        }
    }
}
