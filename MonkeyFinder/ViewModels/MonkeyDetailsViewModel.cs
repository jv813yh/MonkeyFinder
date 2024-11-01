using CommunityToolkit.Mvvm.ComponentModel;

namespace MonkeyFinder.ViewModels
{
    [QueryProperty("Monkey", "Monkey")]
    public partial class MonkeyDetailsViewModel : BaseViewModel
    {
        [ObservableProperty]
        private Monkey _monkey;
    }
}
