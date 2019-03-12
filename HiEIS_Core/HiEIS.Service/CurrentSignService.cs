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
    public interface ICurrentSignService
    {
        IQueryable<CurrentSign> GetCurrentSigns();
        IQueryable<CurrentSign> GetCurrentSigns(Expression<Func<CurrentSign, bool>> where);
        CurrentSign GetCurrentSign(Guid id);
        void CreateCurrentSign(CurrentSign currentSign);
        void UpdateCurrentSign(CurrentSign currentSign);
        void DeleteCurrentSign(CurrentSign currentSign);
        void SaveChanges();
    }

    public class CurrentSignService : ICurrentSignService
    {
        private readonly ICurrentSignRepository _currentSignRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CurrentSignService(ICurrentSignRepository currentSignRepository, IUnitOfWork unitOfWork)
        {
            _currentSignRepository = currentSignRepository;
            _unitOfWork = unitOfWork;
        }

        public void CreateCurrentSign(CurrentSign currentSign)
        {
            _currentSignRepository.Add(currentSign);
        }

        public void DeleteCurrentSign(CurrentSign currentSign)
        {
            _currentSignRepository.Delete(currentSign);
        }

        public CurrentSign GetCurrentSign(Guid id)
        {
            return _currentSignRepository.GetById(id);
        }

        public IQueryable<CurrentSign> GetCurrentSigns()
        {
            return _currentSignRepository.GetAll();
        }

        public IQueryable<CurrentSign> GetCurrentSigns(Expression<Func<CurrentSign, bool>> where)
        {
            return _currentSignRepository.GetMany(where);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void UpdateCurrentSign(CurrentSign currentSign)
        {
            _currentSignRepository.Update(currentSign);
        }
    }
}
