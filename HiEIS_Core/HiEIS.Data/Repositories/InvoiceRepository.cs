using HiEIS.Data.Infrastructures;
using HiEIS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Data.Repositories
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {

    }
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
