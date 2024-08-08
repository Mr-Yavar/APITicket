using Entities.DataTransferObjects;
using Entities.DataTransferObjects.DynParameters;
using Entities.DataTransferObjects.PayingDto;
using Entities.Models;
using Features.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IService
{
    public interface IDynParameterService : IService
    {

        Task<List<DynParametersDto>> GetParents();
        Task<DynParametersDto> GetChildrenByParentId(Guid Id);
        Task<DynParametersDto> GetChildrenByParentKey(string Key);

        Task<DynParametersDto> ManipulateChildren(DynParametersDto dto,FilesDto file);
        Task DeleteChild(Guid id);




    }
}
