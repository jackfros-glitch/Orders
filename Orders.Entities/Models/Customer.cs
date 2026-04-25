using System.ComponentModel.DataAnnotations;

namespace Orders.Entities.Models;

public class Customer
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; }

    [Required]
    // Hashed Password
    public string PasswordHash { get; set; }

    public ICollection<Order>? Orders { get; set; } = new List<Order>();
}