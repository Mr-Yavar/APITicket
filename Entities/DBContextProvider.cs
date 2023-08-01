using Entities.Configurtion;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Entities
{
    public class DBContextProvider : DbContext
    {
        public DBContextProvider(DbContextOptions options):base(options)
           { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }

        public DbSet<CompanyModel> Companies { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }
    }
}
