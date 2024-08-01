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
        glbConcertsDto GetAllPublicConcerts();
        glbConcertsDto GetSpecificConcert(Guid ConcertId);
        glbEventDto GetSpecificEvent(Guid EventId);

        /// <summary>
        /// برای قسمت مدیریت
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        glbConcertsDto GetAllConcerts(int pageSize, int currentPage);

        Task<glbCreationConcertDto> UpdateConcert(glbCreationConcertDto dto);
         Task<glbCreationConcertDto> GetConcert(Guid ConcertId);
         Task<glbCreationConcertDto> CreateConcert(glbCreationConcertDto dto);


        Task<glbCreationConcertDto> GetConcertByConcertId(Guid ConcertId);
        Task UpdateConcertInfo(glbCreationConcertDto dto);
    }
}
