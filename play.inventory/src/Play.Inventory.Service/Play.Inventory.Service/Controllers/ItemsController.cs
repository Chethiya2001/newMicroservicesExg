using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;
using static Play.Inventory.Service.Dtos;

namespace Play.Inventory.Service.Controllers;

[ApiController]
[Route("[controller]")]

public class ItemsController : ControllerBase
{
    private readonly IRepository<Inventoryitems> _repository;
    private readonly CaterlogClient _caterlogClient;

    public ItemsController(IRepository<Inventoryitems> repository, CaterlogClient caterlogClient)
    {
        _repository = repository;
        _caterlogClient = caterlogClient;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest();
        }
        var caterlogItems = await _caterlogClient.GetCaterlogItemsAsync();
        var inventoryItemEntity = await _repository.GetAllAsync(item => item.UserId == userId);
        //combine
        var inventoryitemsDtos = inventoryItemEntity.Select(item =>
        {
            var caterlogItem = caterlogItems.Single(caterlogItem => caterlogItem.Id == item.CatalogItemId);
            return item.AsDto(caterlogItem.Name, caterlogItem.Description);
        });
        return Ok(inventoryitemsDtos);

    }

    [HttpPost]
    public async Task<ActionResult> PostAsync(GrantItemDto grantItemsDto)
    {
        var inventoryItem = await _repository.GetItems(item => item.UserId == grantItemsDto.UserId && item.CatalogItemId == grantItemsDto.CatalogItemId);

        if (inventoryItem is null)
        {
            inventoryItem = new Inventoryitems
            {
                UserId = grantItemsDto.UserId,
                CatalogItemId = grantItemsDto.CatalogItemId,
                Quantity = grantItemsDto.Quantity,
                AcquiredDate = DateTimeOffset.Now
            };
            await _repository.CreateAsync(inventoryItem);

        }
        else
        {
            inventoryItem.Quantity += grantItemsDto.Quantity;
            await _repository.UpdateAsync(inventoryItem);
        }
        return Ok("Created.");
    }
}
