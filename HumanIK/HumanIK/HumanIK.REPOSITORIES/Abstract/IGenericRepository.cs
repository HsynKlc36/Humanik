using HumanIK.ENTITIES.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.REPOSITORIES.Abstract
{
    public interface IGenericRepository<T> where T : class, IBaseEntity
    {
        T GetById(int id);
        T GetByDefault(Expression<Func<T, bool>> exp);
        List<T> GetDefault(Expression<Func<T, bool>> exp);
        List<T> GetAll();
        bool Add(T item);
        bool Remove(int id);
        bool Remove(Expression<Func<T, bool>> exp);
        bool Activate(int id);
        bool Update(T item);
        bool Update(List<T> items);
        bool Save();
    }
}
