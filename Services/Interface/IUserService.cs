using AuthApp_Api.Models;

namespace AuthApp_Api.Services.Interface
{
    public interface IUserService
    {

        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);
        Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token);

        Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordViewModel model);

        Task<UserManagerResponse> ForgetPasswordAsync(string email);
    }
}
