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
        }
    }
}
