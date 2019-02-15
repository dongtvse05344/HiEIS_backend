using HiEIS.Data.Infrastructures;
using HiEIS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Data.Repositories
{
    public interface IMyUserRepository  : IRepository<MyUser>
    {

    }
    public class MyUserRepository : Repository<MyUser>, IMyUserRepository
    {
        public MyUserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
