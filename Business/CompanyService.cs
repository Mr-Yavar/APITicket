using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.Models;

namespace Services
{
    public class CompanyService : ServiceBase, ICompanyService
    {
      
        public CompanyService(IRepositoryManager repositoryManager, IMapper mapper) : base(repositoryManager, mapper)
        {

        }

        public List<CompanyDto> GetAllCompanies()
        {
            var companies = _repositoryManager.CompanyRepository.FindAll(trackChanges: false).OrderBy(c=>c.Name).ToList();
            var result = _mapper.Map<List<CompanyDto>>(companies);
            return result;
        }

        public CompanyDto GetCompany(Guid CompanyID)
        {
            var companyModel = _repositoryManager.CompanyRepository.FindByCondition(x=>x.Id==CompanyID,false).FirstOrDefault();
            var result = _mapper.Map<CompanyDto>(companyModel);
            return result;
        }

        public CompanyDto CreateCompany(CompanyForCreationDto Company)
        {

            var CompanyModel = _mapper.Map<CompanyModel>(Company);
            _repositoryManager.CompanyRepository.Create(CompanyModel);
            _repositoryManager.Save();
            return _mapper.Map<CompanyDto>(CompanyModel);
        }

    }
}