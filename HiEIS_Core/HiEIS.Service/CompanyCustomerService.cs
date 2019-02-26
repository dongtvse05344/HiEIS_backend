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
    public interface ICompanyCustomerService
    {
        IQueryable<CompanyCustomer> GetCompanyCustomers();
        IQueryable<CompanyCustomer> GetCompanyCustomers(Expression<Func<CompanyCustomer, bool>> where);
        CompanyCustomer GetCompanyCustomer(Guid id);
        void CreateCompanyCustomer(CompanyCustomer companyCustomer);
        void UpdateCompanyCustomer(CompanyCustomer companyCustomer);
        void DeleteCompanyCustomer(CompanyCustomer companyCustomer);
        void SaveChanges();
    }

    public class CompanyCustomerService : ICompanyCustomerService
    {
        private readonly ICompanyCustomerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CompanyCustomerService(ICompanyCustomerRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void CreateCompanyCustomer(CompanyCustomer companyCustomer)
        {
            _repository.Add(companyCustomer);
        }

        public void DeleteCompanyCustomer(CompanyCustomer companyCustomer)
        {
            throw new NotImplementedException();
        }

        public CompanyCustomer GetCompanyCustomer(Guid id)
        {
            return _repository.GetById(id);
        }

        public IQueryable<CompanyCustomer> GetCompanyCustomers()
        {
            return _repository.GetAll();
        }

        public IQueryable<CompanyCustomer> GetCompanyCustomers(Expression<Func<CompanyCustomer, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void UpdateCompanyCustomer(CompanyCustomer companyCustomer)
        {
            _repository.Update(companyCustomer);
        }
    }
}
