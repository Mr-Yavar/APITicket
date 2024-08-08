using Entities.DataTransferObjects;
using Entities.Models;
using Features.CustomRequest;
using Features.RequestFeatures;
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
        Task<PagedData<List<ConcertInfoDto>>> GetAllConcerts(ConcertTableRequest dto);

        Task<glbCreationConcertDto> UpdateConcert(glbCreationConcertDto dto);
         Task<glbCreationConcertDto> GetConcert(Guid ConcertId);
         Task<glbCreationConcertDto> CreateConcert(glbCreationConcertDto dto);


        Task<glbCreationConcertDto> GetConcertByConcertId(Guid ConcertId);
        Task UpdateConcertInfo(glbCreationConcertDto dto);
    }
}
