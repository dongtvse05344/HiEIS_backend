using HiEIS.Data.Infrastructures;
using HiEIS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Data.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {

    }
    
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
