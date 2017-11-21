using MyThings.Web.Models;
using MyThings.Web.Services;
using MyThings.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MyThings.Web.Controllers
{
    [Authorize]
    public class ThingController : Controller
    {
        private ThingService _service = null;

        public ThingService Service
        {
            get
            {
                return _service == null ? new ThingService(User.Identity.Name) : _service;
            }
        }

        private IEnumerable<Category> GetCategories(bool include = false)
        {
            return (new CategoryService(User.Identity.Name)).FindAll(include);
        }

        private void LoadCategories(object selectedValue = null)
        {
            ViewBag.CategoryID = new SelectList(GetCategories(), "CategoryID", "Name", selectedValue);
        }

        private IEnumerable<Person> GetPeople()
        {
            return (new PersonService(User.Identity.Name)).FindAll();
        }

        private void LoadPeople(object selectedValue = null)
        {
            ViewBag.PersonID = new SelectList(GetPeople(), "PersonID", "FullName", selectedValue);
        }

        // GET: Thing
        public ActionResult Index(string searchString)
        {
            var categories = GetCategories(true).Where(c => c.Things.Count > 0).OrderBy(c => c.Name);            
            return View(categories);
        }

        // GET: Thing/Details/5
        public ActionResult Details(Guid? id)
        {
            return Edit(id);
        }

        // GET: Thing/Create
        public ActionResult Create()
        {
            LoadCategories();
            return View();
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Thing value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Service.Insert(value);
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(value);
        }

        // GET: Thing/Lend/5
        public ActionResult Lend(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var thing = Service.FindById(id.Value);
            if (thing == null)
                return HttpNotFound();

            var view = new LendViewModel()
            {
                ThingID = thing.ID,
                ThingName = thing.Name,
                ThingImageLink = thing.ImageLink,
                ThingCategoryName = thing.Category.Name,
                LentDate = thing.LentDate == null ? DateTime.Now.Date : thing.LentDate.Value,
                PersonID = thing.PersonID
            };
            LoadPeople(thing.PersonID);
            return View(view);
        }

        // POST: Thing/Lend/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Lend")]
        [ValidateAntiForgeryToken]
        public ActionResult LendPost(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var service = Service;
            var thingToUpdate = service.FindById(id.Value);
            if (TryUpdateModel(thingToUpdate, "",
               new string[] { "PersonID", "LentDate" }))
            {
                try
                {
                    service.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(thingToUpdate);
        }

        // GET: Thing/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var thing = Service.FindById(id.Value);
            if (thing == null)
                return HttpNotFound();

            LoadCategories(thing.CategoryID);
            return View(thing);
        }

        // POST: Thing/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var service = Service;
            var thingToUpdate = service.FindById(id.Value);
            if (TryUpdateModel(thingToUpdate, "",
               new string[] { "Name", "Description", "ImageLink", "CategoryID" }))
            {
                try
                {
                    service.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(thingToUpdate);
        }

        // POST: Thing/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                Service.Delete(id.Value);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
            return RedirectToAction("Index");
        }
    }
}