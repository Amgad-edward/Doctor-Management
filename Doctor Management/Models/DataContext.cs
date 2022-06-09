using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MySql.EntityFrameworkCore.DataAnnotations;

namespace Doctor_Management.Models
{
   
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Owner> Owners { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reveal> Reveals { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<ItemCheckup> ItemCheckups { get; set; }
        public DbSet<MedicName> MedicNames { get; set; }
        public DbSet<Therapy> Therapies { get; set; }
        public DbSet<Informations> Informations { get; set; }
        public DbSet<BlackList> blackLists { get; set; }
        public DbSet<Acccount_Reveal> Acccount_Reveals { get; set; }
        public DbSet<Account_Pay> Account_Pays { get; set; }
        public DbSet<Fixed_pay> Fixed_Pays { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Loging> Logings { get; set; }
        public DbSet<Account_Enter> account_Enters { get; set; }
        public DbSet<NamesCheck> NamesChecks { get; set; }
        public DbSet<ResultNormal> ResultNormals { get; set; }
        public DbSet<PlaneReveals> PlaneReveals { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<Surgery> Surgeries { get; set; }
    }
}
