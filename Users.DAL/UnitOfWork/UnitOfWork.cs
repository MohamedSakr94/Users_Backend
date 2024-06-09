using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.DAL
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly UsersContext _context;
        public IUserRepo UserRepo { get; }

        #region Constructor
        public UnitOfWork(UsersContext context,
            IUserRepo userRepo)
        {
            this._context = context;
            UserRepo = userRepo;
        }
        #endregion
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
