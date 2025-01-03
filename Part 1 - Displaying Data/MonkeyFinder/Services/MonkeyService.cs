using System.Net.Http.Json;

namespace MonkeyFinder.Services;

public class MonkeyService
{
    HttpClient httpClient;
    public MonkeyService()
    {
        httpClient = new HttpClient();
    }

    List<Monkey> monkeyList = new List<Monkey>();
    public async Task<List<Monkey>> GetMonkeys()
    {
        if (monkeyList?.Count > 0)
        {
            return monkeyList;
        }

        string url = "https://montemagno.com/monkeys.json";

        HttpResponseMessage response = await httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            monkeyList = await response.Content.ReadFromJsonAsync<List<Monkey>>();
        }

        /*To be used if we don't get a json response from the URL above.
        ----------------------------------------------------------------
        using Stream stream = await FileSystem.OpenAppPackageFileAsync("monkeydata.json");
        using StreamReader reader = new StreamReader(stream);
        string contents = await reader.ReadToEndAsync();
        monkeyList = JsonSerializer.Deserialize<List<Monkey>>(contents);*/

        return monkeyList;
    }
}
