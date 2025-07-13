using SachisGallery.DataAccess.Data;
using SachisGallery.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SachisGallery.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public UnitOfWork(ApplicationDbContext db) 
        {
            this._db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
        }
        

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
