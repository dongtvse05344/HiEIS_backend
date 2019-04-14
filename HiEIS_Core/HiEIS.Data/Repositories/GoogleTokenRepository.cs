using HiEIS.Data.Infrastructures;
using HiEIS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Data.Repositories
{
    public interface IGoogleTokenRepository : IRepository<GoogleToken>
    {

    }

    public class GoogleTokenRepository : Repository<GoogleToken>, IGoogleTokenRepository
    {
        public GoogleTokenRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
