using SachisGallery.DataAccess.Data;
using SachisGallery.DataAccess.Repository.IRepository;
using SachisGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SachisGallery.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }

        public void Update(Category category)
        {
            _db.Categories.Update(category);
        }
    }
}
