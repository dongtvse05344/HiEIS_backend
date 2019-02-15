using HiEIS.Data.Infrastructures;
using HiEIS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Data.Repositories
{
    public interface IStaffRepository : IRepository<Staff>
    {

    }
    public class StaffRepository : Repository<Staff>, IStaffRepository
    {
        public StaffRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
