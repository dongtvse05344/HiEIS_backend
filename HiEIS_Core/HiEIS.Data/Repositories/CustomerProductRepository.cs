using HiEIS.Data.Infrastructures;
using HiEIS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Data.Repositories
{
    public interface ICustomerProductRepository : IRepository<CustomerProduct>
    {

    }
    public class CustomerProductRepository : Repository<CustomerProduct>, ICustomerProductRepository
    {
        public CustomerProductRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
