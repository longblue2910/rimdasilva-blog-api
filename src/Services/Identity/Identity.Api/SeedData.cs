using Identity.Api.Data;
using Identity.Api.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;

namespace Identity.Api
{
    public class SeedData
    {
        public static void EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var admin = userMgr.FindByNameAsync("admin").Result;
                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "rimdasilva@gmail.com",
                        EmailConfirmed = true,
                    };
                    var result = userMgr.CreateAsync(admin, "Admin@123").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(admin, new Claim[]{
                                new Claim(JwtClaimTypes.Name, "Administrator"),
                                new Claim(JwtClaimTypes.GivenName, "System"),
                                new Claim(JwtClaimTypes.FamilyName, "rimdasilva"),
                                new Claim(JwtClaimTypes.WebSite, "http://rimdasilva.com"),
                            }).Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("rimdasilva created");
                }
                else
                {
                    Log.Debug("rimdasilva already exists");
                }

                var longjr = userMgr.FindByNameAsync("longjr").Result;
                if (longjr == null)
                {
                    longjr = new ApplicationUser
                    {
                        UserName = "longjr",
                        Email = "tienlong291099@gmail.com",
                        EmailConfirmed = true
                    };
                    var result = userMgr.CreateAsync(longjr, "Admin@123").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(longjr, new Claim[]{
                                new Claim(JwtClaimTypes.Name, "Long"),
                                new Claim(JwtClaimTypes.GivenName, "Jr"),
                                new Claim(JwtClaimTypes.FamilyName, "rimdasilva"),
                                new Claim(JwtClaimTypes.WebSite, "http://rimdasilva.com"),
                                new Claim("location", "hcm")
                            }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("longjr created");
                }
                else
                {
                    Log.Debug("longjr already exists");
                }
            }
        }
    }
}
