using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace RecipeMvc.Models
{
    public class RespDataContext : DbContext
    {
       

        public DbSet<RespComp> RespComps { get; set; }
        static RespDataContext()
        {
         Database.SetInitializer(new DropCreateDatabaseIfModelChanges<RespDataContext>());
         //   Database.SetInitializer(new DropCreateDatabaseAlways<RespDataContext>());
           
        }
    }
} 