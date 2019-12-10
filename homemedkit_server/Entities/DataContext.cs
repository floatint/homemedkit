using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace homemedkit_server.Entities
{
    public class DataContext : DbContext
    {
        public DbSet<Medicine> Medicines { set; get; }
        public DbSet<MedkitRecord> MedkitRecords { set; get; }
        public DbSet<Symptom> Symptoms { set; get; }
        public DbSet<Disease> Diseases { set; get; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
