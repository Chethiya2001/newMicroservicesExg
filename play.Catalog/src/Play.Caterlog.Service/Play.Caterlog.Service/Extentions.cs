using Play.Caterlog.Service.Entities;
using static Play.Caterlog.Service.Dtos;

namespace Play.Caterlog.Service
{
    public static class Extentions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name!, item.Description!, item.Price, item.CreatedDate);
        }
    }
}
