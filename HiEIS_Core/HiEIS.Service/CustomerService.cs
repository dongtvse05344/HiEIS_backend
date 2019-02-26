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
    public interface ICustomerService
    {
        IQueryable<Customer> GetCustomers();
        IQueryable<Customer> GetCustomers(Expression<Func<Customer, bool>> where);
        Customer GetCustomer(Guid id);
        void CreateCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
        void SaveChanges();
    }

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(ICustomerRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void CreateCustomer(Customer customer)
        {
            _repository.Add(customer);
        }

        public void DeleteCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomer(Guid id)
        {
            return _repository.GetById(id);
        }

        public IQueryable<Customer> GetCustomers()
        {
            return _repository.GetAll();
        }

        public IQueryable<Customer> GetCustomers(Expression<Func<Customer, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void UpdateCustomer(Customer customer)
        {
            _repository.Update(customer);
        }
    }
}
