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
    public interface IGoogleTokenService
    {
        IQueryable<GoogleToken> GetGoogleTokens();
        IQueryable<GoogleToken> GetGoogleTokens(Expression<Func<GoogleToken, bool>> where);
        GoogleToken GetGoogleToken(string id);
        void CreateGoogleToken(GoogleToken GoogleToken);
        void UpdateGoogleToken(GoogleToken GoogleToken);
        void DeleteGoogleToken(GoogleToken GoogleToken);
        void SaveChanges();
    }
    public class GoogleTokenService : IGoogleTokenService
    {
        private readonly IGoogleTokenRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public GoogleTokenService(IGoogleTokenRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void CreateGoogleToken(GoogleToken GoogleToken)
        {
            _repository.Add(GoogleToken);
        }

        public void DeleteGoogleToken(GoogleToken GoogleToken)
        {
            _repository.Delete(GoogleToken);
        }

        public GoogleToken GetGoogleToken(string id)
        {
            return _repository.GetById(id);
        }

        public IQueryable<GoogleToken> GetGoogleTokens()
        {
            return _repository.GetAll();
        }

        public IQueryable<GoogleToken> GetGoogleTokens(Expression<Func<GoogleToken, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void UpdateGoogleToken(GoogleToken GoogleToken)
        {
            _repository.Update(GoogleToken);
        }
    }
}
