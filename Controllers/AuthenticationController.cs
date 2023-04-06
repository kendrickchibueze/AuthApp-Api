using AuthApp_Api.Models;
using AuthApp_Api.Services;
using AuthApp_Api.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AuthApp_Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {


        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;



        public AuthenticationController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }



















        //api/authentication/resetpassword
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromForm] ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ResetPasswordAsync(model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }

            return BadRequest(ErrorMsg.InvalidProperties);
        }


        //api/authentication/forgotpassword
        [HttpPost("forgetpassword")]
        public async Task<IActionResult> ForgetPasswordAsync(string email)
        {

            if(string.IsNullOrEmpty(email))
            {
                return NotFound();    
            }
            var result = await _userService.ForgetPasswordAsync(email); 
            if(result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }





        //api/authentication/confirmemail?userid&token
        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string token)
        {
            if(string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return NotFound();
            }
            var result  = await _userService.ConfirmEmailAsync(userId, token);
            if (result.IsSuccess)
            {
                return Redirect($"{_configuration["appURL"]}/confirmemail.html");

            }

            return BadRequest(result);
        }











        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest(ErrorMsg.InvalidProperties);
        }


        [HttpPost("Login")]
        public  async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var result  = await _userService.LoginUserAsync(model);
                    if (result.IsSuccess)
                    {
                        return Ok(result);
                    }

                    return BadRequest(result);
                }
                return BadRequest(ErrorMsg.InvalidProperties);
            }
            catch (Exception)
            {
                throw;
            }

        }









       /* public IActionResult Index()
        {
            return View();
        }*/
    }
}
