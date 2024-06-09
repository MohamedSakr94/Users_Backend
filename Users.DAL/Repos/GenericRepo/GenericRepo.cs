using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.DAL
{
    public class GenericRepo<TEntity> : IGenericRepo<TEntity> where TEntity : class
    {
        private readonly UsersContext _context;

        #region Constructor
        public GenericRepo(UsersContext context)
        {
            this._context = context;
        }
        #endregion

        #region CRUD
        public List<TEntity> GetAll()
        {
            return _context.Set<TEntity>()
                .AsNoTracking()
                .ToList();
        }

        public TEntity? GetById(string id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public void Add(TEntity TEntity)
        {
            _context.Set<TEntity>().Add(TEntity);
        }
        public void Update(TEntity TEntity)
        {
            _context.Set<TEntity>().Update(TEntity);
        }

        public void Delete(string id)
        {
            var entity = _context.Set<TEntity>().Find(id)!;
            _context.Set<TEntity>().Remove(entity);
        }
        #endregion
    }
}
