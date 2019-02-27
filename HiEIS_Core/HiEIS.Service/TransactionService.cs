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
    public interface ITransactionService
    {
        IQueryable<Transaction> GetTransactions();
        IQueryable<Transaction> GetTransactions(Expression<Func<Transaction, bool>> where);
        Transaction GetTransaction(Guid id);
        void CreateTransaction(Transaction transaction);
        void UpdateTransaction(Transaction transaction);
        void DeleteTransaction(Transaction transaction);
        void SaveChanges();
    }

    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(ITransactionRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void CreateTransaction(Transaction transaction)
        {
            _repository.Add(transaction);
        }

        public void DeleteTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Transaction GetTransaction(Guid id)
        {
            return _repository.GetById(id);
        }

        public IQueryable<Transaction> GetTransactions()
        {
            return _repository.GetAll();
        }

        public IQueryable<Transaction> GetTransactions(Expression<Func<Transaction, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void UpdateTransaction(Transaction transaction)
        {
            _repository.Update(transaction);
        }
    }
}
