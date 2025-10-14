using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Application.Features.Identity.Dto.Requests;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Domain.RepositoriesInterfaces;
using MetaBlog.Domain.RefreshTokens;
namespace MetaBlog.Infrastructure.Identity
{
    

    public class IdentityService(UserManager<IdentityAppUser> userManager,IJwtService jwtService,IRefreshTokenRepository refreshTokenRepository) 
        : IIdentityService

    {
        public async Task<Result<Guid>> RegisterUserAsync( string Email, string password)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                var newUser = new IdentityAppUser
                {

                    UserName = Email,
                    Email = Email
                };
                var createResult = await userManager.CreateAsync(newUser, password);

                if (createResult.Succeeded)
                {
                    var roleResult = await userManager.AddToRoleAsync(newUser, "User");
                    if (roleResult.Succeeded) { return newUser.Id; }
                }
                
            }

            return Error.Conflict("Email exist");

        }

        public async Task<Result<(Guid,List<string>)>> LoginAsync(string Email, string Password)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null)
                return Error.NotFound("User not found");
            // check password
            var isPasswordValid = await userManager.CheckPasswordAsync(user, Password);
            if (!isPasswordValid)
            {
                return Error.Unauthorized("Invalid password");
            }
             var list = await userManager.GetRolesAsync(user);
            return (user.Id,list.ToList());
        }

        public async Task<Result<object>> ChangePasswordAsync(string Email, string currentPassword, string newPassword)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null)
                return Error.NotFound("User not found");
            var isCurrentPasswordValid = await userManager.CheckPasswordAsync(user, currentPassword);
            if (!isCurrentPasswordValid)
                return Error.Unauthorized("Current password is incorrect");
            var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (result.Succeeded)
                return Result.Success;
            var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description)).ToList();
            return errors;
        }

        public async Task<Result<List<string>>> GetUserRolesAsync(Guid Id)
        {
            var user = await userManager.FindByIdAsync(Id.ToString());
            if (user == null)
                return Error.NotFound("User not found");
            var list = await userManager.GetRolesAsync(user);
            return list.ToList();
       
        }

        public async Task<Result<string>> GetUserEmailAsync(Guid Id)
        {
           var user= await userManager.FindByIdAsync(Id.ToString());
            if (user == null)
                return Error.NotFound("User not found");
            
            return user.Email!;

        }
    }

}
