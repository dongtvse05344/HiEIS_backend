using HiEIS.Data.Infrastructures;
using HiEIS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Data.Repositories
{
    public interface ICompanyRepository : IRepository<Company>
    {

    }
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
