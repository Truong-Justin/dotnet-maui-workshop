using MonkeyFinder.Services;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    private MonkeyService monkeyService;
    // Raises the CollectionChanged event to .NET Maui without us having to explicitly do so.
    public ObservableCollection<Monkey> Monkeys { get; } = new ObservableCollection<Monkey>();

    public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
        this.connectivity = connectivity;
        this.geolocation = geolocation;
    }

    IConnectivity connectivity;
    IGeolocation geolocation;

    [RelayCommand]
    async Task GetClosestMonkeyAsync()
    {
        if (IsBusy || Monkeys.Count == 0)
            return;
        
        try
        {
            var location = await geolocation.GetLastKnownLocationAsync();
            if (location is null)
            {
                location = await geolocation.GetLocationAsync(
                    new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30),
                    });
            }

            if (location is null)
                return;

            var first = Monkeys.OrderBy(monkey => 
                location.CalculateDistance(monkey.Latitude, monkey.Longitude, DistanceUnits.Miles)
                ).FirstOrDefault();

            if (first is null)
                return;

            await Shell.Current.DisplayAlert("Closest Monkey",
                $"{first.Name} in {first.Location}", "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!",
                $"Unable to get closest monkey: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task GoToDetailsAsync(Monkey monkey)
    {
        if (monkey is null)
            return;

        await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true,
            new Dictionary<string, object>
            {
                {"Monkey", monkey}
            });
    }

    [RelayCommand]
    private async Task GetMonkeysAsync()
    {
        if (IsBusy)
            return;

        try
        {
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Internet issue", $"Check your internet and try again!", "OK");
                return;
            }

            IsBusy = true;
            List<Monkey> monkeys = await monkeyService.GetMonkeys();

            if (monkeys.Count != 0)
                Monkeys.Clear();

            foreach (Monkey monkey in monkeys)
            {
                Monkeys.Add(monkey); // Will raise CollectionChanged for each item added, which isn't very efficient.
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
