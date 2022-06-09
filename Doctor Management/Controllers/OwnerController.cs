using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Doctor_Management.IRepository;
using Doctor_Management.Models;
using Doctor_Management.Models_View;
using System.IO;
using Newtonsoft.Json;

namespace Doctor_Management.Controllers
{
    public class OwnerController : Controller
    {
        private readonly IRepositoryData<Owner> owner;
        private readonly IRepositoryData<MedicName> medic;
        private readonly IWebHostEnvironment environment;
        
        private readonly List<string> Extintion = new List<string> { ".jpg", ".png" };
        
        public OwnerController(IRepositoryData<Owner> owner,IRepositoryData<MedicName> Medic,IWebHostEnvironment Environment)
        {
            this.owner = owner;
            this.medic = Medic;
            this.environment = Environment;
        }

        private void AddMedicNames()
        {
            if (!medic.GetAll().Any())
            {
                var FileJ = environment.WebRootPath + Path.Combine("/Medic/", "MedicNames.json");
                var Strjosn = System.IO.File.ReadAllText(FileJ);
                var josn = JsonConvert.DeserializeObject<List<MedicName>>(Strjosn);
                medic.Add(josn);
            }
        }
        public IActionResult Index()
        {
            GetUser();
            if (owner.GetAll().ToList().Count > 0)
            {
                return RedirectToAction(nameof(Details));
            }
            else
            {
                var OM = new OwnerView { Create = true };
                AddMedicNames();
                return View(OM);
            }
        }
        private Users GetUser()
        {
            Users users = new Users(HttpContext);
            ViewBag.user = users.User;
            ViewBag.login = users.Login;
            ViewBag.admin = users.Admin;
            return users;
        }

        public IActionResult Details()
        {
            if (GetUser().Admin)
            {
                var O =  owner.GetAll().ToList().FirstOrDefault();
                return View(O);
            }
            return NotFound();
        }
        static byte[] imglogo; //save old Logo static
        public async Task<IActionResult> Edits(int? id)
        {

            if (id == null || !GetUser().Admin)
                return NotFound();
            GetUser();
            var model = await owner.FindAsync(id);
            imglogo = model.Logo;
            var views = new OwnerView
            {
                Id = model.Id,NameOwner = model.NameOwner , Titel = model.Titel,
                Logo = model.Logo , Addres = model.Addres , Create = false,
                Phones = model.Phones
            };
            return View("Index",views);
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Edits(OwnerView model)
        {
            GetUser();
            var file = Request.Form.Files;
            if (file.Any())
            {
                var logo = file.FirstOrDefault();
                if (!Extintion.Contains(Path.GetExtension(logo.FileName).ToLower()))
                {
                    ModelState.AddModelError("Logo", "صورة اللوجو يجب ان تكون من امتداد[JPG OR PNG]");
                    return View("Index", model);
                }
                if (logo.Length > Hex.MaxAllowImage)
                {
                    ModelState.AddModelError("Logo", "مساحة اللوجو يجب ان لا تتخطى 2ميجا بايت!!");
                    return View("Index", model);
                }
                imglogo = logo.setimage();
            }
            var saveedit = new Owner
            {
                Id = model.Id,NameOwner = model.NameOwner , Logo = imglogo , Titel = model.Titel
                ,Addres = model.Addres ,Phones = model.Phones
            };
            owner.Update(saveedit);
            return RedirectToAction(nameof(Details));
        }


        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Adddoctor(OwnerView Model)
        {
            GetUser();
            var file = Request.Form.Files;
            if (!file.Any())
            {
                ModelState.AddModelError("Logo", "برجاء ادخل صورة اللوجو");
                return View("Index", Model);
            }
            var logo = file.FirstOrDefault();
            if (!Extintion.Contains(Path.GetExtension(logo.FileName).ToLower()))
            {
                ModelState.AddModelError("Logo", "صورة اللوجو يجب ان تكون من امتداد[JPG OR PNG]");
                return View("Index", Model);
            }
            if(logo.Length > Hex.MaxAllowImage)
            {
                ModelState.AddModelError("Logo", "مساحة اللوجو يجب ان لا تتخطى 2ميجا بايت!!");
                return View("Index", Model);
            }
            var savedModel = new Owner
            {
                NameOwner = Model.NameOwner,
                Addres = Model.Addres,
                Phones = Model.Phones,
                Logo = logo.setimage(),
                Titel = Model.Titel
            };
            await owner.AddAsync(savedModel);
            return Redirect("/Login/Index");
        }
    }
}
