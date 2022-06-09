using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;
using Doctor_Management.IRepository;
using Doctor_Management.Models_View;
using NToastNotify;

namespace Doctor_Management.Controllers
{
    public class priceController : Controller
    {
        private readonly IRepositoryData<Price> price;
        private readonly IToastNotification toast;

        public readonly IRepositoryData<Reveal> Reveal;

        public priceController(IRepositoryData<Price> price , IRepositoryData<Reveal> reveal ,  IToastNotification toast)
        {
            this.price = price;
            this.Reveal = reveal;
            this.toast = toast;
        }

        private Users GetUser()
        {
            Users users = new Users(HttpContext);
            ViewBag.user = users.User;
            ViewBag.login = users.Login;
            ViewBag.admin = users.Admin;
            return users;
        }
        public IActionResult Index()
        {
            if (GetUser().Admin)
            {
                return View(price.GetAll());
            }

            return NotFound();
        }

        public IActionResult Create()
        {
            if (GetUser().Admin)
            {
                var model = new PriceView { IsCreate = true };
                return View(model);
            }
            return NotFound();
        }

        [HttpPost ,ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PriceView model)
        {
            GetUser();
            if (price.Any(x=>x.genderprice == model.genderprice))
            {
                ModelState.AddModelError("genderprice", "هذا الاسم مضاف مسبقا");
                return View(model);
            }
            var savemodel = new Price
            {
                genderprice = model.genderprice,
                ThePrice = model.ThePrice,
                Time = model.Time
            };
            await price.AddAsync(savemodel);
            toast.AddSuccessToastMessage("Done Create");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || !GetUser().Admin)
                return NoContent();

            GetUser();
            var model = await price.FindAsync(id);
            var Modelview = new PriceView
            {
                Id = model.Id,genderprice = model.genderprice ,
                ThePrice = model.ThePrice , IsCreate = false,
                Time = model.Time
            };
            return View("Create" , Modelview);

        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PriceView model)
        {
            GetUser();
            var savemodel = new Price
            {
                Id = model.Id , genderprice = model.genderprice , 
                ThePrice = model.ThePrice,
                Time = model.Time
            };
            await price.UpdateAsync(savemodel);
            toast.AddSuccessToastMessage("Done Edit");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();
            
            if(Reveal.Any(x=>x.Idprice == id.Value))
            {
                return NotFound("لا يمكن حذف هذا السعر ولكن يمكن التعديل");
            }

            price.Delete(id);
            return Ok("Delete done");
        }

    }
}
