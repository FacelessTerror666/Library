using Library.Database.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Library.Database.Entities
{
    public class RoleInitialize : IdentityRole<long>, IEntity
    {
        public const string Admin = "admin";
        public const string Librarian = "librarian";
        public const string Reader = "reader";

        public RoleInitialize()
        {

        }

        public RoleInitialize(string roleName) : base(roleName)
        {

        }

        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<RoleInitialize> roleManager)
        {
            var adminEmail = "admin@mail.ru";
            string adminPassword = "Qwerty123!";

            string librarianEmail = "librarian@mail.ru";
            string librarianPassword = "Qwerty321!";

            if (await roleManager.FindByNameAsync(Admin) == null)
            {
                await roleManager.CreateAsync(new RoleInitialize(Admin));
            }
            if (await roleManager.FindByNameAsync(Librarian) == null)
            {
                await roleManager.CreateAsync(new RoleInitialize(Librarian));
            }
            if (await roleManager.FindByNameAsync(Reader) == null)
            {
                await roleManager.CreateAsync(new RoleInitialize(Reader));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new User { Email = adminEmail, UserName = adminEmail };
                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Admin);
                }
            }
            if (await userManager.FindByNameAsync(librarianEmail) == null)
            {
                User librarian = new User { Email = librarianEmail, UserName = librarianEmail };
                IdentityResult result = await userManager.CreateAsync(librarian, librarianPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(librarian, Librarian);
                }
            }
        }
    }
}
