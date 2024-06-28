using Entities.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IService
{
    public interface IPlaceService : IService
    {
        Task<List<PlaceDto>> GetAllPlaces();
        Task<PlaceDto> GetById(Guid id);
        Task<bool> ProcessCreationPlace(PlaceDto dto);
        Task<bool> ProcessUpdatePlace(PlaceDto dto);



    }
}
