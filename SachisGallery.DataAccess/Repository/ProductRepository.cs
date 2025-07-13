using SachisGallery.DataAccess.Data;
using SachisGallery.DataAccess.Repository.IRepository;
using SachisGallery.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SachisGallery.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }

        public void Update(Product product)
        {
            var productFromDb = _db.Products.FirstOrDefault(u => u.Id == product.Id);
            if(productFromDb != null)
            {
                productFromDb.Title = product.Title;
                productFromDb.Description = product.Description;
                productFromDb.ISBN = product.ISBN;
                productFromDb.Author = product.Author;
                productFromDb.ListPrice = product.ListPrice;
                productFromDb.Price = product.Price;
                productFromDb.Price50 = product.Price50;
                productFromDb.Price100 = product.Price100;
                productFromDb.CategoryId = product.CategoryId;
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    productFromDb.ImageUrl = product.ImageUrl;
                }
            }

            _db.Products.Update(product);
        }
    }
}
