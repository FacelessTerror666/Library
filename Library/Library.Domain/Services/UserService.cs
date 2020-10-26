using Library.Database.Entities;
using Library.Domain.Interfaces;
using Library.Domain.Models.Account;
using Library.Domain.Models.Roles;
using Library.Domain.Models.Users;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        //AccountController
        public async Task<IdentityResult> RegisterAsync(RegisterModel model)
        {
            var user = new User { Email = model.Email, UserName = model.Email };
            // добавляем пользователя
            var result = await userManager.CreateAsync(user, model.Password);
            await userManager.AddToRolesAsync(user, new List<string> { RoleModel.Reader });
            return result;
        }

        public async Task SignInAsync(RegisterModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id.ToString());
            await signInManager.SignInAsync(user, false);
        }

        public async Task<RegisterModel> GetUserModelByEmail(string email)
        {
            var normalizedEmail = email.ToUpper();
            var user = await userManager.FindByEmailAsync(normalizedEmail);
            var model = new RegisterModel
            {
                Id = user.Id,
                Email = user.Email
            };
            return model;
        }

        public async Task<SignInResult> PasswordSignInAsync(LoginModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            return result;
        }

        public async Task SignOutAsync()
        {
            await signInManager.SignOutAsync();
        }

        //UsersController
        public List<User> GetUsers()
        {
            var users = userManager.Users.ToList();
            return users;
        }

        public async Task<IdentityResult> CreateUserAsync(CreateUserModel model)
        {
            var user = new User { Email = model.Email, UserName = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);
            return result;
        }

        public async Task<EditUserModel> EditUserGet(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            EditUserModel model = new EditUserModel { Id = user.Id, Email = user.Email };
            return model;
        }

        public async Task<IdentityResult> EditUserPost(EditUserModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id.ToString());
            user.Email = model.Email;
            user.UserName = model.Email;
            var result = await userManager.UpdateAsync(user);
            return result;
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }
        }
    }
}
