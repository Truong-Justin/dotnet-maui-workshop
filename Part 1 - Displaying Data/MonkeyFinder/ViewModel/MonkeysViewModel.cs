using MonkeyFinder.Services;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    private MonkeyService monkeyService;
    // Raises the notifications to .NET Maui without us having to explicitly do so.
    public ObservableCollection<Monkey> Monkeys { get; } = new ObservableCollection<Monkey>();

    public MonkeysViewModel(MonkeyService monkeyService)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
    }

    [RelayCommand]
    private async Task GetMonkeysAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            List<Monkey> monkeys = await monkeyService.GetMonkeys();

            if (monkeys.Count != 0)
                Monkeys.Clear();

            foreach (Monkey monkey in monkeys)
            {
                Monkeys.Add(monkey); // Will raise OnPropertyChanged for each item added, which isn't very efficient.
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", 
                $"Unable to get monkeys: {ex.Message}", "OK"); // Only for demo purposes; I.E. this is not good logic for production.
        }
        finally
        {
            IsBusy = false;
        }
    }

}
