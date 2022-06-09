using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.IRepository;
using Doctor_Management.Models;
using Doctor_Management.Models_View;
using NToastNotify;
using Newtonsoft.Json;
using System.IO;

namespace Doctor_Management.Controllers
{
    public class MedicController : Controller
    {
        private readonly IToastNotification toast;
        private readonly IRepositoryData<MedicName> medic;
        private readonly List<string> ex = new List<string> { ".xlsx", ".json" };
        public MedicController(IToastNotification toast,IRepositoryData<MedicName> medic)
        {
            this.toast = toast;
            this.medic = medic;
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
            if (!GetUser().Login)
                return NotFound();

            GetUser();
            return View(medic.GetAll());
        }

        public IActionResult Create()
        {
            if (!GetUser().Login)
                return NotFound();

            GetUser();
            return View(new Medicview { IsCreate = true });
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Medicview model)
        {
            GetUser();
            if (medic.Any(x=>x.NameMedic.ToLower() == model.NameMedic.ToLower()))
            {
                model.IsCreate = true;
                ModelState.AddModelError("NameMedic", "هذا الاسم مسجل");
                toast.AddErrorToastMessage("هذا الاسم مسجل");
                return View(model);
            }

            var savemodel = new MedicName
            {
                NameMedic = model.NameMedic
            };
            await medic.AddAsync(savemodel);
            toast.AddSuccessToastMessage($"Done Add medic {model.NameMedic}");
            return View(new Medicview { IsCreate = true });
        }

        public IActionResult Edit(int? id)
        {
            GetUser();
            if (id == null || !GetUser().Login)
                return NoContent();

            var model = medic.Find(id);
            var viewmodel = new Medicview
            {
                Id = model.Id, NameMedic = model.NameMedic,
                IsCreate = false
            };
            return View("Create", viewmodel);
        }

        [HttpPost ,ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Medicview model)
        {
            GetUser();
            var savemodel = new MedicName
            {
                Id = model.Id,
                NameMedic = model.NameMedic,
            };
            await medic.UpdateAsync(savemodel);
            toast.AddSuccessToastMessage("Done Edit");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public JsonResult NameExists(string Name)
        {
            return Json(medic.Any(x => x.NameMedic.ToLower() == Name.ToLower()));
        }


        public IActionResult addfile()
        {
            GetUser();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> addfile(FilesCustomersView model)
        {
            GetUser();
            var File = Request.Form.Files;
            if (File.Any())
            {
                var lis = new List<MedicName>();
                var Filedata = File.FirstOrDefault();
                if (!ex.Contains(Path.GetExtension(Filedata.FileName).ToLower()))
                {
                    toast.AddErrorToastMessage("الملف المدخل غير صالح يجب ان يكون اكسيل او جيسون");
                    return View();
                }
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using var Stream = new MemoryStream(Filedata.setimage());
                if (Path.GetExtension(Filedata.FileName) == ".xlsx")
                {
                    using (var Read = ExcelDataReader.ExcelReaderFactory.CreateReader(Stream))
                    {
                        while (Read.Read())
                        {
                            try
                            {
                                var Name = Read.GetValue(0).ToString();
                                if (!medic.Any(x => x.NameMedic.ToLower() == Name.ToLower()))
                                {
                                    lis.Add(new MedicName { NameMedic = Name });
                                }
                            }
                            catch (Exception ex)
                            {
                                toast.AddErrorToastMessage(ex.Message);
                                return View();
                            }
                        }
                    }
                    if (lis.Count > 0)
                    {
                        await medic.AddAsync(lis);
                        toast.AddSuccessToastMessage("Done Add");
                        return View();
                    }
                    else
                    {
                        toast.AddErrorToastMessage("الاسماء مسجلة بالفعل ولا داعى لستجيلها");
                        return View();
                    }
                }
                else if (Path.GetFileName(Filedata.FileName) == ".json")
                {
                    using var Reading = new StreamReader(Stream);
                    var Json = JsonConvert.DeserializeObject<List<jsonName>>(Reading.ReadToEnd());
                    foreach (var item in Json)
                    {
                        try
                        {
                            if (!medic.Any(x => x.NameMedic.ToLower() == item.Name.ToLower()))
                            {
                                lis.Add(new MedicName { NameMedic = item.Name });
                            }
                        }
                        catch (Exception ex)
                        {
                            toast.AddErrorToastMessage(ex.Message);
                            return View();
                        }
                    }
                    if (lis.Count > 0)
                    {
                        await medic.AddAsync(lis);
                        toast.AddSuccessToastMessage("Done Add");
                        return View();
                    }
                    else
                    {
                        toast.AddErrorToastMessage("الاسماء مسجلة بالفعل ولا داعى لستجيلها");
                        return View();
                    }

                }

            }
            return View();
        }

        class jsonName
        {
            public string Name { get; set; }
        }
    }
}
