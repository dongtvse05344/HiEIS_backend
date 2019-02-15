using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HiEIS.Data.Infrastructures
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Add(IEnumerable<T> list);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        T GetById(Guid id);
        T GetById(string id);
        T Get(Expression<Func<T, bool>> where);
        IQueryable<T> GetAll();
        IQueryable<T> GetMany(Expression<Func<T, bool>> where);
    }
    public abstract class Repository<T> where T : class
    {
        #region Properties
        private HiEISDbContext dbContext;
        private readonly DbSet<T> dbSet;

        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected HiEISDbContext DbContext
        {
            get { return dbContext ?? (dbContext = DbFactory.Init()); }
        }
        #endregion

        protected Repository(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }

        #region Implementation
        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Add(IEnumerable<T> list)
        {
            dbSet.AddRange(list);
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
        }
        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
            {
                dbSet.Remove(obj);
            }
        }
        public virtual T GetById(Guid id)
        {
            return dbSet.Find(id);
        }
        public virtual T GetById(string id)
        {
            return dbSet.Find(id);
        }
        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault();
        }
        public virtual IQueryable<T> GetAll()
        {
            return dbSet;
        }
        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where);
        }
        #endregion
    }
}
