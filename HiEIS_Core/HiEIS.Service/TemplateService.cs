using HiEIS.Data.Infrastructures;
using HiEIS.Data.Repositories;
using HiEIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HiEIS.Service
{
    public interface ITemplateService
    {
        IQueryable<Template> GetTemplates();
        IQueryable<Template> GetTemplates(Expression<Func<Template,bool>> where);
        Template GetTemplate(Guid id);
        void CreateTemplate(Template template);
        void UpdateTemplate(Template template);
        void DeleteTemplate(Template template);
        void SaveChanges();
    }

    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public TemplateService(ITemplateRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void CreateTemplate( Template template)
        {
            template.IsActive = true;
            _repository.Add(template);
        }

        public void DeleteTemplate( Template template)
        {
            template.IsActive = false;
            _repository.Update(template);
        }

        public Template GetTemplate(Guid id)
        {
            return _repository.GetById(id);
        }

        public IQueryable<Template> GetTemplates()
        {
            return _repository.GetAll();
        }

        public IQueryable<Template> GetTemplates(Expression<Func<Template, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void UpdateTemplate( Template template)
        {
            _repository.Update(template);
        }
    }
}
