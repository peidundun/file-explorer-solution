using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB
{
    public class XDbContext : DbContext
    {
        public XDbContext(DbContextOptions<XDbContext> options)
       : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.LogTo(msg => {
            //    Console.WriteLine(msg);
            //});
            // optionsBuilder.UseBatchEF_MySQLPomelo();//as for MySQL
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Entities.GeneralFile> GeneralFiles { get; set; }
    }
}
