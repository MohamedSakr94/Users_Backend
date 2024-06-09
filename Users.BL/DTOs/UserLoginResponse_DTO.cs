using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.BL
{
    public class UserLoginResponse_DTO
    {
        public UserRead_DTO? User { get; set; }

        public string? Token { get; set; }
    }
}
