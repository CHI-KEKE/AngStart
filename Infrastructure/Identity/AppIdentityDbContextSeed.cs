using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async  Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            string[] emails = new string[] { "showji@gmail.com", "gina@gmail.com", "allen@gmail.com" };

            await SeedUserAndRoleAsync(userManager,"showji",emails[0],"Admin");
            await SeedUserAndRoleAsync(userManager,"Gina",emails[1],"Manager");
            await SeedUserAndRoleAsync(userManager,"Allen",emails[2],"Member");

        }

        public static async Task SeedUserAndRoleAsync(UserManager<AppUser> userManager,string name,string email,string role)
        {
            // if(!userManager.Users.Any())
            if(await userManager.FindByEmailAsync(email) == null)
            {
                var user = new AppUser
                {
                    Name = name,
                    Email = email,
                    UserName = email,
                    PictureUrl = "https://schoolvoyage.ga/images/123498.png",
                    Address = new Address
                    {
                        FirstName = name,
                        LastName = "Lin",
                        Street = "JinDe Road",
                        City = "XinPei",
                        State = "New Your",
                        ZipCode = "88999",
                    }
                };

                await userManager.CreateAsync(user,"Pa$_w0rd");
                await userManager.AddToRoleAsync(user,role);

            }
        }
    }
}