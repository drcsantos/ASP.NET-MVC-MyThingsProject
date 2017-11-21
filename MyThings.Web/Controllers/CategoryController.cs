using MyThings.Web.Models;
using MyThings.Web.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyThings.Web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private CategoryService ServiceFactory()
        {                
            return new CategoryService(User.Identity.Name);           
        }

        // GET: Category
        public ActionResult Index()
        {
            return View(ServiceFactory().FindAll());
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceFactory().Insert(value);
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(value);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)            
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var category = ServiceFactory().FindById(id.Value);
            if (category == null)            
                return HttpNotFound();
            
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)            
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var service = ServiceFactory();
            var categoryToUpdate = service.FindById(id.Value);
            if (TryUpdateModel(categoryToUpdate, "",
               new string[] { "Name", "Icon"}))
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
            return View(categoryToUpdate);
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                ServiceFactory().Delete(id.Value);
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