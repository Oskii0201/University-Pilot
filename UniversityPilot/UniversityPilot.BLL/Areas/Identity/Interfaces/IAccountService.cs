using UniversityPilot.BLL.Areas.Identity.DTO;

namespace UniversityPilot.BLL.Areas.Identity.Interfaces
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);

        public string GenerateJwt(LoginDto dto);

        public UserDetailsDto GetUserDetails(int id);

        public void Edit(int id, UserDetailsDto userDetails);

        public void ChangePassword(int id, ChangePasswordDto changePassword);
    }
}