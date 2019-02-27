using HiEIS.Data.Infrastructures;
using HiEIS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Data.Repositories
{
    public interface IInvoiceItemRepository : IRepository<InvoiceItem>
    {

    }

    public class InvoiceItemRepository : Repository<InvoiceItem>, IInvoiceItemRepository
    {
        public InvoiceItemRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
