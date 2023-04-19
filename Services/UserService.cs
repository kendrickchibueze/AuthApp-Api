using AuthApp_Api.Data;
using AuthApp_Api.Models;
using AuthApp_Api.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthApp_Api.Services
{
    public class UserService : IUserService
    {

        private readonly UserManager<User> _userManager;
        private readonly ISecurityService _jwtsecurity;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public UserService(UserManager<User> userManager, ISecurityService jwtsecurity, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _jwtsecurity = jwtsecurity;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token)
        {
            try
            {
               var user = await _userManager.FindByIdAsync( userId );
                if (user == null)
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = ErrorMsg.InvalidUser + " " + userId
                    };
                }
                var dToken = WebEncoders.Base64UrlDecode(token);
                string normalToken = Encoding.UTF8.GetString(dToken);

                var result = await _userManager.ConfirmEmailAsync(user, normalToken);
                if (result.Succeeded)
                {
                    return new UserManagerResponse
                    {
                        Message = Msg.EmailConfirm,
                        IsSuccess = true,
                    };
                 
                }
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = ErrorMsg.EmailNotConfirm,
                    Errors = result.Errors.Select(e => e.Description)
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserManagerResponse> ForgetPasswordAsync(string email)
        {
            try
            {

                var user = await _userManager.FindByEmailAsync(email);

                if(user == null)
                {
                    return new UserManagerResponse
                    {
                        Message = ErrorMsg.NoUserEmail,
                        IsSuccess = false,
                    };
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var eToken = Encoding.UTF8.GetBytes(token);
                var vToken = WebEncoders.Base64UrlEncode(eToken);   


                string url = $"{_configuration["appURL"]}ResetPassword?email={email}&token={vToken}";


                await _emailService.SendEmail(email, Msg.ResetPassword, $"<h1>{Msg.ResetPasswordMsg1}</h1>" +
                  $"<p>{Msg.ResetPasswordMsg2}<a href='{url}' > {Msg.ResetPasswordMsg3}</a> </p>");

                return new UserManagerResponse
                {
                    Message = Msg.ResetPasswordSuccess,
                    IsSuccess = true,
                };



            }
            catch (Exception)
            {
                throw;
            }
        }

        public  async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            try
            {
                var user =  await _userManager.FindByEmailAsync(model.Email);
                if(user == null)
                {
                    return new UserManagerResponse
                    {
                        Message = ErrorMsg.NoUserEmail,
                        IsSuccess = false,
                    };
                } 
                
                var result = await  _userManager.CheckPasswordAsync(user, model.Password);
                if (!result)
                {
                    return new UserManagerResponse
                    {
                        Message = ErrorMsg.InvalidPassword,
                        IsSuccess = false,
                    };
                }


                //generate tokens
                var claims = new[] 
                { 
                    new Claim("Email", model.Email),   
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                };

                //security(we get the tokens)

                _jwtsecurity.SecureToken(claims, out JwtSecurityToken token, out string tokenAstring);
                return new UserManagerResponse
                {
                    Message = tokenAstring,
                    IsSuccess = true,
                    ExpiredDate = token.ValidTo,
                };




            }
            catch (Exception)
            {
                throw;  
            }
        }

        public  async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            try
            {
                if(model == null)
                {
                    throw new NullReferenceException(ErrorMsg.NullModel);
                }

                if (model.Password != model.ConfirmPassword)
                {
                    return new UserManagerResponse
                    {

                        Message = Msg.ConfirmPasswordNotMatch,
                        IsSuccess = false,

                    };
                }

                    var user = new User
                    { 
                    
                      Email = model.Email,
                      UserName = model.Email
                    
                    
                    };

                    var result  = await _userManager.CreateAsync(user, model.Password);

                

                if (result.Succeeded)
                {
                    var confirmedEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var encodeEmailToken = Encoding.UTF8.GetBytes(confirmedEmailToken);
                    var validEmailToken = WebEncoders.Base64UrlEncode(encodeEmailToken);


                    //this do later to have confirmation email

                    string url = $"{_configuration["appURL"]}api/authentication/confirmEmail?userid={user.Id}&token={validEmailToken}";


                    await _emailService.SendEmail(user.Email, Msg.ConfirmEmailMsg, $"<h1>{Msg.EmailMsgBody1}</h1>"
                        + $"<p>{Msg.EmailMsgBody2} <a href='{url}'>{Msg.EmailMsgBody3}</a></p>");

                    return new UserManagerResponse
                    {
                        Message = Msg.UserCreated,
                        IsSuccess = true,
                    };

                }


                return new UserManagerResponse
                {
                    Message = ErrorMsg.UserNotCreated,
                    IsSuccess = false,
                    Errors = result.Errors.Select(e=> e.Description)
                };

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            try
            {

                var user = await  _userManager.FindByEmailAsync(model.Email);

                if(user == null)
                {
                    return new UserManagerResponse
                    {
                        Message = ErrorMsg.NoUserEmail,
                        IsSuccess = false,
                    };
                }

                if(model.NewPassword != model.ConfirmPassword)
                {
                    return new UserManagerResponse
                    {
                        Message = ErrorMsg.EmailNotConfirm,
                        IsSuccess = false,
                    };
                }
                var dToken = WebEncoders.Base64UrlDecode(model.Token);
                var nToken = Encoding.UTF8.GetString(dToken);

                var result = await _userManager.ResetPasswordAsync(user, nToken, model.NewPassword);
                if (result.Succeeded)
                {
                    return new UserManagerResponse
                    {
                        Message = Msg.ResetPasswordSuccess,
                        IsSuccess = true,
                    };
                }
                return new UserManagerResponse
                {
                    Message = ErrorMsg.GeneralErrorMsg,
                    IsSuccess = false,
                    Errors = result.Errors.Select(e => e.Description),
                };
        
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
