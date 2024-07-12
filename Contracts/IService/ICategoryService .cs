using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IService
{
    public interface ICategoryService : IService
    {

        Task<List<CategoryDto>> GetCategories();
    }
}
