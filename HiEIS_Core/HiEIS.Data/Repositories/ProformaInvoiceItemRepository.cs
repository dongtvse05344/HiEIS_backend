using HiEIS.Data.Infrastructures;
using HiEIS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Data.Repositories
{
    public interface IProformaInvoiceItemRepository : IRepository<ProformaInvoiceItem>
    {

    }
    
    public class ProformaInvoiceItemRepository : Repository<ProformaInvoiceItem>, IProformaInvoiceItemRepository
    {
        public ProformaInvoiceItemRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
