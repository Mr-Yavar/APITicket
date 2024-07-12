using Entities.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IService
{
    public interface IPromoterService : IService
    {
        /// <summary>
        /// سطح ادمنی - بدون در نظر گرفتن قعالی ا غیرفعال بودن برزگرکنندگان
        /// </summary>
        /// <returns></returns>
        Task<List<PromoterDto>> GetAllPromoters();
        
        /// <summary>
        /// سطح کاربر - گرفتن برگزارکنندگان فعال
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<PromoterDto>> GetPromoters();
        Task<PromoterDto> GetPromoter(Guid id);
        Task<bool> ProcessCreationPromoter(PromoterDto dto);
        Task<bool> ProcessUpdatePromoter(PromoterDto dto);
        Task<bool> TogglePromoter(Guid id);


    }
}
