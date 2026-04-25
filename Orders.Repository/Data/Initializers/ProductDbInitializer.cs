using System.Threading.Tasks;
using Orders.Contracts;
using Orders.Entities.Models;
using Orders.Repository;

namespace Orders.Repository.Data.Initializers;

public static class ProductDbInitializer
{
    public static async Task Initialize(IRepositoryManager repoManager)
    {
        var existingProducts = await repoManager.Product.GetAllProductsAsync(false);
        if (existingProducts.Any())
            Console.WriteLine("Products Table is Already Populated");
            return;

        List<Product> products = new()
        {
            new()
            {
                Id          = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000001"),
                Name        = "Wireless Headphones",
                Description = "Premium noise-cancelling wireless headphones with 30hr battery",
                Price       = 149.99m,
                Quantity    = 50,
                CreatedAt   = DateTime.UtcNow,
            },
            new()
            {
                Id          = Guid.Parse("a1b2c3d4-0002-0002-0002-000000000002"),
                Name        = "Mechanical Keyboard",
                Description = "RGB backlit mechanical keyboard with Cherry MX switches",
                Price       = 89.99m,
                Quantity    = 30,
                CreatedAt   = DateTime.UtcNow,
            },
            new()
            {
                Id          = Guid.Parse("a1b2c3d4-0003-0003-0003-000000000003"),
                Name        = "USB-C Hub",
                Description = "7-in-1 USB-C hub with HDMI, USB 3.0 and SD card reader",
                Price       = 45.99m,
                Quantity    = 100,
                CreatedAt   = DateTime.UtcNow,
            },
            new()
            {
                Id          = Guid.Parse("a1b2c3d4-0004-0004-0004-000000000004"),
                Name        = "Webcam 1080p",
                Description = "Full HD webcam with built-in microphone and auto-focus",
                Price       = 79.99m,
                Quantity    = 5,
                CreatedAt   = DateTime.UtcNow,
            },
            new()
            {
                Id          = Guid.Parse("a1b2c3d4-0005-0005-0005-000000000005"),
                Name        = "Monitor Stand",
                Description = "Adjustable aluminium monitor stand with storage drawer",
                Price       = 35.99m,
                Quantity    = 3,
                CreatedAt   = DateTime.UtcNow,
            },
        };

        repoManager.Product.AddRange(products);
        await repoManager.SaveAsync();
        Console.WriteLine("✅ Products seeded successfully.");
    }
}