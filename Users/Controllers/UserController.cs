using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Users.BL;
using Users.DAL;

namespace Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserManager _userManager;
        private readonly IConfiguration _configuration;

        public UserController(IUserManager userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Register")]
        public ActionResult<UserRead_DTO> Register(UserAdd_DTO guest)
        {
            UserRead_DTO? user = _userManager.GetByEmail(guest.Email);

            if (user == null) return _userManager.Add(guest);

            else return StatusCode(409);
        }


        [HttpPost]
        [Route("Login")]
        public ActionResult<UserLoginResponse_DTO> Login([FromForm] UserLogin_DTO guest)
        {
            UserRead_DTO? user = _userManager.GetByEmailAndPassword(guest);

            if (user == null) return Unauthorized();
            if (user.IsDisabled == true) return StatusCode(403);
            UserLoginResponse_DTO userLoginResopnse = new()
            {
                User = user,
                Token = Helpers.GenerateJWT_Token(_configuration, user)
            };

            return userLoginResopnse;

        }

        [HttpGet]
        [Route("Admin")]
        //[Authorize(AuthenticationSchemes = "JWT")]
        public ActionResult<List<UserRead_DTO>> GetAll(string? searchText = null, bool? isAdmin = null, bool? isDisabled = null)
        {
            List<UserRead_DTO> users = _userManager.GetAll(searchText, isAdmin, isDisabled);
            return users;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = "JWT")]
        public ActionResult<UserRead_DTO> GetById([FromHeader(Name = "Authorization")][Required] string Authorization, string id)
        {
            if (id == null) return BadRequest();

            UserRead_DTO? user = _userManager.GetById(id);

            if (user == null) return NotFound();

            return user;

        }


        [HttpPut]
        [Route("Edit")]
        [Authorize(AuthenticationSchemes = "JWT")]
        public ActionResult Update([FromHeader(Name = "Authorization")][Required] string Authorization, [FromForm] UserUpdate_DTO user)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != user.Id) return StatusCode(403);
            var userdb = _userManager.GetByIdWithDetails(user.Id);
            _userManager.Update(user);

            return StatusCode(202);
        }


        [HttpPost]
        [Route("Changepassword")]
        [Authorize(AuthenticationSchemes = "JWT")]
        public ActionResult ChangePassword([FromHeader(Name = "Authorization")][Required] string Authorization, [FromForm] UserChangePassword_DTO user)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != user.Id) return StatusCode(403);

            if (user.NewPassword == user.OldPassword || user.NewPassword != user.ConfirmPassword) return StatusCode(400);

            bool result = _userManager.ChangePassword(user);
            if (result == false) return StatusCode(400);
            return StatusCode(202);

        }


        [HttpDelete]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = "JWT")]
        public ActionResult Delete([FromHeader(Name = "Authorization")][Required] string Authorization, string id)
        {
            if (id == null) return BadRequest();
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == id) return StatusCode(409);
            var userTobeDeleted = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userTobeDeleted != null)
            {
                _userManager.DeleteById(id);
                return StatusCode(200);
            }
            return StatusCode(404);
        }

    }
}
