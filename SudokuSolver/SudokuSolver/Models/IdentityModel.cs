using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace SudokuSolver.Models
{
    public class IdentityModel
    {
        public class User : IdentityUser
        {
            public virtual List<Puzzle> Puzzles { get; set; }

            public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
            {
                var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
                return userIdentity;
            }


        }

        public class ApplicationDbContext : IdentityDbContext<User>
        {
            public ApplicationDbContext(): base("SudokuSolver", throwIfV1Schema: false)
            {

            }

            public static ApplicationDbContext Create()
            {
                return new ApplicationDbContext();
            }
        }
    }
}