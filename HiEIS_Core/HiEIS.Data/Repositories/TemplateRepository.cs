using HiEIS.Data.Infrastructures;
using HiEIS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Data.Repositories
{
    public interface ITemplateRepository : IRepository<Template>
    {

    }
    public class TemplateRepository : Repository<Template>, ITemplateRepository
    {
        public TemplateRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
