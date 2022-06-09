using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;
using Doctor_Management.IRepository;
using Doctor_Management.Models_View;
using NToastNotify;
using System.Data.OleDb;
using System.IO;
using Newtonsoft.Json;

namespace Doctor_Management.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IRepositoryData<Customer> customer;
        private readonly IToastNotification _toast;
        private readonly List<string> Ex = new List<string>() { ".xlsx", ".json" };
        public CustomerController(IRepositoryData<Customer> customer ,IToastNotification toast )
        {
            this.customer = customer;
            this._toast = toast;
            
        }
        private Users GetUser()
        {
            Users users = new Users(HttpContext);
            ViewBag.user = users.User;
            ViewBag.login = users.Login;
            ViewBag.admin = users.Admin;
            return users;
        }
        public async Task<IActionResult> Index()
        {
            if (!GetUser().Login)
                return NotFound();

            return View(await customer.GetAllAsync());
        }

        public IActionResult Create()
        {
            if (!GetUser().Login)
                return NotFound();
            GetUser();
            var modelcreate = new CustomerView { create = true };
            return View("create_edit",modelcreate);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerView model)
        {
            GetUser();
            model.create = true;
            if(customer.Any(x=>x.NameCustomer == model.NameCustomer))
            {
                ModelState.AddModelError("NameCustomer", "هذا الاسم مضاف مسبقا");
                return View("create_edit", model);
            }
            if(model.dateBirth.Year < 1910 || model.dateBirth.Year > DateTime.Now.Year)
            {
                ModelState.AddModelError("dateBirth", "برجاء ادخل تاريخ الميلاد بشكل صحيح");
                return View("create_edit", model);
            }
            var savedmodel = new Customer
            {
                NameCustomer = model.NameCustomer,
                Blood = model.Blood,
                dateBirth = model.dateBirth,
                Gender = model.Gender,
                Phones = model.Phones
            };
            await customer.AddAsync(savedmodel);
            _toast.AddSuccessToastMessage($"تم اضافــة العميل : {model.NameCustomer} بنجاح");

            return View("create_edit" , new CustomerView { create = true });
        }

        [HttpPost]
        public JsonResult exsitsname(string Name)
        {
            return Json(customer.Any(x => x.NameCustomer == Name));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            GetUser();
            if (id == null)
                return NotFound();

            var model = await customer.FindAsync(id);
            var editmodel = new CustomerView
            {
                ID = model.ID,
                NameCustomer = model.NameCustomer , Blood = model.Blood,
                create = false,dateBirth = model.dateBirth,Gender = model.Gender,Phones = model.Phones
            };
            return View("create_edit",editmodel);
        }

        [HttpPost ,ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerView model)
        {
            GetUser();
            model.create = false;
            if (model.dateBirth.Year < 1910 || model.dateBirth.Year > DateTime.Now.Year)
            {
                ModelState.AddModelError("dateBirth", "برجاء ادخل تاريخ الميلاد بشكل صحيح");
                return View("create_edit", model);
            }
            var savedmodel = new Customer
            {
                ID = model.ID,
                NameCustomer = model.NameCustomer,
                Blood = model.Blood,
                dateBirth = model.dateBirth,
                Gender = model.Gender,
                Phones = model.Phones

            };
            await customer.UpdateAsync(savedmodel);
            _toast.AddSuccessToastMessage($"تم تعديل العميل : {model.NameCustomer} بنجاح");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            customer.Delete(id);
            return Ok();
        }

        public IActionResult addfile()
        {
            if (GetUser().Login)
                return View();

            return NoContent();
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> addfile(FilesCustomersView model)
        {
            GetUser();
            var File = Request.Form.Files;
            if (File.Any())
            {
                var customers = new List<Customer>();
                var Thefile = File.FirstOrDefault();
                if(!Ex.Contains(Path.GetExtension(Thefile.FileName)))
                {
                    ModelState.AddModelError("Filesup", "الملف المدخل ليس ملف اكسيل او جيسون");
                    _toast.AddErrorToastMessage("الملف المدخل غير صحيح");
                    return View(model);
                }
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using var stream = new MemoryStream(Thefile.setimage());
                if(Path.GetExtension(Thefile.FileName) == ".xlsx")
                {
                    using (var exlread = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
                    {
                        while (exlread.Read())
                        {
                            try
                            {
                                var Name = exlread.GetValue(0).ToString();
                                var Phones = exlread.GetValue(1).ToString();
                                var date = Convert.ToDateTime(exlread.GetValue(2));
                                var Gender = exlread.GetValue(3).ToString();
                                var Blood = exlread.GetValue(4).ToString();
                                if (!customer.Any(x => x.NameCustomer == Name))
                                {
                                    customers.Add(new Customer
                                    {
                                        NameCustomer = Name,
                                        Phones = Phones,
                                        dateBirth = date,
                                        Gender = (Gender.ToLower().Contains("ذ") || Gender.ToLower().Contains("m") ? "M" : "F"),
                                        Blood = Blood
                                    });
                                }
                            }
                            catch (Exception ex)
                            {
                                _toast.AddErrorToastMessage(ex.Message);
                                return View(model);
                            }
                        }
                        if (customers.Count > 0)
                        {
                             await customer.AddAsync(customers);
                            _toast.AddSuccessToastMessage("Done Add data");
                        }
                        else
                        {
                            _toast.AddErrorToastMessage("هذة الاسماء مضافة مسبقا");
                            return View(model);
                        }
                    }
                }
                else if(Path.GetExtension(Thefile.FileName) == ".json")
                {
                    using (var jsonfile = new StreamReader(stream))
                    {
                        var JsonData = JsonConvert.DeserializeObject<IEnumerable<CustomerJson>>(jsonfile.ReadToEnd());
                        foreach (var item in JsonData)
                        {
                            try
                            {
                                if (!customer.Any(x => x.NameCustomer == item.Name))
                                {
                                    customers.Add(new Customer
                                    {
                                        NameCustomer = item.Name,
                                        Phones = item.Phone,
                                        dateBirth = Convert.ToDateTime(item.date),
                                        Gender = (item.Gender.ToLower().Contains("ذ") || item.Gender.Contains("m") ? "M" : "F"),
                                        Blood = item.Blood
                                    });
                                }
                            }
                            catch (Exception ex)
                            {
                                _toast.AddErrorToastMessage(ex.Message);
                                return View();
                            }
                        }
                        if (customers.Count > 0)
                        {
                            await customer.AddAsync(customers);
                            _toast.AddSuccessToastMessage("Done Save Date");
                        }
                        else
                        {
                            _toast.AddErrorToastMessage("هذة الاسماء مضافة مسبقا");
                            return View(model);
                        }
                    }
                }
            }
       
            return RedirectToAction("Index");
        }
    }
}
