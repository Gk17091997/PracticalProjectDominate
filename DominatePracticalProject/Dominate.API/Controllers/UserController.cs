using Azure;
using Dominate.Data.Constant;
using Dominate.Data.ViewModel;
using Dominate.Services.IRepositories;
using Dominate.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dominate.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserRepository userRepository,ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
            
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel loginView)
        {
            try
            {
                _logger.LogInformation($"login Api executing at {DateTime.Now}");
                var userLogin = await _userRepository.LogIn(loginView);
                if (userLogin != null)
                {
                    _logger.LogInformation($"login Api executed at {DateTime.Now}");
                    return new OkObjectResult(new ResponseViewModel { IsSuccess = true, Data = userLogin });
                }
                return new OkObjectResult(new ResponseViewModel { IsSuccess = false, Message = ConstantMsg.InvalidMsg });
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"exception :{ex.Message} {DateTime.Now}");
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(AddUserViewModel addUpdateUser)
        {
            try
            {
                _logger.LogInformation($"Create user  Api executing at {DateTime.Now}");
                var user = await _userRepository.CreateUserAsync(addUpdateUser);

                _logger.LogInformation($"create Api executed at {DateTime.Now}");
                return new OkObjectResult(new ResponseViewModel { IsSuccess = true, Data = user });
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"exception :{ex.Message} {DateTime.Now}");
                throw;
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserViewModel updatedUser)
        {
            try
            {
                var user = await _userRepository.UpdateUserAsync(updatedUser);

                return new OkObjectResult(new ResponseViewModel { IsSuccess = true, Data = user });

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"exception :{ex.Message} {DateTime.Now}");
                throw;
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var listUser = await _userRepository.GetAllUsersAsync();

                return new OkObjectResult(new ResponseViewModel { IsSuccess = true, Data = listUser });

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"exception :{ex.Message} {DateTime.Now}");
                throw;
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user != null)
                {

                    return new OkObjectResult(new ResponseViewModel { IsSuccess = true, Data = user });
                }
                return new OkObjectResult(new ResponseViewModel { IsSuccess = false, Message = ConstantMsg.UserNotFound });
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"exception :{ex.Message} {DateTime.Now}");
                throw;
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var user = await _userRepository.DeleteUserAsync(userId);
                if (user != null)
                {

                    return new OkObjectResult(new ResponseViewModel { IsSuccess = true, Data = user });
                }
                return new OkObjectResult(new ResponseViewModel { IsSuccess = false, Message = ConstantMsg.UserNotFound });
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"exception :{ex.Message} {DateTime.Now}");
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            try
            {
                var response = await _userRepository.ForgotPassword(forgotPassword);
                if (response == null)
                {

                    return new OkObjectResult(new ResponseViewModel { IsSuccess = false, Message = ConstantMsg.UserNotFound });
                }
                if (response == ConstantMsg.PaswordReset)
                {
                    return new OkObjectResult(new ResponseViewModel { IsSuccess = true, Message = response });
                }
                return new OkObjectResult(new ResponseViewModel { IsSuccess = false, Message = response });
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"exception :{ex.Message} {DateTime.Now}");
                throw;
               
            }
        }

        
    }
}
