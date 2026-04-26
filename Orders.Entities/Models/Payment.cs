using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Orders.Shared.Enums;

namespace Orders.Entities.Models;

public class Payment
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();


    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid PaymentReferenceId { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Paid;

    // Navigation back to parent
    public PaymentReference PaymentReference { get; set; }

}