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
    public interface IProformaInvoiceService
    {
        IQueryable<ProformaInvoice> GetProformaInvoices();
        IQueryable<ProformaInvoice> GetProformaInvoices(Expression<Func<ProformaInvoice, bool>> where);
        ProformaInvoice GetProformaInvoice(Guid id);
        void CreateProformaInvoice(ProformaInvoice proformaInvoice);
        void UpdateProformaInvoice(ProformaInvoice proformaInvoice);
        void DeleteProformaInvoice(ProformaInvoice proformaInvoice);
        void SaveChanges();
    }

    public class ProformaInvoiceService : IProformaInvoiceService
    {
        private readonly IProformaInvoiceRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProformaInvoiceService(IProformaInvoiceRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void CreateProformaInvoice(ProformaInvoice proformaInvoice)
        {
            _repository.Add(proformaInvoice);
        }

        public void DeleteProformaInvoice(ProformaInvoice proformaInvoice)
        {
            throw new NotImplementedException();
        }

        public ProformaInvoice GetProformaInvoice(Guid id)
        {
            return _repository.GetById(id);
        }

        public IQueryable<ProformaInvoice> GetProformaInvoices()
        {
            return _repository.GetAll();
        }

        public IQueryable<ProformaInvoice> GetProformaInvoices(Expression<Func<ProformaInvoice, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void UpdateProformaInvoice(ProformaInvoice proformaInvoice)
        {
            _repository.Update(proformaInvoice);
        }
    }
}
