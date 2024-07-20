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
        /// <summary>
        /// فقط فعال ها
        /// </summary>
        /// <param name="placeId"></param>
        /// <returns></returns>
        Task<List<HallDto>> GetHallsByPlaceId(Guid placeId);
        /// <summary>
        /// همه حتی غیرفعال ها
        /// </summary>
        /// <param name="placeId"></param>
        /// <returns></returns>
        Task<List<HallDto>> GetAllHallsByPlaceId(Guid placeId);
        /// <summary>
        /// همه فعال ها با تمام بلیط های ثبت شده
        /// </summary>
        /// <param name="placeId"></param>
        /// <returns></returns>
        Task<List<GlbCreationHallDto>> GetHallsWithSeatsByPlaceId(Guid placeId);
        Task<GlbCreationHallDto> GetHallById(Guid Id);
        Task<bool> ProcessCreationHall(GlbCreationHallDto dto);
        Task<bool> ProcessUpdateHall(GlbCreationHallDto dto);
        Task<bool> ToggleHall(Guid id);
    }
}
