using Hospital.Models;

namespace Hospital.APIs.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> Roles);
    }
}
