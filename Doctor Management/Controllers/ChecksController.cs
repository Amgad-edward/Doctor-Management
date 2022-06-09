using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.IRepository;
using Doctor_Management.Models;
using Doctor_Management.Models_View;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Doctor_Management.Controllers
{
    public class ChecksController : Controller
    {
        private readonly IRepositoryData<ItemCheckup> check;
        private readonly IRepositoryData<Customer> customer;
        private readonly IRepositoryData<Reveal> reveal;
        private readonly IRepositoryData<NamesCheck> names;
        private readonly IRepositoryData<ResultNormal> normals;
        private readonly IToastNotification toast;
        private readonly List<string> Extension = new List<string> { ".jpg", ".png" };
        public ChecksController(IRepositoryData<ItemCheckup> check , IRepositoryData<NamesCheck> names, IRepositoryData<ResultNormal> normals, IRepositoryData<Customer> Customer,IRepositoryData<Reveal> reveal , IToastNotification Toast )
        {
            this.check = check;
            this.toast = Toast;
            this.customer = Customer;
            this.reveal = reveal;
            this.normals = normals;
            this.names = names;
        }

        public IActionResult Goback(int id)
        {
            GetUser();
            return Redirect($"/Home/ShowProfile/{id}");
        }
        private Users GetUser()
        {
            Users users = new Users(HttpContext);
            ViewBag.user = users.User;
            ViewBag.login = users.Login;
            ViewBag.admin = users.Admin;
            return users;
        }
        public async Task<IActionResult> Index(int? id)
        {
            GetUser();
            if (!GetUser().Login)
                return Redirect("/Home/Index");

            if (id == null)
                return BadRequest();

            var model = await check.GetAsync(x => x.Idreveal == id.Value);
            if (model.ToList().Count == 0)
                return Redirect("/Home/Privacy");
            ViewBag.gender = customer.Find(model.ToList()[0].Idcustomer).Gender;
            var Models = new CheckUPView
            {
                ItemCheckups = model.ToList(),
                customerName = customer.Find(model.ToList()[0].Idcustomer).NameCustomer,
                date = reveal.Find(id).DateReservation,
                ResultNormal = normals.GetAll().ToList()
            };
            GetUser();
            return View(Models);
        }

        public IActionResult AddnameList()
        {
            if (!GetUser().Login)
                return NotFound();

            GetUser();
            ViewBag.table = names.GetAll().ToList();
            return View();
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> AddList(NamesCheck model)
        {
            GetUser();
            if (names.Any(x => x.TitelName.ToLower() == model.TitelName.ToLower()))
            {
                ModelState.AddModelError("TitelName", "اسم هذة القائمة مضافة مسبقا");
                ViewBag.table = names.GetAll().ToList();
                return View("AddnameList", model);
            }
            var str = model.Nameschiled.Split(',');
            if (str.Length <= 1)
            {
                ModelState.AddModelError("Nameschiled", "يجب ان تحتوى القائمة على اكثر من اسم تحليل ويجب الفصل بهم بعلامة (,)");
                ViewBag.table = names.GetAll().ToList();
                return View("AddnameList", model);
            }

            await names.AddAsync(model);
            toast.AddSuccessToastMessage($"Done Add {model.TitelName}");
            return RedirectToAction(nameof(AddnameList));
               
        }

        [HttpGet]
        public IActionResult deleteblist(int id)
        {
            if (!GetUser().Login)
                return NotFound();

            names.Delete(id);
            return Ok();
        }
       

        public async Task<IActionResult> Create(int? id)
        {
            GetUser();
            if (id == null)
                return NotFound();

            var model = await reveal.FindAsync(id);

            var Models = new ItemsCheckView
            {
                Idcustomer = model.Idcustomer,
                Idreveal = model.ID,
                create = true
            };
            ViewBag.Name = customer.Find(model.Idcustomer).NameCustomer;
            return View("Edit",Models);
        }

        [HttpGet]
        public async Task<IActionResult> Average(int ? id)
        {
            if (!GetUser().Admin)
                return NotFound();
            GetUser();
            var gender = customer.Find(id).Gender;
            var model = await check.GetAsync(x => x.Idcustomer == id);
            var lis = new List<Avg_View>();
            var modeldis = model.Select(x=>new { x.NameCheck }).Distinct().ToList();
            foreach (var item in modeldis)
            {
                if(model.Where(x=>x.NameCheck.ToLower() == item.NameCheck.ToLower() && x.Resulte > 0).ToList().Count > 1)
                {
                    double? n = normals.Get(x => x.Name.ToLower() == item.NameCheck.ToLower() && (x.Gender.ToLower() == gender.ToLower() || x.Gender.ToLower().Contains(gender.ToLower()))).FirstOrDefault().Normal;
                    var Avr = model.Where(x => x.NameCheck.ToLower() == item.NameCheck.ToLower()  && x.Resulte > 0).Average(x => x.Resulte);
                    lis.Add(new Avg_View { Name = item.NameCheck, Results = Avr,normals = n });
                }
            }
            var average = new AverageView
            {
                Avg_Views = lis,Name= customer.Find(id).NameCustomer
            };
            return View(average);
        }


        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemsCheckView Model)
        {
            GetUser();
            var file = Request.Form.Files;
            if (file.Any())
            {
                if (file.setimage().Length > Hex.MaxAllowImage)
                {
                    ModelState.AddModelError("ImageReport", "حجم الصورة لا يجب ان تزيد عن 2ميجا بايت");
                    return View("Edit",Model);
                }
                if (!Extension.Contains(Path.GetExtension(file[0].FileName).ToLower()))
                {
                    ModelState.AddModelError("ImageReport", "يسمح بتحميل الصور فقط من امتداد JPG Or PNG");
                    return View("Edit", Model);
                }
                Model.ImageReport = file.setimage();
            }
            var Modelsave = new ItemCheckup
            {
                Idcustomer = Model.Idcustomer,
                Idreveal = Model.Idreveal,
                ImageReport = Model.ImageReport,
                NameCheck = Model.NameCheck,
                Resulte = Model.Resulte,

            };
            await check.AddAsync(Modelsave);
            toast.AddSuccessToastMessage("تم الاضافة بنجاح");
            return Redirect($"/Checks/Index/{Model.Idreveal}");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            GetUser();
            if (id == null)
                return NotFound();

            var model = await check.FindAsync(id);
            var Models = new ItemsCheckView
            {
                Id = model.Id, NameCheck = model.NameCheck,
                Idcustomer = model.Idcustomer, Idreveal = model.Idreveal,
                ImageReport = model.ImageReport,Resulte = model.Resulte,
                create = false
            };
            ViewBag.Name = customer.Find(model.Idcustomer).NameCustomer;
            return View(Models);
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ItemsCheckView Model)
        {
            GetUser();
            var file = Request.Form.Files;
            if (file.Any())
            {
                if(file.setimage().Length  > Hex.MaxAllowImage)
                {
                    ModelState.AddModelError("ImageReport", "حجم الصورة لا يجب ان تزيد عن 2ميجا بايت");
                    toast.AddErrorToastMessage("لم يتم التعديل");
                    return View(Model);
                }
                if (!Extension.Contains(Path.GetExtension(file[0].FileName).ToLower()))
                {
                    ModelState.AddModelError("ImageReport", "يسمح بتحميل الصور فقط من امتداد JPG Or PNG");
                    toast.AddErrorToastMessage("لم يتم التعديل");
                    return View(Model);
                }
                Model.ImageReport = file.setimage();
            }
            var Modelsave = new ItemCheckup
            {
                Id = Model.Id,Idcustomer = Model.Idcustomer,Idreveal = Model.Idreveal,
                ImageReport = Model.ImageReport,NameCheck = Model.NameCheck,Resulte = Model.Resulte,
                
            };
            await check.UpdateAsync(Modelsave);
            toast.AddSuccessToastMessage("تم تعديل بنجاح");
            return Redirect($"/Checks/Index/{Model.Idreveal}");
        }

        [HttpGet]
        public IActionResult delete(int? id)
        {
            if (!GetUser().Admin)
            {
                toast.AddErrorToastMessage("غير مسموح لهذا المستخدم الحذف");
                return BadRequest();
            }
            if(id == null)
            {
                return BadRequest();
            }
            check.Delete(id);
            return Ok();
        }

        [HttpGet]
        public IActionResult Normals(int? id)
        {
            GetUser();
            var Nor_Enter = normals.GetAll();      
            var N = names.GetAll().Select(x => new { x.Id, x.TitelName });
            var Listname = new List<SelectListItem>();
            foreach (var n in N)
                Listname.Add(new SelectListItem { Value = n.Id.ToString(), Text = n.TitelName });
            var Results = new List<ResultNormal>();
            if (id.HasValue)
            {
                var list = names.Find(id);
                var Names = list.Nameschiled.Split(',');
                foreach (var item in Names)
                {
                    Results.Add(new ResultNormal
                    {
                        Name = item,
                        Normal = 0
                    }) ;
                }

            }

            var model = new NormalsView
            {
                ListNames = Listname,Results = Results,GetAlls = normals.GetAll().ToList()
            };
            return View(model);
        }

        [HttpPost , ValidateAntiForgeryToken]
        public IActionResult Normals(NormalsView model)
        {
            GetUser();
            if (model.ID.HasValue)
                return Redirect($"/checks/Normals/{model.ID}");

            return View();
        }

        [HttpGet]
        public IActionResult del_Normal(int? id)
        {
            normals.Delete(id);
            return Ok();
        }


        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> AddListNormal(NormalsView model)
        {
            GetUser();
            foreach (var item in model.Results)
            {
                if(normals.Any(x=>x.Name.ToLower() == item.Name.ToLower() && x.Gender.ToLower() == item.Gender.ToLower()))
                {
                    toast.AddErrorToastMessage("this Name list is arely Adding !!!!");
                    model.GetAlls = normals.GetAll().ToList();
                    return View("Normals", model);
                }
            }
            await  normals.AddAsync(model.Results);
            toast.AddSuccessToastMessage("Done Aollwed");
            return RedirectToAction(nameof(Normals));
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOne(NormalsView model)
        {
            GetUser();
            if (normals.Any(x=>x.Name.ToLower() == model.Resul.Name.ToLower() && x.Gender.ToLower() == model.Resul.Gender.ToLower()))
            {
                if (model.IdNor.HasValue)
                {
                    var edit = normals.Find(model.IdNor);
                    edit.Name = model.Resul.Name;
                    edit.Normal = model.Resul.Normal;
                    edit.Gender = model.Resul.Gender;
                    await normals.UpdateAsync(edit);
                    model.GetAlls = normals.GetAll().ToList();
                    toast.AddSuccessToastMessage("Done update");
                    return RedirectToAction(nameof(Normals));
                }
                toast.AddErrorToastMessage("this Name list is arely Adding !!!!");
                return View("Normals", model);
            }
            await normals.AddAsync(model.Resul);
            toast.AddSuccessToastMessage("Done Aollwed");
            return RedirectToAction(nameof(Normals));
        }

        [HttpGet]
        public IActionResult ShowCustomerCheck(int? id)
        {
            if (!GetUser().Login)
                return NotFound();

            if (id == null)
                return NotFound();

            GetUser();
            var model = check.GetClude().Include(c=>c.customer).Include(R=>R.reveal).Where(x=>x.Idcustomer == id).ToList();
            ViewBag.ids = reveal.Get(x => x.Idcustomer == id).Select(x => x.ID).ToList();
            ViewBag.normal = normals.GetAll();
            return View(model);
        }

        [HttpGet]
        public IActionResult EditNormal(int? id)//id NormalResulte
        {
            GetUser();
            if (id == null)
                return NoContent();
            var normalmodel = normals.Find(id);
            return View(normalmodel);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public IActionResult EditNormal(ResultNormal Model)
        {
            if (!ModelState.IsValid)
                return NoContent();

            normals.Update(Model);
            return RedirectToAction(nameof(Normals));
        }
    }
}
