using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.Models;
using LoggerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ConcertService : ServiceBase,IConcertService
    {
        public ConcertService(IRepositoryManager repositoryManager,IMapper mapper) : base(repositoryManager, mapper)
        {

        }



        public List<Concert> GetAllConcerts()
        {
            
            return _repositoryManager._IConcertRepository.FindAll(false).Where(x=>x.StartDate>= DateTime.Now || x.EndDate>=DateTime.Now).ToList();

        }
    }
}
