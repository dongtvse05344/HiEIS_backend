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
    public interface IProformaInvoiceItemService
    {
        IQueryable<ProformaInvoiceItem> GetProformaInvoiceItems();
        IQueryable<ProformaInvoiceItem> GetProformaInvoiceItems(Expression<Func<ProformaInvoiceItem, bool>> where);
        ProformaInvoiceItem GetProformaInvoiceItem(Guid id);
        void CreateProformaInvoiceItem(ProformaInvoiceItem proformaInvoiceItem);
        void UpdateProformaInvoiceItem(ProformaInvoiceItem proformaInvoiceItem);
        void DeleteProformaInvoiceItem(ProformaInvoiceItem proformaInvoiceItem);
        void SaveChanges();
    }

    public class ProformaInvoiceItemService : IProformaInvoiceItemService
    {
        private readonly IProformaInvoiceItemRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProformaInvoiceItemService(IProformaInvoiceItemRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void CreateProformaInvoiceItem(ProformaInvoiceItem proformaInvoiceItem)
        {
            _repository.Add(proformaInvoiceItem);
        }

        public void DeleteProformaInvoiceItem(ProformaInvoiceItem proformaInvoiceItem)
        {
            throw new NotImplementedException();
        }

        public ProformaInvoiceItem GetProformaInvoiceItem(Guid id)
        {
            return _repository.GetById(id);
        }

        public IQueryable<ProformaInvoiceItem> GetProformaInvoiceItems()
        {
            return _repository.GetAll();
        }

        public IQueryable<ProformaInvoiceItem> GetProformaInvoiceItems(Expression<Func<ProformaInvoiceItem, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void UpdateProformaInvoiceItem(ProformaInvoiceItem proformaInvoiceItem)
        {
            _repository.Update(proformaInvoiceItem);
        }
    }
}
