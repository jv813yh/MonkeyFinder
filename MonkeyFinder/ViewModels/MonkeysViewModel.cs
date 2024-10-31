using MonkeyFinder.Services.MonkeyServices;
using MvvmHelpers.Commands;
using System.Diagnostics;

namespace MonkeyFinder.ViewModels
{
    public class MonkeysViewModel : BaseViewModel
    {
        private readonly IMonkeyService _monkeyService;

        public ObservableCollection<Monkey> MonkeysObsCollection { get; }
        public AsyncCommand GetMonkeysCommand { get; }
        public MonkeysViewModel(IMonkeyService monkeyService)
        {
            _monkeyService = monkeyService;

            Title = "Monkey Finder";
            MonkeysObsCollection = new ObservableCollection<Monkey>();
            GetMonkeysCommand = new AsyncCommand(LoadMonkeysAsync);

        }

        async Task LoadMonkeysAsync()
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
