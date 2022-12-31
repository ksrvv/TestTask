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
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
                   : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
