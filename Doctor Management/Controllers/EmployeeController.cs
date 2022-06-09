using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.IRepository;
using Doctor_Management.Models;
using Doctor_Management.Models_View;
using NToastNotify;


namespace Doctor_Management.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IToastNotification toast;
        private readonly IRepositoryData<Employee> employee;
        private readonly IRepositoryData<Fixed_pay> Fixed;

        public EmployeeController(IToastNotification toast,IRepositoryData<Fixed_pay> Fixed,IRepositoryData<Employee> employee)
        {
            this.toast = toast;
            this.Fixed = Fixed;
            this.employee = employee;
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
            if (!GetUser().Admin)
                return NoContent();

            return View(employee.GetAll());
        }

        public IActionResult Create()
        {
            if (!GetUser().Admin)
                return NoContent();

            return View(new EmployeeView { ISCreate = true , datestart=DateTime.Now });
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeView model)
        {
            GetUser();
            if (employee.Any(x=>x.Name == model.Name))
            {
                ModelState.AddModelError("Name", "هذا الموظف مضاف مسبقا");
                return View(model);
            }
            var savemodel = new Employee
            {
                Name = model.Name , Salary = model.Salary,
                TitleJop =model.TitleJop , datestart = model.datestart
            };
            var modelfix = new Fixed_pay
            {
                itemName = $"salary {model.Name}",
                FixsedAmmount = model.Salary,Timespan = 30
            };
            await employee.AddAsync(savemodel);
            await Fixed.AddAsync(modelfix);
            toast.AddSuccessToastMessage($"Add Employee {model.Name} Done");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || !GetUser().Admin)
                return NoContent();

            GetUser();
            var M = employee.Find(id);
            ViewBag.id = Fixed.Find(x => x.itemName == $"salary {M.Name}").Id;
            var model = new EmployeeView
            {
                Id = M.Id , Name = M.Name ,Salary = M.Salary
                ,datestart = M.datestart,TitleJop = M.TitleJop, ISCreate = false
            };
            return View("Create",model);
        }

        [HttpPost , ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeView model)
        {
            GetUser();
            var savemodel = new Employee
            {
                Id = model.Id,
                Name = model.Name,
                Salary = model.Salary,
                TitleJop = model.TitleJop,
                datestart = model.datestart
            };
            employee.Update(savemodel);
            var modelfix = new Fixed_pay
            {
                Id = (int)TempData["id"],
                itemName = $"salary {model.Name}",
                FixsedAmmount = model.Salary,
                Timespan = 30
            };
            Fixed.Update(modelfix);

            toast.AddSuccessToastMessage($"Edit info Employee {model.Name} Done");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult delete(int? id)
        {
            if (id == null || !GetUser().Admin)
                return NoContent();

            var Emp = employee.Find(id);
            var Fix = Fixed.Find(x => x.itemName == $"salary {Emp.Name}");
            employee.Delete(id);
            Fixed.Delete(Fix);
            return Ok();
        }
    }
}
