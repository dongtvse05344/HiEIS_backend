using HiEIS.Data.Infrastructures;
using HiEIS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Data.Repositories
{
    public interface ICompanyCustomerRepository : IRepository<CompanyCustomer>
    {

    }

    public class CompanyCustomerRepository : Repository<CompanyCustomer>, ICompanyCustomerRepository
    {
        public CompanyCustomerRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
