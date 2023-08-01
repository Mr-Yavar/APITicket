using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;

namespace Services
{
    public class CompanyService : ServiceBase, ICompanyService
    {
      
        public CompanyService(IRepositoryManager repositoryManager, IMapper mapper) : base(repositoryManager, mapper)
        {

        }

        public List<CompanyDto> GetAllCompanies()
        {
            var companies = _repositoryManager.CompanyRepository.GetAllCompanies(trackChanges: false);
            var result = _mapper.Map<List<CompanyDto>>(companies);
            return result;
        }

        public CompanyDto GetCompany(Guid CompanyID)
        {
            var companyModel = _repositoryManager.CompanyRepository.FindByCondition(x=>x.Id==CompanyID,false).FirstOrDefault();
            var result = _mapper.Map<CompanyDto>(companyModel);
            return result;
        }

    }
}