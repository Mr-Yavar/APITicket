using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Entities.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ConfigureCompany(); // Model <-> Dto
            ConfigureEmplyee(); // Model <-> Dto
        }

        public void ConfigureCompany()
        {
            CreateMap<CompanyModel, CompanyDto>()
                .ForMember(c => c.FullAddress,opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
        }

        public void ConfigureEmplyee()
        {
            CreateMap<EmployeeModel, EmployeeDto>().ReverseMap();
        }
    }

}
