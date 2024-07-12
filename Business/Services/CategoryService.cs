using AutoMapper;
using Contracts.IDataShaper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.Models;
using Features.RequestFeatures;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class CategoryService : ServiceBase,ICategoryService
    {
        
        ILoggerManager _logger;
        private readonly IRepositoryManager _repositoryManager;

        public CategoryService(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager logger) : base(repositoryManager, mapper)
        {
            _logger = logger;
            _repositoryManager= repositoryManager;
        }

        public async Task<List<CategoryDto>> GetCategories()
        {
            var result = _repositoryManager.ICategoryRepository.FindAll(false).ToList();
            if (result != null)
            {
                var res= _mapper.Map<List<CategoryDto>>(result);
                return res;
            }
            return default;

        }




    }
}
