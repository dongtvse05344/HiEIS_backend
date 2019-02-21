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
    public interface IProductService
    {
        IQueryable<Product> GetProducts();
        IQueryable<Product> GetProducts(Expression<Func<Product,bool>> where);
        Product GetProduct(Guid id);
        void CreateProduct(Product product);
        void UpadteProduct(Product product);
        void DeleteProduct(Product product);
        void SaveChanges();
    }
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void CreateProduct(Product product)
        {
            _repository.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            _repository.Delete(product);
        }

        public Product GetProduct(Guid id)
        {
            return _repository.GetById(id);
        }

        public IQueryable<Product> GetProducts()
        {
            return _repository.GetAll();
        }

        public IQueryable<Product> GetProducts(Expression<Func<Product, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void UpadteProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
