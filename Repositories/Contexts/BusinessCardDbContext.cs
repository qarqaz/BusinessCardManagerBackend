using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contexts
{
    public class BusinessCardDbContext : DbContext
    {
        public DbSet<BusinessCard> BusinessCards { get; set; }

        public BusinessCardDbContext(DbContextOptions<BusinessCardDbContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
