using Microsoft.EntityFrameworkCore;
using RealApiData.Models;
using RealApiData.Repository.Abstract;

namespace RealApiData.Repository
{
    public class ProductRepository : IProductService
    {
        databaseContext _context;
        public ProductRepository(databaseContext context)
        {
            _context = context;
        }
        //get all
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }
        //get by id
        public Product GetById(int id)
        {
            return _context.Products.Find(id);
        }
        //add
        public bool Add(Product model)
        {
            try
            {
                _context.Products.Add(model);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        //delete
        public async Task DeleteAsync(Product product)
        {

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        //update
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        //get by id
        public async Task<Product?> FindByIdAsync(int id)
        {
            // var product = await _context.Product.FirstOrDefaultAsync(a => a.Id == id);
            var product = await _context.Products.FindAsync(id);
            return product;
        }


    }
}
