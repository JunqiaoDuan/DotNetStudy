using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace DAL.Model
{
    public class AzureDbContext: DbContext
    {
        public AzureDbContext() :
            base(SettingsHelper.GetStringValue("DbConnString"))
        {

        }

        public DbSet<Order> Orders { get; set; }
    }
}
