using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Domain;
using API.Domain.AccountService;
using API.Dtos;
using API.Helpers;
using Core;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class AccountController:BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signManager;

        private readonly ITokenService _tokenService;

        private readonly FacebookLoginService _facebookLoginService;
        private readonly AccountService _accountService;

        private static HttpClient client = new HttpClient();


        private readonly  string ClientID = "629483562400919";
        private readonly string ClientSecret = "df041939538481f9207c1b35127597a2";

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signManager,ITokenService tokenService,FacebookLoginService facebookLoginService,AccountService accountService)
        {
            _signManager = signManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _facebookLoginService = facebookLoginService;
            _accountService = accountService;
        }

        // [Cache(600)]
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            // var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            var user = await _userManager.FindByEmailAsync(email);


            var userDto =  new UserDto(user.Email,user.Name,user.Token,user.PictureUrl);
            return Ok(userDto);
        }




        [HttpGet("Emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            //Login Natively

            if (loginDto.NativeValid)
            {
                var (succeeded, userDto, errorMessage) = await _accountService.LoginServiceAsync(loginDto);
                if (succeeded)
                {
                    return Ok(userDto);
                }

                return Unauthorized(errorMessage);               
            }

            //Login with Facebook

            if(loginDto.FBValid)
            {
                 var (succeeded, userDto, errorMessage) = await _accountService.FacebookLoginAsync(loginDto);
                if (succeeded)
                {
                    return Ok(userDto);
                }

                return Unauthorized(errorMessage);                          
            }

            return Unauthorized("Please enter valid email and password..."); 

        }


        [HttpPost("register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto registerdto)
        {
            var (succeeded, userDto, errorMessage) = await _accountService.RegisterServiceAsync(registerdto);
            if (succeeded)
            {
                return Ok(userDto);
            }
            return BadRequest(errorMessage);  
        }





        [HttpGet("Profile")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUserProfile()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            return new UserDto(user.Email,user.Name,_tokenService.CreateToken(user,"Member"),user.PictureUrl);
        }



        [HttpGet("Url")]
        public string GetLoginUrl([FromQuery] string redirect_url)
        {
            return _facebookLoginService.GetLoginUrl(redirect_url);
        }

        [HttpGet("Tokens")]
        public async Task<FacebookTokenDto> GetTokensByAuthToken([FromQuery] string authToken,[FromQuery] string callBackUrl)
        {
            Console.WriteLine(authToken,callBackUrl);
            return await _facebookLoginService.GetTokenByAuthToken(authToken,callBackUrl);
        }


        [HttpGet("FacebookProfile/{accessToken}")]

        public async Task<UserProfileDto> GetUserProfileByAccessToken(string accessToken)
        {
            
            var FacebookProfile =  await _facebookLoginService.GetUserProfileByAccessToken(accessToken);
            Console.WriteLine(FacebookProfile);
            return FacebookProfile;
        }

    }
}