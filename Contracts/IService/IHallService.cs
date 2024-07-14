using Entities.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IService
{
    public interface IHallService : IService
    {
        Task<List<HallDto>> GetHallsByPlaceId(Guid placeId);
        Task<List<HallDto>> GetAllHallsByPlaceId(Guid placeId);
        Task<GlbCreationHallDto> GetHallById(Guid Id);
        Task<bool> ProcessCreationHall(GlbCreationHallDto dto);
        Task<bool> ProcessUpdateHall(GlbCreationHallDto dto);
        Task<bool> ToggleHall(Guid id);
    }
}
