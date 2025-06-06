using DataAccessLayer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Data
{
    public class ApplicationDBContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var roles = new List<IdentityRole>{
                new IdentityRole{Name="Admin",NormalizedName="ADMIN"},
                new IdentityRole{Name="Staff", NormalizedName="STAFF"},
                new IdentityRole{Name="Manager", NormalizedName="MANAGER"},
                new IdentityRole{Name="Consultant", NormalizedName="CONSULTANT"},
                new IdentityRole{Name="Member", NormalizedName="MEMBER"},
            };
            base.OnModelCreating(builder);
        }
    }
}
