using HiEIS.Data.Infrastructures;
using HiEIS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Data.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {

    }
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
