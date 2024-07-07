using Play.Inventory.Service.Entities;
using static Play.Inventory.Service.Dtos;

namespace Play.Inventory.Service;

public static class Extentions
{
    public static InventoryItemDto AsDto(this Inventoryitems item, string name, string description)
    {
        return new InventoryItemDto(item.CatalogItemId, name, description, item.Quantity, item.AcquiredDate);
    }
}
