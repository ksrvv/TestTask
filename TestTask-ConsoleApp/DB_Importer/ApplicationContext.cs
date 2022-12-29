using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DB_Importer
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<DataModel> Data => Set<DataModel>(); //Data will contain the DataModel objects collection
        public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=testtask.db");//in case database doesnt exist it will be created  or the existing one will be used
        }

    }
}
