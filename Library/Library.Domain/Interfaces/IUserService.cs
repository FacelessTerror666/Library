using Library.Database.Entities;
using Library.Domain.Models.Account;
using Library.Domain.Models.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IUserService
    {
        //AccountController
        public Task<IdentityResult> RegisterAsync(RegisterModel model);

        public Task SignInAsync(RegisterModel model);

        public Task<RegisterModel> GetUserModelByEmail(string email);

        public Task<SignInResult> PasswordSignInAsync(LoginModel model);

        public Task SignOutAsync();

        //UsersController
        public List<User> GetUsers();

        public Task<IdentityResult> CreateUserAsync(CreateUserModel model);

        public Task<EditUserModel> EditUserGet(string id);

        public Task<IdentityResult> EditUserPost(EditUserModel model);

        public Task DeleteUserAsync(string id);
    }
}
