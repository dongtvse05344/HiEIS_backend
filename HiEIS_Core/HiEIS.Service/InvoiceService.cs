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
    public interface IInvoiceService
    {
        IQueryable<Invoice> GetInvoices();
        IQueryable<Invoice> GetInvoices(Expression<Func<Invoice, bool>> where);
        Invoice GetInvoice(Guid id);
        void CreateInvoice(Invoice invoice);
        void UpdateInvoice(Invoice invoice);
        void DeleteInvoice(Invoice invoice);
        void SaveChanges();
    }

    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceService(IInvoiceRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void CreateInvoice(Invoice invoice)
        {
            _repository.Add(invoice);
        }

        public void DeleteInvoice(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public Invoice GetInvoice(Guid id)
        {
            return _repository.GetById(id);
        }

        public IQueryable<Invoice> GetInvoices()
        {
            return _repository.GetAll();
        }

        public IQueryable<Invoice> GetInvoices(Expression<Func<Invoice, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void UpdateInvoice(Invoice invoice)
        {
            _repository.Update(invoice);
        }
    }
}
