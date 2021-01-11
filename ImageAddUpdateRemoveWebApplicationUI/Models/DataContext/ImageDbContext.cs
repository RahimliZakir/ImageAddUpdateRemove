using ImageAddUpdateRemoveWebApplicationUI.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageAddUpdateRemoveWebApplicationUI.Models.DataContext
{
    public class ImageDbContext : DbContext
    {
        public ImageDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Image>()
                .Property(p => p.CreatedDate)
                .HasDefaultValueSql("DATEADD(HOUR,4,GETUTCDATE())");
        }
    }
}
