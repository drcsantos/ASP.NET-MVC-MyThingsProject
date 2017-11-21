using MyThings.Web.DAL;
using MyThings.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace MyThings.Web.Services
{
    public class PersonService
    {
        private string userName = string.Empty;
        private DataContext db = new DataContext();

        public PersonService(string username)
        {
            this.userName = username.ToLowerInvariant();
        }

        public IEnumerable<Person> FindAll(bool include = false)
        {
            if (include)
                return db.People
                    .Include(p => p.Things)
                    .Where(o => o.UserName == userName)
                    .OrderBy(p => p.Name);
            else
                return db.People
                    .Where(o => o.UserName == userName)
                    .OrderBy(p => p.Name);
        }

        public Person FindById(int id)
        {
            return db.People
                .Include(p => p.Things)
                .SingleOrDefault(p => p.UserName == userName && p.PersonID == id);
        }

        public Person Insert(Person value)
        {
            value.UserName = userName;
            db.People.Add(value);
            SaveChanges();
            return value;
        }

        public void Delete(int id)
        {
            var item = db.People.Find(id);
            db.People.Remove(item);
            SaveChanges();
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }
    }
}