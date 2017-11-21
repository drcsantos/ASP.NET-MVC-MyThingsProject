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
    public class PersonController : Controller
    {
        private PersonService ServiceFactory()
        {                
            return new PersonService(User.Identity.Name);           
        }

        // GET: Person
        public ActionResult Index()
        {
            return View(ServiceFactory().FindAll());
        }

        // GET: Person/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Person value)
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

        // GET: Person/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)            
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var person = ServiceFactory().FindById(id.Value);
            if (person == null)            
                return HttpNotFound();
            
            return View(person);
        }

        // POST: Person/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)            
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var service = ServiceFactory();
            var personToUpdate = service.FindById(id.Value);
            if (TryUpdateModel(personToUpdate, "",
               new string[] { "Name", "NickName", "ImageLink"}))
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
            return View(personToUpdate);
        }

        // POST: Person/Delete/5
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