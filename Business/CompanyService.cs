using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        public List<CompanyDto> GetCompanyCollectionByIds(IEnumerable<Guid> ids)
        {
            var Companies =    _repositoryManager.CompanyRepository.FindByCondition(x=>ids.Contains(x.Id),false).ToList();
            return _mapper.Map<List<CompanyDto>>(Companies);
        }

        public IEnumerable<CompanyDto> CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var companyEntities = _mapper.Map<IEnumerable<CompanyModel>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _repositoryManager.CompanyRepository.Create(company);
            }
            _repositoryManager.Save();

            var companyCollectionToReturn =_mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            return companyCollectionToReturn;
            
        }
    }
}