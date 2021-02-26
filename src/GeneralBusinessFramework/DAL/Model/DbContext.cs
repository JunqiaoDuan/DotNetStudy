using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class AzureDbContext: DbContext
    {
        public AzureDbContext() :
            base(@"Data Source=localhost;Initial Catalog=DotNetStudy;Persist Security Info=True;User ID=sa;Password=sa")
        {

        }

        public DbSet<Order> Orders { get; set; }
    }
}
