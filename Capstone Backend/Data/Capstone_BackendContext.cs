using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Capstone_Backend.Models;

namespace Capstone_Backend.Data
{
    public class Capstone_BackendContext : DbContext
    {
        public Capstone_BackendContext() { }
        public Capstone_BackendContext (DbContextOptions<Capstone_BackendContext> options)
            : base(options)
        {
        }
        public DbSet<Capstone_Backend.Models.User> User { get; set; }
        public DbSet<Capstone_Backend.Models.Vendor> Vendor { get; set; }
        public DbSet<Capstone_Backend.Models.Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<User>(e => { e.HasIndex(u => u.Username).IsUnique(); });
            builder.Entity<Product>(p => { p.HasIndex(p => p.PartNumber).IsUnique(); });
            builder.Entity<Vendor>(v => { v.HasIndex(c => c.Code).IsUnique(); });
            builder.Entity<Request>(r => { r.Property(d => d.DeliveryMode).HasDefaultValue("Pickup"); });
            builder.Entity<Request>(r => { r.Property(s => s.Status).HasDefaultValue("NEW"); });
            builder.Entity<Request>(r => { r.Property(t => t.Total).HasDefaultValue(0); });
            builder.Entity<Requestline>(rl => { rl.Property(q => q.Quantity).HasDefaultValue(1); });
        }

        public DbSet<Capstone_Backend.Models.Request> Request { get; set; }

        public DbSet<Capstone_Backend.Models.Requestline> Requestline { get; set; }
        
    }

}
