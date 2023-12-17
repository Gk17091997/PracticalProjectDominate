using Dominate.Data.Model;
using Dominate.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominate.Services.IRepositories
{
    public interface IUserRepository
    {
        Task<List<UserViewModel>> GetAllUsersAsync();
        Task<UserViewModel> GetUserByIdAsync(int userId);
        Task<UserViewModel> CreateUserAsync(AddUserViewModel newUser);
        Task<bool> DeleteUserAsync(int userId);
        Task<UserViewModel> UpdateUserAsync(UpdateUserViewModel updatedUser);
        Task<string> LogIn(LoginViewModel viewModel);
        Task<string> ForgotPassword(ForgotPasswordViewModel forgotPassword);
    }
}
