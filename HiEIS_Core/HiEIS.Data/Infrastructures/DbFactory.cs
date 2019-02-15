using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Data.Infrastructures
{
    public interface IDbFactory : IDisposable
    {
        HiEISDbContext Init();
    }

    public class DbFactory : Disposable, IDbFactory
    {
        HiEISDbContext dbContext;

        public HiEISDbContext Init()
        {
            return dbContext ?? (dbContext = new HiEISDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
