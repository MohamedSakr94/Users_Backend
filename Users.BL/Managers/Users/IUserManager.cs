using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.BL
{
    public interface IUserManager
    {
        List<UserRead_DTO> GetAll(string? searchText = null, bool? isAdmin = null, bool? isDisabled = null);
        UserRead_DTO? GetById(string id);
        UserRead_DTO? GetByEmailAndPassword(UserLogin_DTO guest);
        UserRead_DTO GetByEmail(string email);
        UserReadWithDetails_DTO GetByIdWithDetails(string id);
        UserRead_DTO Add(UserAdd_DTO user);
        bool Update(UserUpdate_DTO user);
        bool ChangeActive(UserEditActive_DTO user);
        bool ChangePassword(UserChangePassword_DTO user);
        bool Delete(UserDelete_DTO user);
        bool DeleteById(string Id);
    }
}
