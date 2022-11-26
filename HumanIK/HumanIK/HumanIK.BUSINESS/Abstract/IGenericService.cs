using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.BUSINESS.Abstract
{
    public interface IGenericService<T>
    {
        T GetById(int id);
        List<T> GetDefault(Expression<Func<T, bool>> exp);
        T GetByDefault(Expression<Func<T, bool>> exp);
        List<T> GetAll();
        bool Add(T item);
        bool Remove(int id);
        bool Remove(Expression<Func<T, bool>> exp);
        bool Activate(int id);
        bool Update(T item);
        bool Update(List<T> item);
    }
}
