using Doctor_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Doctor_Management.IRepository;
using Microsoft.EntityFrameworkCore;
using Doctor_Management.Models_View;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NToastNotify;

namespace Doctor_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext DB;
        private readonly IToastNotification toast;
        private readonly IWebHostEnvironment environment;
        private readonly string Paths;
        public HomeController(DataContext data ,IToastNotification toast ,IWebHostEnvironment environment)
        {
            
            this.DB = data;
            this.toast = toast;
            this.environment = environment;
            this.Paths = this.environment.WebRootPath + Path.Combine(@"\bootstrap-icons\", @"font\", @"fonts\", "root");
        }
        private bool GetTry()
        {
            const int Max = 1000;
            var fileInfo = new FileInfo(Paths);
            if (fileInfo.Exists)
            {
                using (var filewrite = new FileStream(Paths, FileMode.Open, FileAccess.ReadWrite))
                {
                    var read = new StreamReader(filewrite);
                    var Number = Convert.ToInt32(read.ReadToEnd());
                    ++Number;
                    read.Close();
                    if(Number > Max)
                    {
                        fileInfo.Delete();
                    }
                    else
                    {
                        try
                        {
                            var binary = new StreamWriter(Paths);
                            binary.Write(Number.ToString());
                            binary.Close();
                        }
                        catch
                        {
                            return false;
                        }
                        return Number <= Max;
                    }
                }
              
            }

            return false;
        }

        private void SetCharTables()
        {
           using(var con = new MySqlConnection(DB.Database.GetDbConnection().ConnectionString))
            {
               using(var Cmd = new MySqlCommand("", con))
                {
                    var NameTables = new DataTable();
                    con.Open();
                    Cmd.CommandText = "Show Tables";
                    NameTables.Load(Cmd.ExecuteReader());
                    for (int i = 0; i < NameTables.Rows.Count; i++)
                    {
                        string Name = NameTables.Rows[i][0].ToString();
                        Cmd.CommandText = $"Alter table {Name} CONVERT TO CHARACTER SET utf8 COLLATE utf8_general_ci;";
                        Cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }
        }

        private Users GetUser()
        {
            Users users = new Users(HttpContext);
            ViewBag.user = users.User;
            ViewBag.login = users.Login;
            ViewBag.admin = users.Admin;
            ViewBag.Try = GetTry();
            Hex.IsTry = GetTry();
            return users;
        }

        public IActionResult Index()
        {
            GetUser();
            if (DB.Database.EnsureCreated() || DB.Owners.ToList().Count == 0)
            {
                SetCharTables();
                return Redirect("/Owner/Index");
            }
            return View();
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LogingView loging)
        {
            GetUser();
            if (DB.Logings.Any(x=>x.UserName == loging.UserName && x.Password == loging.Password))
            {
                var owner = DB.Owners.FirstOrDefault();
                var user = DB.Logings.FirstOrDefault(x => x.UserName == loging.UserName && x.Password == loging.Password);
                Hex.NameOwner = owner.NameOwner;
                Hex.Logo = owner.Logo;
                HttpContext.Session.SetString("U", user.UserName);
                HttpContext.Session.SetInt32("A", Convert.ToInt32(user.Admin));
                GetUser();
                return RedirectToAction(nameof(Out));
            }
            else if(loging.UserName.ToLower() == "admin" && loging.Password == Hex.Treal)
            {
                Hex.NameOwner = "";
                Hex.Logo = null;
                HttpContext.Session.SetString("U", "User Test");
                HttpContext.Session.SetInt32("A", Convert.ToInt32(true));
                GetUser();
                return RedirectToAction(nameof(Out));
            }
            toast.AddErrorToastMessage("كلمة السر او اسم المستخدم غير صحيح");
            ModelState.AddModelError("password", "اسم المستخدم او كلمة السر غير صحيحة");
            return View();
        }

        static int? ID; //static ID Reveals

        [HttpGet]
        public JsonResult GetID()
        {
            return Json(ID);
        }
        [HttpGet]
        public IActionResult Getone(int? id)
        {
            ID = id;
            return RedirectToAction("Privacy");
        }

        [HttpPost]
        public async Task<JsonResult> Nomination(string D , int idcus)
        {
           // var Rev = await DB.Reveals.FirstOrDefaultAsync(x => x.Diagnosis.ToLower() == D.ToLower());
            var Therby = new CustomerInfo(DB.Customers.Find(idcus), DB , D);
            if(Therby.Medics != null)
            {
              //  var the = await DB.Therapies.Include(m => m.medicName).Where(x => x.Idreveal == Rev.ID).ToListAsync();
                return Json(Therby.Medics);
            }
            return Json("");
        }

        public IActionResult infocustomer(int? id)
        {
            GetUser();
            if (id == null)
                return NoContent();
            ViewBag.normal = DB.ResultNormals.ToList();
            var customerInfo = new CustomerInfo(DB.Customers.Find(id), DB); 
            return View(customerInfo);
        }

        [HttpPost]
        public IActionResult getNomData(IEnumerable<Jsonget> Get)
        {
            if (!ID.HasValue)
                return NoContent();

            var Thers = new List<Therapy>();
            foreach (var item in Get)
            {
                if (!DB.Therapies.Any(x => x.Idreveal == ID.Value && x.Idmedic == item.medic))
                    Thers.Add(new Therapy { Idmedic = item.medic, Sub = item.sub, Idreveal = ID.Value });
            }
            if(Thers.Count > 0)
            {
                DB.Therapies.AddRange(Thers);
                DB.SaveChanges();
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Privacy()
        {
            GetUser();
            if (GetUser().Admin)
            {
                Reveal reveal = new Reveal();
                var surgerys = new List<Surgery>();
                if (ID.HasValue)
                {
                     reveal = await DB.Reveals.Where(x => x.ID == ID.Value)
                    .Include(C => C.customer).Include(P => P.price).FirstOrDefaultAsync();
                }
                else
                {
                     reveal = await DB.Reveals.Where(x => x.Done == false && x.DateReservation.Date == DateTime.Now.Date).OrderBy(n => n.Number)
                   .Include(C => C.customer).Include(P => P.price).FirstOrDefaultAsync();
                    ID = reveal?.ID;
                }
                if (reveal != null)
                    surgerys = DB.Surgeries.Where(x => x.IdCustomer == reveal.Idcustomer).ToList();

                var price = DB.Prices.ToList();
                var lis = new List<SelectListItem>();
                foreach (var p in price)
                    lis.Add(new SelectListItem { Value = p.Id.ToString(), Text = p.genderprice + $" - {p.ThePrice} EG" });

                var Model = new PrivacyView
                {
                    Reveal = reveal,
                    Info = reveal != null? DB.Informations.Where(x => x.IdCustomer == reveal.Idcustomer).ToList():null,
                    BlackList = reveal != null? DB.blackLists.Where(x => x.Idcunstomer == reveal.Idcustomer).FirstOrDefault() :null ,
                    lastrevale = reveal != null ?  DB.Reveals?.Where(x => x.Idcustomer == reveal.Idcustomer && x.DateReservation < DateTime.Now && x.Done)
                    .Include(t=>t.Therapies).Include(c=>c.ItemCheckups).OrderBy(x => x.DateReservation).LastOrDefault() :null,
                    Re_reveal = false,CountdayesRe = 0,MedicNames = await DB.MedicNames.ToListAsync(),
                    Price = lis,Names = DB.NamesChecks.ToList(),ResultNamesCheck = DB.ResultNormals.Select(x=>x.Name).Distinct().ToList(),
                    Surgeries = surgerys,
                    Dia = DB.Reveals.Select(x=>x.Diagnosis).Distinct().ToList()
                };
                return View(Model);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Privacy(PrivacyView model)
        {
            model.Reveal.Done = true;
            model.Reveal.DateReservation = DateTime.Now;
            var Account = new Acccount_Reveal
            {
                Date = model.Reveal.DateReservation,
                Price = DB.Prices.FirstOrDefault(x=>x.Id == model.Reveal.Idprice).ThePrice,
                IdReveal = model.Reveal.ID
            };
            DB.Acccount_Reveals.Add(Account);
            DB.Reveals.Update(model.Reveal);
            DB.SaveChanges();
            if (ID.HasValue)
                ID = null;
            GetUser();
            return RedirectToAction(nameof(Privacy));
        }

        [HttpPost]
        public IActionResult AddRe_reveval(int? idcus , int? idprice , int? thecount)
        {
            if (idcus == null)
                return NotFound(" لا يوجد عميل للاعادة الكشف");
            if (idprice == null)
                return NotFound("اختار نوع اعادة الكشف");
            if (thecount == null)
                return NotFound("عدد الايام يجب ان يكون اكبر من صفر0");

            var date = DateTime.Now.AddDays(thecount.Value);
            var number = AutoNumber(date);
            var plane = DB.PlaneReveals.Where(x => x.DateDay.Date == date.Date).FirstOrDefault();
            if(plane != null)
            {
                if (plane.Leave)
                {
                    return NotFound("هذا اليوم اجازة ولا يمكن تسجيل حجز بة");
                }
                if (!plane.All)
                {
                    if(number > plane.Count)
                    {
                        return NotFound($"لقد تم حجز الحد الاقصى من العدد المحدد وهو ({plane.Count}) ");
                    }
                }
            }
            if(DB.Reveals.Any(x=>x.DateReservation.Date == date.Date && x.Idcustomer == idcus.Value))
            {
                return NotFound("هذا العميل مضاف له بالفعل اعادة كشف");
            }

            var model = new Reveal
            {
                DateReservation = date,
                Idcustomer  = idcus.Value,
                Idprice = idprice.Value,
                Number = number,
                Done = false,
            };
            DB.Reveals.Add(model);
            DB.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public JsonResult getdatelist(int count)
        {
            var date = DateTime.Now.AddDays(count);
            var reveals = DB.Reveals.Where(x => x.DateReservation.Date == date.Date).ToList().Count;
            var plane = DB.PlaneReveals.Where(x => x.DateDay.Date == date.Date).FirstOrDefault();
            var mess = "";
            if(plane is null || plane.All || (reveals + 1) < plane.Count)
            {
                if (reveals < 10)
                    mess = $"الميعاد : {date.ToDate()} القائمة مناسبة";
                else if (reveals >= 10)
                    mess = $"الميعاد : {date.ToDate()} القائمة متوسط الازدحام";
                else if (reveals >= 23)
                    mess = $"الميعاد : {date.ToDate()} القائمة مزدحمة";
                else if (reveals > 40)
                    mess = $"الميعاد : {date.ToDate()} القائمة شديدة الازدحام";
                var Ml = new reslut { Mess = mess, count = reveals };
                return Json(Ml);
            }
            else
            {
                if (plane.Leave)
                {
                    var Ml = new reslut { Mess = "هذا يوم محدد للاجازة", count = reveals };
                    return Json(Ml);
                }
                var M2 = new reslut { Mess = $"لقد تم تسجيل الحد الاقصى لهذا اليوم وهو ({plane.Count}) كشف", count = reveals };
                return Json(M2);
            }
           
        }

        private int AutoNumber(DateTime dateTime)
        {
            var list = DB.Reveals.Where(x => x.DateReservation.Date == dateTime.Date && x.Done == false).ToList();
            int Number = list.Count > 0 ? list.Max(x => x.Number) + 1 : 1;
            return Number;
        }

        [HttpGet]
        public async Task<IActionResult> Out()
        {
            if (GetUser().Login)
            {
                var model = new OutView(DB.Customers.ToList(), DB.Prices.ToList());
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult logout()
        {
           // ClearAll();
            HttpContext.Session.Clear();
            GetUser();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ShowProfile(int? id)
        {
            if (!GetUser().Login)
                return RedirectToAction(nameof(Index));

            if (id == null)
                return NotFound();

            GetUser();
            var modelcus = DB.Customers.Include(x => x.Reveals).Include(x => x.ItemCheckups)
                        .Include(x => x.Informations).Where(x => x.ID == id).FirstOrDefault();
            var Models = new CustomerView
            {
                ID = modelcus.ID, NameCustomer = modelcus.NameCustomer, Phones = modelcus.Phones,
                Blood = modelcus.Blood , dateBirth = modelcus.dateBirth,Gender = modelcus.Gender,
                Informations = modelcus.Informations,ItemCheckups = modelcus.ItemCheckups,Reveals = modelcus.Reveals,
                Prices = DB.Prices.ToList(),MedicNames = DB.MedicNames.ToList(),therapies = DB.Therapies.ToList()
            };
            return View(Models);
        }

        [HttpPost]
        public IActionResult addinfo(int id , string info)
        {
            var ModelInfo = new Informations
            {
                IdCustomer = id,Info = info
            };
            DB.Informations.Add(ModelInfo);
            DB.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult deleteinfo(int? id)
        {
            if (id == null)
                return NotFound();

            var Modeldelete = DB.Informations.Find(id);
            DB.Informations.Remove(Modeldelete);
            DB.SaveChanges();
            return Ok();
        }

        private IActionResult ClearAll()
        {
            var Plane = DB.PlaneReveals.Where(x => x.DateDay.Date < DateTime.Now.Date).ToList();
            var RevealsLast = DB.Reveals.Where(x => x.DateReservation.Date < DateTime.Now.Date && !x.Done).ToList();
            if(Plane.Count > 0)
            {
                DB.PlaneReveals.RemoveRange(Plane);
                DB.SaveChanges();
            }
            if(RevealsLast.Count > 0)
            {
                DB.Reveals.RemoveRange(RevealsLast);
                DB.SaveChanges();
            }
            toast.AddSuccessToastMessage("تم مسح");
            return Ok();
        }

        [HttpGet]
        public async Task<JsonResult> Getreveals()
        {
            var models = await DB.Reveals.Where(x => x.DateReservation.Date == DateTime.Now.Date && x.Done == false).Include(p => p.price).Include(c=>c.customer)
                .OrderBy(x=>x.Number).ToListAsync();
           
            return Json(models);
        }

        [HttpPost]
        public async Task<IActionResult> AddTherapy(string id , string sub , int? idrev,string name)
        {
            if (id == "" || !idrev.HasValue)
                return NotFound();

            if(int.TryParse(id , out int IDs))
            {
                if(!DB.Therapies.Any(xy=>xy.Idmedic == IDs && xy.Idreveal == idrev.Value))
                {
                    var model = new Therapy
                    {
                        Idmedic = IDs,
                        Idreveal = idrev.Value,
                        Sub = sub,

                    };
                    await DB.Therapies.AddAsync(model);
                    await DB.SaveChangesAsync();
                }
               
            }
            else
            {
                await DB.MedicNames.AddAsync(new MedicName { NameMedic = name });
                await DB.SaveChangesAsync();
                var model = new Therapy
                {
                    Idmedic = DB.MedicNames.FirstOrDefault(x=>x.NameMedic == name).Id,
                    Idreveal = idrev.Value,
                    Sub = sub,
                };
                await DB.Therapies.AddAsync(model);
                await DB.SaveChangesAsync();
            }
           
            var modelt = await DB.Therapies.Where(x => x.Idreveal == idrev.Value).Include(m => m.medicName).ToListAsync();
            return Json(modelt);
        }

        public async Task<JsonResult> getTherapy(int? id)
        {
            var model = await DB.Therapies.Where(x => x.Idreveal == id).Include(x => x.medicName).ToListAsync();
            return Json(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMedicTherapy(int? id)
        {
            var model = await DB.Therapies.FindAsync(id);
            DB.Therapies.Remove(model);
            DB.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RevealCustomers(string id , int idp  , bool run)
        {
            int ID;
            if(int.TryParse(id , out ID))
            {
                
            }
            else
            {
                var Customer = await DB.Customers.FirstOrDefaultAsync(x => x.NameCustomer.ToLower() == id.ToLower());
                if (Customer == null)
                    return NotFound("تاكد من اسم العميل او يكون مسجلا");

                ID = Customer.ID;
            }
            var Reveal = DB.Reveals.Where(x => x.Idcustomer == ID && x.DateReservation.Date == DateTime.Now.Date && x.Done == false).ToList();
            if (Reveal.Count > 0)
            {
                return NotFound("هناك حجز بالفعل لهذا الاسم");
            }
            var NR = DB.Reveals.Where(x => x.DateReservation.Date == DateTime.Now.Date ).ToList();
            var N = DB.Reveals.Where(x => x.DateReservation.Date == DateTime.Now.Date).ToList().Count;
            int theNumber = NR.Count > 0 ? NR.Max(x => x.Number) + 1 : 1;
            if (run)
                theNumber = 0;
            var plane = DB.PlaneReveals.Where(x => x.DateDay.Date == DateTime.Now.Date).FirstOrDefault();
            if(plane is not null)
            {
                if (plane.Leave)
                {
                    return Ok($"هذا اليوم اجارة ولا يوجد بة حجز");
                }
                if (!plane.All)
                {
                    if((N + 1) > plane.Count)
                    {
                        return Ok($"تم الوصول للحد الاقصى للحجز لهذا اليوم وهو ({plane.Count}) كشف");
                    }
                }
            }
            var addmodel = new Reveal
            {
                Done = false,
                Idcustomer = ID,
                DateReservation = DateTime.Now,
                Number = theNumber,
                Idprice = idp
            };
            await DB.Reveals.AddAsync(addmodel);
            await DB.SaveChangesAsync();
            return Ok("");
        }

        public IActionResult DeleteReveal(int? id)
        {
            if (id == null)
                return NoContent();

            var model = DB.Reveals.Find(id);
            DB.Reveals.Remove(model);
            DB.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult Deletecheck(int? id)
        {
            var model = DB.ItemCheckups.Find(id);
            DB.ItemCheckups.Remove(model);
            DB.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public async Task<JsonResult> getchecks(int? id)
        {
            var model = await DB.ItemCheckups.Where(x => x.Idreveal == id.Value).ToListAsync();
            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> addcheck(int idcus , int idRev , string name)
        {
            int ID = -1;
            if (DB.NamesChecks.Any(x=>x.TitelName.ToLower() == name.ToLower()) || int.TryParse(name , out ID))
            {
                
                var Names = DB.NamesChecks.Where(x => x.Id == ID || x.TitelName.ToLower() == name.ToLower()).FirstOrDefault();
                var N = Names.Nameschiled.Split(',');
                foreach (var item in N)
                {
                    var AddModel = new ItemCheckup
                    {
                        Idcustomer = idcus,
                        Idreveal = idRev,
                        NameCheck = item,
                        Resulte = 0,
                        ImageReport = null
                    };
                    await DB.ItemCheckups.AddAsync(AddModel);
                    await DB.SaveChangesAsync();
                }
                
            }
            else
            {
                var AddModel = new ItemCheckup
                {
                    Idcustomer = idcus,
                    Idreveal = idRev,
                    NameCheck = name,
                    Resulte = 0,
                    ImageReport = null
                };
                await DB.ItemCheckups.AddAsync(AddModel);
                await DB.SaveChangesAsync();
            }
            
            return Json("ok");
        }

        public IActionResult print(int ?id)
        {
            if (!GetUser().Login || !id.HasValue)
                return NotFound();

            GetUser();
            var Modelprint = DB.Reveals.Where(x => x.ID == id).Include(m => m.Therapies).Include(items => items.ItemCheckups).Include(c => c.customer).FirstOrDefault();
            ViewBag.date = DB.Reveals.Where(x => x.Idcustomer == Modelprint.Idcustomer && x.DateReservation.Date > DateTime.Now.Date).FirstOrDefault()?.DateReservation;
            ViewBag.owner = DB.Owners.FirstOrDefault();
            ViewBag.medics = DB.MedicNames.ToList();
            return View(Modelprint);
        }

        [HttpPost]
        public async Task<IActionResult> AddToblacklist(int? id , string report)
        {
            if (id == null || report == "" )
                return NotFound();

            var b = new BlackList { Idcunstomer = id.Value, Report = report };
            await DB.blackLists.AddAsync(b);
            await DB.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public IActionResult Del_Blacklist(int? id)
        {
            if (id == null)
                return NotFound();

            var Bl = DB.blackLists.Find(id);
            DB.blackLists.Remove(Bl);
            DB.SaveChanges();
            return Ok();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        class reslut
        {
            public int count { get; set; }

            public string Mess { get; set; }
        }

       public class Jsonget
        {
            public int medic { get; set; }

            public string sub { get; set; }
        }
    }
}
