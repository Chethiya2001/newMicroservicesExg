namespace Play.Inventory.Service;

public class Dtos
{
    public record GrantItemDto(Guid UserId, Guid CatalogItemId, int Quantity);
    public record InventoryItemDto(Guid catalogItemId, string name, string discription, int Quantity, DateTimeOffset AcquiredDate);
    public record CaterlogItemDto(Guid Id, string Name, string Description);

}
