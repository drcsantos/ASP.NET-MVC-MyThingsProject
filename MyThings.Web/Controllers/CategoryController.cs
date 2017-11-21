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

        private string Icon(string icon)
        {
            return $"glyphicon glyphicon-{icon}";
        }

        private SelectListItem IconItem(string icon)
        {
            return new SelectListItem() { Value = Icon(icon.ToLower()), Text = icon };
        }

        private void LoadIcons(string selected = "")
        {
            var icons = new List<SelectListItem>();

            icons.Add(IconItem("Asterisk"));
            icons.Add(IconItem("Plus"));
            icons.Add(IconItem("Minus"));
            icons.Add(IconItem("Cloud"));
            icons.Add(IconItem("Envelope"));
            icons.Add(IconItem("Pencil"));
            icons.Add(IconItem("Glass"));
            icons.Add(IconItem("Music"));
            icons.Add(IconItem("Search"));
            icons.Add(IconItem("Heart"));
            icons.Add(IconItem("Star"));
            icons.Add(IconItem("Star-empty"));
            icons.Add(IconItem("User"));
            icons.Add(IconItem("Film"));
            icons.Add(IconItem("Ok"));
            icons.Add(IconItem("Remove"));
            icons.Add(IconItem("Zoom-in"));
            icons.Add(IconItem("Zoom-out"));
            icons.Add(IconItem("Off"));
            icons.Add(IconItem("Signal"));
            icons.Add(IconItem("Cog"));
            icons.Add(IconItem("Trash"));
            icons.Add(IconItem("Home"));
            icons.Add(IconItem("File"));
            icons.Add(IconItem("Time"));
            icons.Add(IconItem("Download-alt"));
            icons.Add(IconItem("Download"));
            icons.Add(IconItem("Upload"));
            icons.Add(IconItem("Inbox"));
            icons.Add(IconItem("Play-circle"));
            icons.Add(IconItem("Repeat"));
            icons.Add(IconItem("Refresh"));
            icons.Add(IconItem("List-alt"));
            icons.Add(IconItem("Lock"));
            icons.Add(IconItem("Flag"));
            icons.Add(IconItem("Headphones"));
            icons.Add(IconItem("Volume-off"));
            icons.Add(IconItem("Tag"));
            icons.Add(IconItem("Tags"));
            icons.Add(IconItem("Book"));
            icons.Add(IconItem("Bookmark"));
            //TODO: Emprove this list

            if (!string.IsNullOrEmpty(selected))
            {
                foreach (var item in icons)
                {
                    if (item.Value == selected)
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }            

            ViewBag.icons = icons;
        }

        // GET: Category
        public ActionResult Index()
        {
            return View(ServiceFactory().FindAll());
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            LoadIcons();
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

            LoadIcons(category.Icon);
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
               new string[] { "Name", "Icon" }))
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