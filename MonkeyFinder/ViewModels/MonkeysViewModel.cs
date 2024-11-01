using CommunityToolkit.Mvvm.Input;
using MonkeyFinder.Services.MonkeyServices;
using MonkeyFinder.Views;
using MvvmHelpers.Commands;
using System.Diagnostics;

namespace MonkeyFinder.ViewModels
{
    public partial class MonkeysViewModel : BaseViewModel
    {
        private readonly IMonkeyService _monkeyService;

        public ObservableCollection<Monkey> MonkeysObsCollection { get; }
        public AsyncCommand GetMonkeysCommand { get; }
        public MonkeysViewModel(IMonkeyService monkeyService)
        {

            Title = "Monkey Finder";
            _monkeyService = monkeyService;

            MonkeysObsCollection = new ObservableCollection<Monkey>();
            
            GetMonkeysCommand = new AsyncCommand(LoadMonkeysAsync);

        }

        [RelayCommand]
        private async Task GoToDetailsAsync(Monkey currentMonkey)
        {
            if(currentMonkey == null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true,
                    new Dictionary<string, object>
                    {
                        { "Monkey", currentMonkey }
                    });
        }

        private async Task LoadMonkeysAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
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
                await Application.Current.MainPage.DisplayAlert("Error", $"Unable to get monkes: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
