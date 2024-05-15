using RealApiData.Models;

namespace RealApiData.Repository.Abstract
{
    public interface IProductService
    {
        bool Add(Product model);

        Task<Product?> FindByIdAsync(int id);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task<IEnumerable<Product>> GetProducts();
    }
}
