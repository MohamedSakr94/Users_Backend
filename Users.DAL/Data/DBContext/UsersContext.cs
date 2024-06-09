using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.DAL
{
    public class UsersContext : IdentityDbContext<User>
    {
        #region Constructor
        public UsersContext(DbContextOptions options) : base(options) { }
        #endregion

        #region DbSets
        override public DbSet<User> Users { get; set; }
        #endregion
    }
}
