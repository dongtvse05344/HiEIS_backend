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
    public interface ICompanyService
    {
        IQueryable<Company> GetCompanys();
        IQueryable<Company> GetCompanys(Expression<Func<Company, bool>> where);
        Company GetCompany(Guid id);
        void CreateCompany(Company company);
        void UpdateCompany(Company company);
        void DeleteCompany(Company company);
        void SaveChanges();
    }
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(ICompanyRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void CreateCompany(Company company)
        {
            company.IsActive = true;
            _repository.Add(company);
        }

        public void DeleteCompany(Company company)
        {
            _repository.Delete(company);
        }

        public Company GetCompany(Guid id)
        {
            return _repository.GetById(id);
        }

        public IQueryable<Company> GetCompanys()
        {
            return _repository.GetAll();
        }

        public IQueryable<Company> GetCompanys(Expression<Func<Company, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void UpdateCompany(Company company)
        {
            _repository.Update(company);
        }
    }
}
