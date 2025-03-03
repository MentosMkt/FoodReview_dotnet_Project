using Hawassa.Data;
using Hawassa.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hawassa.Controllers
{
    public class AddMenusController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        public AddMenusController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var Menu = context.Menu.OrderByDescending(p => p.Id).ToList();
            return View(Menu);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Menuform Menuform)
        {
            if (Menuform.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The Image file is required");
            }
            if (!ModelState.IsValid)
            {
                return View(Menuform);
            }

            //this save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddmmssfff");
            newFileName += Path.GetExtension(Menuform.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/Images/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                Menuform.ImageFile.CopyTo(stream);
            }

            // save the new Menu in the HawassaDB database
            AddMenu addMenu = new()
            {
                Name = Menuform.Name,
                Location = Menuform.Location,
                Description = Menuform.Description,
                Price = Menuform.Price,
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,
            };
            context.Menu.Add(addMenu);
            context.SaveChanges();

            return RedirectToAction("Index", "AddMenus");
        }
        public IActionResult Edit(int id)
        {
            var AddMenu = context.Menu.Find(id);
            if (AddMenu == null)
            {
                return RedirectToAction("Index", "AddMenus");
            }
            //create MenuForm from AddMenu
            var Menuform = new Menuform()
            {
                Name = AddMenu.Name,
                Location = AddMenu.Location,
                Description = AddMenu.Description,
                Price = (decimal)AddMenu.Price,
            };

            ViewData["AddMenuId"] = AddMenu.Id;
            ViewData["ImageFileName"] = AddMenu.ImageFileName;
            ViewData["CreatedAt"] = AddMenu.CreatedAt.ToString("MM/dd/yyyy");

            return View(Menuform);
        }
        [HttpPost]
        public IActionResult Edit(int id, Menuform Menuform)
        {
            var AddMenu = context.Menu.Find(id);
            if (AddMenu == null)
            {
                return RedirectToAction("Index", "AddMenus");
            }
            if (!ModelState.IsValid)
            {
                ViewData["AddMenuId"] = AddMenu.Id;
                ViewData["ImageFileName"] = AddMenu.ImageFileName;
                ViewData["CreatedAt"] = AddMenu.CreatedAt.ToString("MM/dd/yyyy");

                return View(Menuform);
            }
            //update the image file if we have a new image file

            string newFileName = AddMenu.ImageFileName;
            if (Menuform.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyy,MMddHHmmssfff");
                newFileName += Path.GetExtension(Menuform.ImageFile.FileName);

                String imageFullPath = environment.WebRootPath + "/Images/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    Menuform.ImageFile.CopyTo(stream);
                }
                //deelete the old image
                string oldImageFullPath = environment.WebRootPath + "/Images/" + AddMenu.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
            }

            //update the Menu in the database
            AddMenu.Name = Menuform.Name;
            AddMenu.Location = Menuform.Location;
            AddMenu.Description = Menuform.Description;
            AddMenu.Price = Menuform.Price;
            AddMenu.ImageFileName = newFileName;


            context.SaveChanges();
            return RedirectToAction("Index", "AddMenus");
        }

        public IActionResult Delete(int id)
        {
            var AddMenu = context.Menu.Find(id);
            if (AddMenu == null)
            {
                return RedirectToAction("index", "AddMenus");
            }

            String imageFullPath = environment.WebRootPath + "/Images" + AddMenu.ImageFileName;
            System.IO.File.Delete(imageFullPath);
            context.Menu.Remove(AddMenu);
            context.SaveChanges(true);
            return RedirectToAction("index", "AddMenus");
        }

        }
    }





