using System.ComponentModel.DataAnnotations;

namespace Orders.Shared.DataTransferObjects;

public record ProductRequestDto
{
    public Guid ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; } = 1;
}