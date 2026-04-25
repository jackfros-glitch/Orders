// Orders.Api/Extensions/SeedExtensions.cs
using Orders.Contracts;
using Orders.Repository.Data.Initializers;

namespace Orders.Api.Extensions;

public static class SeedExtensions
{
    public static async Task SeedProducts(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var repoManager = scope.ServiceProvider
            .GetRequiredService<IRepositoryManager>();

        await ProductDbInitializer.Initialize(repoManager);
    }
}