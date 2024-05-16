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
    public class EmployeeService : ServiceBase
    {
        
        ILoggerManager _logger;
        
        public EmployeeService(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager logger) : base(repositoryManager, mapper)
        {
            _logger = logger;
        }




       
    }
}
