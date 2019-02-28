﻿using HiEIS.Data.Infrastructures;
using HiEIS.Data.Repositories;
using HiEIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HiEIS.Service
{
    public interface IInvoiceItemService
    {
        IQueryable<InvoiceItem> GetInvoiceItems();
        IQueryable<InvoiceItem> GetInvoiceItems(Expression<Func<InvoiceItem, bool>> where);
        InvoiceItem GetInvoiceItem(Guid id);
        void CreateInvoiceItem(InvoiceItem invoiceItem);
        void UpdateInvoiceItem(InvoiceItem invoiceItem);
        void DeleteInvoiceItem(InvoiceItem invoiceItem);
        void SaveChanges();
    }

    public class InvoiceItemService : IInvoiceItemService
    {
        private readonly IInvoiceItemRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceItemService(IInvoiceItemRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void CreateInvoiceItem(InvoiceItem invoiceItem)
        {
            _repository.Add(invoiceItem);
        }

        public void DeleteInvoiceItem(InvoiceItem invoiceItem)
        {
            throw new NotImplementedException();
        }

        public InvoiceItem GetInvoiceItem(Guid id)
        {
            return _repository.GetById(id);
        }

        public IQueryable<InvoiceItem> GetInvoiceItems()
        {
            return _repository.GetAll();
        }

        public IQueryable<InvoiceItem> GetInvoiceItems(Expression<Func<InvoiceItem, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void UpdateInvoiceItem(InvoiceItem invoiceItem)
        {
            _repository.Update(invoiceItem);
        }
    }
}