
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Caterlog.Service.Entities;
using Play.Common;
using static Play.Caterlog.Contracts.Contracts;
using static Play.Caterlog.Service.Dtos;



namespace Play.Caterlog.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> _itemRepository;
        //send messages
        private readonly IPublishEndpoint _publishedEndpoint;
        public ItemsController(IRepository<Item> itemRepository, IPublishEndpoint publishEndpoint)
        {
            _itemRepository = itemRepository;
            _publishedEndpoint = publishEndpoint;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
        {

            var items = (await _itemRepository.GetAllAsync()).Select(item => item.AsDto());

            return Ok(items);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemByIdAsync(Guid id)
        {
            var item = await _itemRepository.GetItems(id);
            if (item == null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        //Add Items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto createItemDto)
        {
            var item = new Item
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _itemRepository.CreateAsync(item);

            await _publishedEndpoint.Publish(new CaterlogItemCreated(item.Id, item.Name, item.Description));


            return CreatedAtAction(nameof(GetItemByIdAsync), new { id = item.Id }, item);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemAsynv(Guid id, UpdateItemDto updateItemDto)
        {

            var existingItem = await _itemRepository.GetItems(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await _itemRepository.UpdateAsync(existingItem);
            await _publishedEndpoint.Publish(new CaterlogItemUpdated(existingItem.Id, existingItem.Name, existingItem.Description));


            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemAsync(Guid id)
        {
            var item = await _itemRepository.GetItems(id);
            if (item == null)
            {
                return NotFound();
            }

            await _itemRepository.RemoveAync(item.Id);
            await _publishedEndpoint.Publish(new CaterlogItemDeleted(item.Id));
            return NoContent();
        }
    }
}
