using HiEIS.Data.Infrastructures;
using HiEIS.Data.Repositories;
using HiEIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HiEIS.Service
{
    public interface IMyUserService
    {
        IQueryable<MyUser> GetMyUsers(Expression<Func<MyUser, bool>> where);
        MyUser GetMyUser(string id);
        void SaveChanges();
    }
    public class MyUserService : IMyUserService
    {
        private readonly IMyUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MyUserService(IMyUserRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public MyUser GetMyUser(string id)
        {
            return _repository.GetById(id);
        }

        public IQueryable<MyUser> GetMyUsers(Expression<Func<MyUser, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }
    }
}
