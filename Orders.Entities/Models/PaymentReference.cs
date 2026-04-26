using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Orders.Shared.Enums;

namespace Orders.Entities.Models;

public class PaymentReference
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid Reference { get; set; } = Guid.NewGuid();
    public Guid? OrderId { get; set; }
    public Order? Order { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public Payment? Payment { get; set; }
}