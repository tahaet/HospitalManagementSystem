using Hospital.APIs.Service.IService;
using Hospital.DataAccess.Data;
using Hospital.Models.Dto.AuthDto;
using Hospital.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hospital.APIs.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppDbContext db;
        private readonly IJwtTokenGenerator jwtTokenGenerator;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IJwtTokenGenerator jwtTokenGenerator)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.db = db;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = await db.ApplicationUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if (await roleManager.RoleExistsAsync(roleName))
                {
                    await userManager.AddToRoleAsync(user, roleName);
                    return true;
                }
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = db.ApplicationUsers.FirstOrDefault(x => x.UserName.ToLower() == loginRequestDto.Email.ToLower());
            bool isValid = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (user is null || !isValid)
            {
                return new LoginResponseDto()
                {
                    User = null,
                    Token = ""
                };
            }
            var roles = await userManager.GetRolesAsync(user);
            var token = jwtTokenGenerator.GenerateToken(user, roles);
            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };
            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber,
            };
            try
            {
                var result = await userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = db.ApplicationUsers.FirstOrDefault(x => x.Email == registrationRequestDto.Email);
                    UserDto userDto = new UserDto()
                    {
                        Id = userToReturn.Id,
                        Name = userToReturn.Name,
                        Email = userToReturn.Email,
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }

            }
            catch (Exception ex)
            {

            }
            return "Error encountered";
        }
    }
}
