
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;


namespace Doctor_Management.Models_View
{
    public class CustomerInfo
    {
        private readonly Customer customer;
        private readonly string dign;
        private  DataContext data;

        public int Age => getAge();

        public string Gender => getgender();

        public string Blood => getblood();

        public string Phones => customer.Phones;

        public bool IsInBlackList => Isblacklist();

        public DateTime DateBirth => customer.dateBirth;

        public string Name => customer.NameCustomer;

        public string GetReportBlackList => reportbalcklist();

        public List<(string name, double Avr)> AverageChecks => getavrege();

        public List<Therapy> Medics => getmedic();

        public IEnumerable<Informations> Informations => getinfo();

        public IEnumerable<Therapy> AllMedics => getAllTherapys();

        public IEnumerable<ItemCheckup> GetAllChecks => getchecks();


        public CustomerInfo(Customer customer)
        {
            this.customer = customer;
        }

        public CustomerInfo(Customer customer, DataContext data)
        {
            this.customer = customer;
            this.data = data;
        }


        public CustomerInfo(Customer customer , DataContext data , string Ding)
        {
            this.customer = customer;
            this.data = data;
            this.dign = Ding;
        }

        private List<Therapy> getmedic()
        {
            if(data != null && dign != null)
            {
                var db = data.Reveals.Include(c=>c.customer)
                    .Where(x => x.Done && x.Diagnosis.ToLower() == this.dign.ToLower())
                    .OrderByDescending(d => d.DateReservation.Date).ToList();
                foreach (var item in db)
                {
                    CustomerInfo info = item.customer;
                    if ((this.getAge() - 10) <= info.Age | this.getgender() == info.Gender)
                    {
                        var Ther = data.Therapies.Include(m => m.medicName).Where(x => x.Idreveal == item.ID).ToList();
                        return Ther;
                    }
                    else if ((this.getAge() + 10) <= info.Age | this.getgender() == info.Gender)
                    {
                        var Ther = data.Therapies.Include(m => m.medicName).Where(x => x.Idreveal == item.ID).ToList();
                        return Ther;
                    }
                }
                    
            }
            return null;
        }
        private int getAge()
        {
            return DateTime.Now.Year - customer.dateBirth.Year;
        }

        private string reportbalcklist()
        {
            if (IsInBlackList && data != null)
            {
                return data.blackLists.FirstOrDefault(x => x.Idcunstomer == customer.ID).Report;
            }
            return "غير مدرج للقائمة";
        }
        private IEnumerable<Informations> getinfo()
        {
            if (data is not null)
                return data.Informations.Where(x => x.IdCustomer == customer.ID).ToList();
            return null;
        }

        private List<(string n , double a)> getavrege()
        {
            var listall = new List<(string name, double av)>();
            if(data != null)
            {
                var allcheck = data.ItemCheckups.Where(x => x.Idcustomer == customer.ID).ToList();
                var Namecheck = allcheck.Select(x => x.NameCheck).Distinct().ToList();
                foreach (var item in Namecheck)
                {
                    if(allcheck.Where(x=>x.NameCheck.ToLower() == item.ToLower()).ToList().Count > 1)
                    {
                        var Avrg = allcheck.Where(x => x.NameCheck.ToLower() == item.ToLower()).Average(rs => rs.Resulte);
                        listall.Add((name: item, av: Avrg));
                    }

                }
            }
            return listall;
        }

        private bool Isblacklist()
        {
            if (data is not null)
                return data.blackLists.Any(x => x.Idcunstomer == customer.ID);

            return false;
        }

        private IEnumerable<Therapy> getAllTherapys()
        {
            if (data is not null)
            {
                var reveals = data.Reveals.Where(x => x.Idcustomer == customer.ID).Select(t => t.ID).ToList();
                var list = new List<Therapy>();
                foreach (var item in reveals)
                {
                    var g = data.Therapies.Include(medic => medic.medicName).Where(x => x.Idreveal == item).ToList();
                    list.AddRange(g);
                }
                return list;
            }
            return null;
        }

        private string getgender()
        {
            return customer.Gender;
        }

        private IEnumerable<ItemCheckup> getchecks()
        {
            if (data is not null)
                return data.ItemCheckups.Include(r=>r.reveal).Where(x => x.Idcustomer == customer.ID).ToList();

            return null;
        }
        private string getblood()
        {
            return customer.Blood;
        }

        public static implicit operator CustomerInfo(Customer customer)
        {
            return new CustomerInfo(customer);
        }

        public static implicit operator int(CustomerInfo customerInfo)
        {
            return customerInfo.getAge();
        }

        public static implicit operator DateTime(CustomerInfo customerInfo)
        {
            return customerInfo.DateBirth;
        }

        public static explicit operator string(CustomerInfo customer)
        {
            return customer.Name;
        }

        public static implicit operator List<ItemCheckup>(CustomerInfo customerInfo)
        {
            return customerInfo.GetAllChecks.ToList();
        }

    }
}
