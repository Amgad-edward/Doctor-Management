using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;
using Doctor_Management.Controllers;

namespace Doctor_Management.Models_View
{
    public class Search 
    {
        private readonly string text;
        private  DataContext data;
        private string home = nameof(HomeController).Replace("Controller", "").ToLower();
        private string accountenter = nameof(AccountEnterController).Replace("Controller", "").ToLower();
        private string accountpay = nameof(PaysController).Replace("Controller", "").ToLower();
        private string Reveal = nameof(RevealsController).Replace("Controller", "").ToLower();
        private string customers = nameof(CustomerController).Replace("Controller", "").ToLower();
        private string Surgery = nameof(SurgeryController).Replace("Controller", "").ToLower();
        private string Check = nameof(ChecksController).Replace("Controller", "").ToLower();
        private string Customer = nameof(CustomerController).Replace("Controller", "").ToLower();
        private string Medic = nameof(MedicController).Replace("Controller", "").ToLower();
        private List<UrlLink> list => getlink();

       
        public Search(string Text)
        {
            text = Text.ToLower();
        }

        public Search(string Text , DataContext data)
        {
            text = Text.ToLower();
            this.data = data;
        }


        private List<UrlLink> getlink()//page links
        {
            var  listlink = new List<UrlLink>();
            listlink.Add(new UrlLink { Name = home, Url = getlink(home,"Out") });
            listlink.Add(new UrlLink { Name = "صفحة رئسية", Url = getlink(home,"Out") });
            listlink.Add(new UrlLink { Name = "حجز", Url = getlink(Reveal, "getreveals") });
            listlink.Add(new UrlLink { Name = "reveal", Url = getlink(Reveal, "getreveals") });
            listlink.Add(new UrlLink { Name = "تحليل", Url = getlink(Check, "AddnameList") });
            listlink.Add(new UrlLink { Name = "عملاء العملاء عميل", Url = getlink(Customer, "Index") });
            listlink.Add(new UrlLink { Name = "قيم تحليل الطبيعية الافتراضية الاساسية", Url = getlink(Check, "Normals") });
            listlink.Add(new UrlLink { Name = "دواء ادوية medic", Url = getlink(Medic, "Index") });
            return listlink;
        }

       

        private string SearchResulte()
        {
            if (list.Any(x=>x.Name.Contains(text)))
            {
                return list.FirstOrDefault(x => x.Name.Contains(text));
            }
            else if (data.Customers.Any(x => x.NameCustomer.ToLower().Contains(text)))
            {
                var cust = data.Customers.FirstOrDefault(x => x.NameCustomer.ToLower().Contains(text));
                if (cust is not null)
                    return $"/Home/infocustomer/{cust.ID}";
            }

            return string.Empty;
        }



        private string getlink(string name , string Method = "" )
        {
            return $"/{name}/{Method}";
        }

        public static implicit operator Search(string text)
        {
            return new Search(text);
        }

        public static implicit operator string(Search search)
        {
            return search.SearchResulte();
        }
        public override string ToString()
        {
            return SearchResulte();
        }

        class UrlLink
        {
            public string Name { get; set; }
            public string Url { get; set; }




            public static implicit operator string (UrlLink url)
            {
                return url.Url;
            }
            public override string ToString()
            {
                return this.Url;
            }
        }
    }
}
