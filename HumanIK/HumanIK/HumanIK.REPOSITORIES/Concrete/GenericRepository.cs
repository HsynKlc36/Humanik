using HumanIK.ENTITIES.Entities;
using HumanIK.ENTITIES.Enums;
using HumanIK.REPOSITORIES.Abstract;
using HumanIK.REPOSITORIES.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace HumanIK.REPOSITORIES.Concrete
{

    public class GenericRepository<T> : IGenericRepository<T> where T : class, IBaseEntity
    {
        private readonly IKDbContext _db;

        public GenericRepository(IKDbContext db)
        {
            _db = db;
        }

        public bool Activate(int id)
        {
            T item = GetById(id);
            item.Status = Status.Active;
            return Save();
        }

        public bool Add(T item)
        {
            _db.Set<T>().Add(item);
            return Save();
        }

        public List<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }

        public T GetByDefault(Expression<Func<T, bool>> exp)
        {
            return _db.Set<T>().FirstOrDefault(exp);
        }

        public T GetById(int id)
        {
            return _db.Set<T>().Find(id);
        }

        public List<T> GetDefault(Expression<Func<T, bool>> exp)
        {
            return _db.Set<T>().Where(exp).ToList();
        }

        public bool Remove(int id)
        {
            T item = GetById(id);
            item.Status = Status.Passive;
            return Save();
        }

        public bool Remove(Expression<Func<T, bool>> exp)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    var items = GetDefault(exp);
                    int count = 0;

                    foreach (var item in items)
                    {
                        item.Status = Status.Passive;
                        bool result = Update(item);
                        if (result) count++;
                    }

                    if (items.Count == count) ts.Complete();
                    else return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }

        public bool Update(T item)
        {
            try
            {
                _db.Set<T>().Update(item);
                return Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Update(List<T> items)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    int count = 0;

                    foreach (var item in items)
                    {
                        bool result = Update(item);
                        if (result) count++;
                    }

                    if (items.Count == count) ts.Complete();
                    else return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
