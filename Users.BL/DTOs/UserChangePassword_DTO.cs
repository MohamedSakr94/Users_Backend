using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.BL
{
    public class UserChangePassword_DTO
    {
        public string Id { get; set; } = null!;

        [DataType(DataType.Password)]
        public string OldPassword { get; set; } = null!;

        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
