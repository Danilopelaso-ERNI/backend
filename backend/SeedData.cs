using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace backend.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Users.Any())
                {
                    return;   // DB has been seeded
                }

                context.Users.AddRange(
                    new User
                    {
                        Username = "examiner",
                        Password = BCrypt.Net.BCrypt.HashPassword("password"), // Hash the password
                        Role = "Examiner"
                    },
                    new User
                    {
                        Username = "examinee",
                        Password = BCrypt.Net.BCrypt.HashPassword("password"), // Hash the password
                        Role = "Examinee"
                    }
                );

                context.SaveChanges();
            }
        }
    }
}