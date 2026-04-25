namespace Orders.Shared.DataTransferObjects;

public record ProductDto
{
    public Guid Id {get; set;}
    public string Name {get; set;}
    public Decimal Price {get; set;}
    public string Description {get; set;}
}