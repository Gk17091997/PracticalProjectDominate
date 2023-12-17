using Dominate.Data.Constant;
using Dominate.Data.Interface;
using Dominate.Data.Model;
using Dominate.Data.ViewModel;
using Dominate.Services.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dominate.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IRepository<User> _userRepositories;
        private readonly IConfiguration _configuration;
        private readonly IEmailHelper _emailHelper;
        public UserRepository(IUnitofWork unitofWork, IConfiguration configuration, IEmailHelper emailHelper)
        {
            _unitofWork = unitofWork;
            _userRepositories = _unitofWork.GetRepository<User>();
            _configuration = configuration;
            _emailHelper = emailHelper;
        }

        public async Task<UserViewModel> CreateUserAsync(AddUserViewModel addUpdateUser)
        {

            var user = new User
            {
                Username = addUpdateUser.Username,
                FirstName = addUpdateUser.FirstName,
                LastName = addUpdateUser.LastName,
                Password = GetHashedPassword(addUpdateUser.Password)
            };
            _userRepositories.Add(user);

            await _unitofWork.commit();
            return new UserViewModel {
                Username = addUpdateUser.Username,
                FirstName = addUpdateUser.FirstName,
                LastName = addUpdateUser.LastName,
            };


        }
        public string GetHashedPassword(string plainPassword)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainPassword));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userRepositories.GetByIdAsync(userId);
            if (user != null)
            {
                _userRepositories.Delete(user);
                await _unitofWork.commit();
                return true;
            }
            return false;
        }

        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            return await _userRepositories.GetAll().Select(x=> new UserViewModel
            {
                Username = x.Username,
                FirstName = x.FirstName,
                LastName = x.LastName,
            }).ToListAsync();
        }

        public Task<User> GetUserByEmailAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<UserViewModel> GetUserByIdAsync(int userId)
        {
            var user = await _userRepositories.GetByIdAsync(userId);
            return new UserViewModel
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName
            }; 
        }

        public async Task<string> LogIn(LoginViewModel viewModel)
        {
            var password = GetHashedPassword(viewModel.Password);
            var user = await _userRepositories.GetAll().Where(x => x.Username == viewModel.UserName && x.Password == password).FirstOrDefaultAsync();
            if (user != null)
            {
                var jwtToken = _configuration.GetSection("JwtTokenKeysValue:JWT_SECURRITY_KEY").Value;
                var tokenRequest = new TokenRequestViewModel
                {
                    JwtKey = jwtToken,
                    UserFullName = $"{user.FirstName} {user.LastName}",
                    UserName = user.Username
                };
                var token = TokenManager.GenerateToken(tokenRequest);
                return token;
            }
            return null;
        }

        public async Task<UserViewModel> UpdateUserAsync(UpdateUserViewModel updatedUser)
        {
            var user = await _userRepositories.GetByIdAsync(updatedUser.Id);
            if (user != null)
            {
                user.FirstName = !string.IsNullOrEmpty(updatedUser.FirstName) ? updatedUser.FirstName : user.FirstName;
                user.LastName = !string.IsNullOrEmpty(updatedUser.LastName) ? updatedUser.LastName : user.LastName;
                _userRepositories.Update(user);
                await _unitofWork.commit();
            }
            return new UserViewModel
            {
                Username = user.Username,
                FirstName = updatedUser.FirstName,
                LastName = updatedUser.LastName,
            }; 
        }

        public async Task<string> ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            var user = await _userRepositories.GetAll().Where(x => x.Username == forgotPassword.Email).FirstOrDefaultAsync();
            if (user != null)
            {
                var resetToken = GenerateResetToken();
                user.ResetPasswordToken = resetToken;
                _userRepositories.Update(user);
                ForgotPasswordEmailViewModel forgotpswrd = new ForgotPasswordEmailViewModel
                {
                    UserFullName = user.FirstName + " " + user.LastName,
                    Email = user.Username,
                    ResetPasswordToken = resetToken
                };
                var response = await _emailHelper.ForgotPasswordEmailAsync(forgotpswrd);
                if (response)
                {
                    await _unitofWork.commit();
                    return ConstantMsg.PaswordReset;
                }
                return ConstantMsg.FailedEmail;
            }
            return null;
        }
        private string GenerateResetToken()
        {

            return Guid.NewGuid().ToString();
        }
    }
}
