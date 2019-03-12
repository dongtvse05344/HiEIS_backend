using HiEIS.Data.Infrastructures;
using HiEIS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Data.Repositories
{
    public interface ICurrentSignRepository : IRepository<CurrentSign>
    {

    }

    public class CurrentSignRepository : Repository<CurrentSign>, ICurrentSignRepository
    {
        public CurrentSignRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
