using Microsoft.EntityFrameworkCore;
using Orders.Entities.Models;
using Orders.Contracts;

namespace Orders.Repository
{
	public class ProductRepository : RepositoryBase<Product>, IProductRepository
	{
		public ProductRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        	{

        	}

        public void CreateProduct(Product product)=> Create(product);
       

        public void DeleteProduct(Product product)=> Delete(product);


        public async Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges)=> await FindAll(trackChanges).OrderBy(p =>
        		p.Name).ToListAsync();


        public async Task<Product> GetProductAsync(Guid Id, bool trackChanges) => await FindByCondition(p => p.Id.Equals(Id), trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) => await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();

        public void AddRange(IEnumerable<Product> products) => CreateRange(products);
    } 
}
