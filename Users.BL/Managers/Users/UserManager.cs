using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.DAL;

namespace Users.BL
{
    public class UserManager : IUserManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //////////////////////////////////////////

        #region Getters
        public List<UserRead_DTO> GetAll()
        {
            List<User> dbUsers = _unitOfWork.UserRepo.GetAll();
            return dbUsers.Select(u => new UserRead_DTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                IsDisabled = u.IsDisabled,
                IsAdmin = u.IsAdmin,
            }).ToList();
        }

        public UserRead_DTO? GetById(string id)
        {
            User? dbUser = _unitOfWork.UserRepo.GetById(id);

            if (dbUser is null) return null;

            UserRead_DTO ReadUser = new()
            {
                Id = dbUser.Id,
                FirstName = dbUser.FirstName,
                LastName = dbUser.LastName,
                Email = dbUser.Email,
                IsDisabled = dbUser.IsDisabled,
                IsAdmin = dbUser.IsAdmin
            };

            return ReadUser;
        }

        public UserRead_DTO? GetByEmailAndPassword(UserLogin_DTO guest)
        {
            guest.Password = Helpers.HashPassword(guest.Password);

            User? dbUser = _unitOfWork.UserRepo.GetByEmailAndPassword(guest.Email, guest.Password);

            if (dbUser is null) return null;

            UserRead_DTO ReadUser = new()
            {

                Id = dbUser.Id,
                FirstName= dbUser.FirstName,
                LastName= dbUser.LastName,
                Email = dbUser.Email,
                IsDisabled = dbUser.IsDisabled,
                IsAdmin = dbUser.IsAdmin
            };

            return ReadUser;
        }

        public UserRead_DTO GetByEmail(string email)
        {
            User? dbUser = _unitOfWork.UserRepo.GetByEmail(email);
            if (dbUser is null) return null!;
            UserRead_DTO user = new()
            {
                Id = dbUser.Id,
                Email = dbUser.Email
            };
            return user;
        }
        #endregion

        #region Add
        public UserRead_DTO Add(UserAdd_DTO user)
        {
            User dbUser = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PasswordHash = Helpers.HashPassword(user.Password),
                IsAdmin = user.IsAdmin,
                IsDisabled = user.IsDisabled
            };
            _unitOfWork.UserRepo.Add(dbUser);
            _unitOfWork.SaveChanges();

            UserRead_DTO ReadUser = new()
            {
                Id= dbUser.Id,
                FirstName = dbUser.FirstName,
                LastName = dbUser.LastName,
                Email = dbUser.Email,
                IsAdmin = dbUser.IsAdmin,
                IsDisabled = dbUser.IsDisabled
            };
            return ReadUser;
        }
        #endregion

        #region Edit Users
        public bool Update(UserUpdate_DTO user)
        {
            User? dbUser = _unitOfWork.UserRepo.GetById(user.Id);

            if (dbUser is null) return false;

            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Email = user.Email;
            dbUser.PasswordHash = Helpers.HashPassword(user.Password);
            dbUser.IsDisabled = user.IsDisabled;
            dbUser.IsAdmin = user.IsAdmin;

            _unitOfWork.UserRepo.Update(dbUser);
            _unitOfWork.SaveChanges();

            return true;
        }

        public bool ChangeActive(UserEditActive_DTO user)
        {
            User? dbUser = _unitOfWork.UserRepo.GetById(user.Id);
            if (dbUser is null) return false;

            dbUser.IsDisabled = user.IsDisabled;

            _unitOfWork.UserRepo.Update(dbUser);
            _unitOfWork.SaveChanges();

            return true;
        }

        public bool ChangePassword(UserChangePassword_DTO user)
        {
            User? dbUser = _unitOfWork.UserRepo.GetById(user.Id);
            if (dbUser is null) return false;

            user.OldPassword = Helpers.HashPassword(user.OldPassword);
            user.NewPassword = Helpers.HashPassword(user.NewPassword);

            if (user.OldPassword != dbUser.PasswordHash || user.NewPassword == dbUser.PasswordHash) return false;

            dbUser.PasswordHash = user.NewPassword;

            _unitOfWork.UserRepo.Update(dbUser);
            _unitOfWork.SaveChanges();

            return true;
        }

        #endregion

        #region Delete
        public bool DeleteById(string id)
        {
            User? dbUser = _unitOfWork.UserRepo.GetById(id);
            if (dbUser is null) return false;

            _unitOfWork.UserRepo.Delete(dbUser.Id);
            _unitOfWork.SaveChanges();

            return true;
        }

        public bool Delete(UserDelete_DTO user)
        {
            return DeleteById(user.Id);
        }
        #endregion

    }
}
