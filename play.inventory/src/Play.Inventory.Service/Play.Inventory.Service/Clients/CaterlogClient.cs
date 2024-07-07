using static Play.Inventory.Service.Dtos;

namespace Play.Inventory.Service.Clients;

public class CaterlogClient
{
    private readonly HttpClient _client;

    public CaterlogClient(HttpClient client)
    {
        _client = client;
    }
    //retive items from caterlog
    public async Task<IReadOnlyCollection<CaterlogItemDto>> GetCaterlogItemsAsync()
    {
        var items = await _client.GetFromJsonAsync<IReadOnlyCollection<CaterlogItemDto>>("/items");
        return items!;
    }
}
