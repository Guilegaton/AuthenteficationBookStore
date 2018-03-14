using AuthenteficationBookStore.Models;
using BookStore.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthenteficationBookStore.Controllers
{
    public class HomeController : Controller
    {
        private BookStoreContext _db;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public HomeController()
        {
            _db = new BookStoreContext();
        }


        public ActionResult Index()
        {
            return View(_db.Books.ToList());
        }

        [Authorize]
        public ActionResult BookInfo(int id)
        {
            Book book = _db.Books.FirstOrDefault(item => item.Id == id);
            if (book != null)
            {
                return View(book);
            }
            return new HttpStatusCodeResult(404);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditBook(Book currentBook)
        {

            Book book = _db.Books.FirstOrDefault(item => item.Id == currentBook.Id && item.Name == currentBook.Name && item.Author == currentBook.Author);
            if (book != null)
            {
                return View(currentBook);
            }
            return new HttpStatusCodeResult(404);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AddNewBook()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult Upload()
        {
            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    string path = Server.MapPath("~/Files/" + fileName);
                    upload.SaveAs(path);
                }
            }
            return Json("файл загружен");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewBook(Book newBook)
        {
            Book book = _db.Books.FirstOrDefault(item => item.Name.ToLower() == newBook.Name.ToLower() && item.Author.ToLower() == newBook.Author.ToLower());
            if (book == null)
            {
                newBook.ImageURL = "/Files/" + newBook.ImageURL;
                _db.Books.Add(newBook);
                _db.SaveChanges();
            }
            else
            {
                ModelState.AddModelError("", "This book already exists");
                return View(newBook);
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditedBook(Book editedBook)
        {
            _db.Entry(editedBook).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteBook(Book deletedBook)
        {
            Book book = _db.Books.FirstOrDefault(item => item.Id == deletedBook.Id && item.Name == deletedBook.Name);
            if (book != null)
            {
                _db.Books.Remove(book);
                _db.SaveChanges();
            }
            else
            {
                ModelState.AddModelError("", "Book not found");
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult BuyBook(Book model)
        {
            if (((List<Book>)Session["Cart"]) == null || ((List<Book>)Session["Cart"]).Count == 0)
            {
                Session["Cart"] = new List<Book>();
            }
            ((List<Book>)Session["Cart"]).Add(model);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Cart()
        {
            if (((List<Book>)Session["Cart"]) != null || ((List<Book>)Session["Cart"]).Count != 0)
            {
                return View((List<Book>)Session["Cart"]);
            }
            else
                return View(new List<Book>());
        }

        [HttpPost]
        [Authorize]
        public ActionResult Buy()
        {
            try
            {
                foreach (var item in (List<Book>)Session["Cart"])
                {
                    Purchase newOrder = new Purchase()
                    {
                        Person = User.Identity.Name,
                        Date = DateTime.Now,
                        BookId = item.Id
                    };
                    _db.Purchases.Add(newOrder);
                }

                _db.SaveChanges();
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", "Something wrong");
                return View("Cart");
            }
            Session["Cart"] = new List<Book>();
            return RedirectToAction("Index", "Home");
        }
        

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult JsonGetBooks(string name)
        {

            var Base = _db.Books.ToList();
            if (!string.IsNullOrEmpty(name))
            {
                var SortedBase = Base.Where(e => e.Author.Contains(name));
                return Json(SortedBase);
            }
            return Json(Base);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Users()
        {
            using (UserContext usdb = new Models.UserContext())
            {
                return View(usdb.AspNetUsers.ToList());
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteUser(string id)
        {
            using (UserContext usdb = new Models.UserContext())
            {
                AspNetUsers deletedUser = usdb.AspNetUsers.Find(id);
                if (deletedUser != null)
                {
                    usdb.AspNetUsers.Remove(deletedUser);
                    usdb.SaveChanges();
                }
                return RedirectToAction("Users", "Home");
            }
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(string id)
        {
            using (UserContext usdb = new Models.UserContext())
            {
                AspNetUsers editedUser = usdb.AspNetUsers.Find(id);
                if (editedUser != null)
                {
                    List<SelectListItem> items = new List<SelectListItem>();
                    items.Add(new SelectListItem { Text = editedUser.AspNetRoles.FirstOrDefault().Name, Value = editedUser.AspNetRoles.FirstOrDefault().Name });
                    foreach (var item in usdb.AspNetRoles)
                    {
                        if (!item.Id.Equals(editedUser.AspNetRoles.FirstOrDefault().Id))
                        {
                            items.Add(new SelectListItem { Text = item.Name, Value = item.Name });
                        }
                    }
                    ViewBag.Roles = items;
                    return View(editedUser);
                }
                return new HttpStatusCodeResult(404);
            }
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(string Id, string Roles)
        {
            using (UserContext usdb = new Models.UserContext())
            {
                AspNetUsers EditedUser = usdb.AspNetUsers.Find(Id);
                if (EditedUser != null)
                {
                    usdb.AspNetUsers.Find(Id).AspNetRoles.Remove(usdb.AspNetUsers.Find(Id).AspNetRoles.First());
                    usdb.AspNetUsers.Find(Id).AspNetRoles.Add(usdb.AspNetRoles.First(item => item.Name.Equals(Roles)));
                    usdb.SaveChanges();
                }
                return RedirectToAction("Users", "Home");
            }
        }
    }
}