using HiEIS.Model;
using HiEIS_Core.ViewModels;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.Utils
{
    public class MapsterConfig
    {
        public MapsterConfig()
        {
        }
        public void Run()
        {

            TypeAdapterConfig<EnterpriseTaxVM, CompanyVM>.NewConfig()
                                .Map(dest => dest.Name, src => src.Title)
                                .Map(dest => dest.Address, src => src.DiaChiCongTy);
            TypeAdapterConfig<Staff, StaffVM>.NewConfig()
                                .Map(dest => dest.UserName, src => src.MyUser.UserName)
                                .Map(dest => dest.Email, src => src.MyUser.Email)
                                .Map(dest => dest.Id, src => src.Id)
                                .Map(dest => dest.IsActive, src => src.MyUser.IsActive)
                                .Map(dest => dest.Name, src => src.Name)
                                .Map(dest => dest.PhoneNumber, src => src.MyUser.PhoneNumber)
                                .Map(dest => dest.CompanyId, src => src.Company.Id)
                                .Map(dest => dest.Code, src => src.Code)

                                ;

            TypeAdapterConfig<StaffUM, Staff >.NewConfig()
                                .Map(dest => dest.Name, src => src.Name)
                                .Map(dest => dest.Code, src => src.Code)
                                ;
        }
    }
}
