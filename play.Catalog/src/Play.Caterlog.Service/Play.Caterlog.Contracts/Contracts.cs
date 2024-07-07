namespace Play.Caterlog.Contracts;

public class Contracts
{
    public record CaterlogItemCreated(Guid ItemId, string Name, string Description);

    public record CaterlogItemUpdated(Guid ItemId, string Name, string Description);

    public record CaterlogItemDeleted(Guid ItemId);
    
}
