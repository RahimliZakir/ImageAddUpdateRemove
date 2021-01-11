using ImageAddUpdateRemoveWebApplicationUI.Models.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageAddUpdateRemoveWebApplicationUI.Models.DataContext
{
    public static class DataSeeding
    {
        public static IApplicationBuilder DataSeed(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ImageDbContext>();

                db.Database.Migrate();

                if (!db.Images.Any())
                {
                    db.Images.Add(new Image
                    {
                        ImagePath = "first-azerbaijan.jpg"
                    });

                    db.Images.Add(new Image
                    {
                        ImagePath = "second-snow.jpg"
                    });

                    db.SaveChanges();
                }
            }

            return app;
        }
    }
}
