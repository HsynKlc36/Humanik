using HumanIK.BUSINESS.Abstract;
using HumanIK.ENTITIES.Entities;
using HumanIK.REPOSITORIES.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace HumanIK.BUSINESS.Concrete
{
    
    public class GenericManager<T> : IGenericService<T> where T : class, IBaseEntity
    {
        private readonly IGenericRepository<T> _repository;
        private readonly Cloudinary _cloudinary;

        public GenericManager(IGenericRepository<T> repository)
        {
            _repository = repository;
            Account account = new Account(
                                        "drgl2pfow",
                                        "331271952486897",
                                        "bcSuOwtq7ap2gMzqODAV1xIHa6k");

            _cloudinary = new Cloudinary(account);
        }
        public bool Activate(int id)
        {
            if (id <= 0 || GetById(id) == null)
            {
                return false;
            }
            return _repository.Activate(id);
        }

        public bool Add(T item)
        {
            try
            {
                if (item == null)
                {
                    return false;
                }
                return _repository.Add(item);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T GetByDefault(Expression<Func<T, bool>> exp)
        {
            return _repository.GetByDefault(exp);
        }

        public T GetById(int id)
        {
            return _repository.GetById(id);
        }

        public List<T> GetDefault(Expression<Func<T, bool>> exp)
        {
            return _repository.GetDefault(exp);
        }

        public bool Remove(int id)
        {
            if (id <= 0|| GetById(id)==null)
            {
                return false;
            }
            return _repository.Remove(id);
        }

        public bool Remove(Expression<Func<T, bool>> exp)
        {
            return _repository.Remove(exp);
        }

        public bool Update(T item)
        {
            if (item == null)
            {
                return false;
            }
            return _repository.Update(item);
        }

        public bool Update(List<T> items)
        {
            if (items == null)
            {
                return false;
            }
            return _repository.Update(items);
        }


    }
}
