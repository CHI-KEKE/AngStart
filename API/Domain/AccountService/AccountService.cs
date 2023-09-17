using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using Core;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Domain.AccountService
{
    public class AccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;    
        private readonly FacebookLoginService _facebookLoginService;

        public AccountService(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,ITokenService tokenService,FacebookLoginService facebookLoginService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _facebookLoginService = facebookLoginService;
        }

    
        public async Task<(bool Succeeded, UserDto UserDto, string ErrorMessage)> LoginServiceAsync(LoginDto loginDto)
        {
            var userFound = await _userManager.FindByEmailAsync(loginDto.Email);
            if (userFound == null)
            {
                return (false, null, "User not found.");
            }

            var userHasCheckedPassword = await _signInManager.CheckPasswordSignInAsync(userFound, loginDto.Password, false);
            if (!userHasCheckedPassword.Succeeded)
            {
                return (false, null, "Found Email but Password is invalid.");
            }


            var userDto =  await GenerateUserDtoAsync(userFound);
            return (true, userDto, null);           
        }
        

        public async Task<(bool Succeeded, UserDto UserDto, string ErrorMessage)> FacebookLoginAsync(LoginDto loginDto)
        {           

            var FBuserProfile = await _facebookLoginService.GetUserProfileByAccessToken(loginDto.Token);
            var userFoundDB = await _userManager.FindByEmailAsync(FBuserProfile.Email);

            if(userFoundDB != null)
            {
                var userReturnDto = await GenerateUserDtoAsync(userFoundDB);
                return (true, userReturnDto, "Found the FB user in DB!");
            }

            var FBuser = new AppUser
            {
                Name = FBuserProfile.Name,
                Email = FBuserProfile.Email,
                UserName = FBuserProfile.Email,
                PictureUrl = FBuserProfile.Picture.Url,
            };

            var userDto = await GenerateNewUser(FBuser,"palceholder");

            return (true, userDto,"Create FB User Success!");
        }        


        public async Task<(bool Succeeded, UserDto UserDto, string ErrorMessage)> RegisterServiceAsync(RegisterDto registerDto)
        {
             var ExistingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if(ExistingUser != null) return (true, null,"Email has been registered!");


            var user = new AppUser
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                UserName = registerDto.Email,
                
            };

            var userDto = await GenerateNewUser(user,registerDto.Password);

            return (true, userDto,"Register successfully!!");
        }


        private async Task<UserDto> GenerateUserDtoAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            user.Token = _tokenService.CreateToken(user, role);
            await _userManager.UpdateAsync(user);

            var userDto = new UserDto(user.Email,user.Name,user.Token,user.PictureUrl);
            return userDto;
        }

        private async Task<UserDto> GenerateNewUser(AppUser user,string? password)
        {
            user.Token = _tokenService.CreateToken(user, "Member");
            var result = await _userManager.CreateAsync(user,password);

            await _userManager.AddToRoleAsync(user, "Member");

            var userDto = new UserDto(user.Email,user.Name,user.Token,user.PictureUrl);
            return userDto;
        }


        
    
    }

}
