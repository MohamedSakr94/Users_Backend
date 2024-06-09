using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.DAL
{
    public interface IGenericRepo<TEntity> where TEntity : class
    {
        List<TEntity> GetAll();
        TEntity? GetById(string id);
        void Add(TEntity TEntity);
        void Update(TEntity TEntity);
        void Delete(string id);
    }
}
