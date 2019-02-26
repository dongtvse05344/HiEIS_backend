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
    public interface ICustomerProductService
    {
        IQueryable<CustomerProduct> GetCustomerProducts();
        IQueryable<CustomerProduct> GetCustomerProducts(Expression<Func<CustomerProduct, bool>> where);
        CustomerProduct GetCustomerProduct(Guid id);
        void CreateCustomerProduct(CustomerProduct customerProduct);
        void UpdateCustomerProduct(CustomerProduct customerProduct);
        void DeleteCustomerProduct(CustomerProduct customerProduct);
        void SaveChanges();
    }

    public class CustomerProductService : ICustomerProductService
    {
        private readonly ICustomerProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerProductService(ICustomerProductRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void CreateCustomerProduct(CustomerProduct customerProduct)
        {
            _repository.Add(customerProduct);
        }

        public void DeleteCustomerProduct(CustomerProduct customerProduct)
        {
            throw new NotImplementedException();
        }

        public CustomerProduct GetCustomerProduct(Guid id)
        {
            return _repository.GetById(id);
        }

        public IQueryable<CustomerProduct> GetCustomerProducts()
        {
            return _repository.GetAll();
        }

        public IQueryable<CustomerProduct> GetCustomerProducts(Expression<Func<CustomerProduct, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void UpdateCustomerProduct(CustomerProduct customerProduct)
        {
            _repository.Update(customerProduct);
        }
    }
}
