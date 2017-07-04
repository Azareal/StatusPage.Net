using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StatusPage.Net.Models;

namespace StatusPage.Net.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Ping> Pings { get; set; }
        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<SiteGroup> SiteGroups { get; set; }
        public virtual DbSet<StatusMessage> StatusMessages { get; set; }
        public virtual DbSet<Incident> Incidents { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Ping>().HasIndex(x => x.DateTime);
            builder.Entity<PingSetting>().HasIndex(x => x.Visible);
            builder.Entity<PingSetting>().HasIndex(x => x.Site);
            builder.Entity<Incident>().HasIndex(x => x.Start);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
