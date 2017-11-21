using MyThings.Web.DAL;
using MyThings.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace MyThings.Web.Services
{
    public class ThingService
    {
        private string userName = string.Empty;
        private DataContext db = new DataContext();

        public ThingService(string username)
        {
            this.userName = username.ToLowerInvariant();
        }

        public IEnumerable<Thing> FindAll()
        {
            return db.Things
                .Include(t => t.Person)
                .Where(o => o.UserName == userName)
                .OrderBy(p => p.Name);
        }

        public Thing FindById(Guid id)
        {
            return db.Things
                .Include(t => t.Person)
                .Include(t => t.Category)
                .SingleOrDefault(t => t.UserName == userName && t.ID == id);
        }

        public Thing Insert(Thing value)
        {
            value.ID = Guid.NewGuid();
            value.UserName = userName;
            db.Things.Add(value);
            SaveChanges();
            return value;
        }

        public void Delete(Guid id)
        {
            var item = db.Things.Find(id);
            db.Things.Remove(item);
            SaveChanges();
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }
    }
}