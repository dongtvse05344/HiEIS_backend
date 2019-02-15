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
    public interface IStaffService
    {
        IQueryable<Staff> GetStaffs();
        IQueryable<Staff> GetStaffs(Expression<Func<Staff,bool>> where);
        Staff GetStaff(string id);
        void CreateStaff(Staff staff);
        void UpdateStaff(Staff staff);
        void DeleteStaff(Staff staff);
        void SaveChanges();
    }
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public StaffService(IStaffRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void CreateStaff(Staff staff)
        {
            _repository.Add(staff);
        }

        public void DeleteStaff(Staff staff)
        {
            _repository.Delete(staff);
        }

        public Staff GetStaff(string id)
        {
            return _repository.GetById(id);
        }

        public IQueryable<Staff> GetStaffs()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Staff> GetStaffs(Expression<Func<Staff, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void UpdateStaff(Staff staff)
        {
            _repository.Update(staff);
        }
    }
}
