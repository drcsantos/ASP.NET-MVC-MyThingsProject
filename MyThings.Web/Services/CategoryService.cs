using MyThings.Web.DAL;
using MyThings.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace MyThings.Web.Services
{
    public class CategoryService
    {
        private string userName = string.Empty;
        private DataContext db = new DataContext();

        public CategoryService(string username)
        {
            this.userName = username.ToLowerInvariant();
        }

        public IEnumerable<Category> FindAll(bool include = false)
        {
            if (include)
                return db.Categories
                    .Include(i => i.Things)
                    .Where(o => o.UserName == userName)
                    .OrderBy(p => p.Name);
            else
                return db.Categories
                    .Where(o => o.UserName == userName)
                    .OrderBy(p => p.Name);
        }

        public Category FindById(int id)
        {
            return db.Categories
                .Include(i => i.Things)
                .SingleOrDefault(t => t.UserName == userName && t.CategoryID == id);
        }

        public Category Insert(Category value)
        {
            value.UserName = userName;
            db.Categories.Add(value);
            SaveChanges();
            return value;
        }

        public void Delete(int id)
        {
            var item = db.Categories.Find(id);
            db.Categories.Remove(item);
            SaveChanges();
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }
    }
}