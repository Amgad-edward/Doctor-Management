using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.Models;

namespace Doctor_Management
{
    public class Databackup
    {
        public List<Customer> CustomersTable { get; set; }

        public List<Acccount_Reveal> Acccount_Reveal_Table { get; set; }

        public List<Account_Enter> Account_Enter_Table { get; set; }

        public List<Account_Pay> Account_Pays_Table { get; set; }

        public List<BlackList>  BlackLists_Table { get; set; }

        public List<Employee> Employees_Table { get; set; }

        public List<Fixed_pay> Fixed_Pays_Table { get; set; }

        public List<Informations> Informations_Table { get; set; }

        public List<ItemCheckup> ItemCheckups_Table { get; set; }

        public List<Loging> Logings_Table { get; set; }

        public List<MedicName> MedicNames_Table { get; set; }

        public List<Owner> Owners_Table { get; set; }

        public List<Price> Prices_Table { get; set; }

        public List<Reveal> Reveals_Table { get; set; }

        public List<Therapy> Therapies_Table { get; set; }
    }
}
