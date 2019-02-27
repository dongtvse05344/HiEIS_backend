using HiEIS.Data.Infrastructures;
using HiEIS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Data.Repositories
{
    public interface IProformaInvoiceRepository : IRepository<ProformaInvoice>
    {

    }
    public class ProformaInvoiceRepository : Repository<ProformaInvoice>, IProformaInvoiceRepository
    {
        public ProformaInvoiceRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
