using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IService
{
    public interface IConcertService : IService
    {
        glbConcertsDto GetAllConcerts();


        glbConcertsDto GetSpecificConcert(Guid ConcertId);
    }
}
