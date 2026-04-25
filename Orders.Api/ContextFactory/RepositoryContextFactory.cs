using Orders.Repository;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Orders.Api.ContextFactory
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("mysqlConnection")
                ?? throw new InvalidOperationException("please Set your mysqlConnection string");

            var serverVersion = ServerVersion.AutoDetect(connectionString);

            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                .UseMySql(connectionString, serverVersion,
                    b => b.MigrationsAssembly("Orders.Api"));

            return new RepositoryContext(builder.Options);
        }
    }
}
