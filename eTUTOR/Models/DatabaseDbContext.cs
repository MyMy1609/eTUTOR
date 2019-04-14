using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace eTUTOR.Models
{
    public class DatabaseDbContext: DbContext
    {
        public DatabaseDbContext()
            : base(@"data source= ten database trong sql ; initial catalog = eTUITORModel; Integrated Security = True ")
        {

        }

        public DbSet<student> students { get; set; }
        public DbSet<parent> parents { get; set; }
    }
}