using Dominate.Data.ViewModel;

namespace Dominate.Services.IRepositories
{
    public interface IEmailHelper
    {
        Task<bool> ForgotPasswordEmailAsync(ForgotPasswordEmailViewModel model);
    }
}
