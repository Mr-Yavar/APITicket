
using Contracts.IDapperRepository;
using Dapper.DapperReporsitory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.DapperReporsitory
{
    public class CategoryDapperRepository :ICategoryDapperRepository
    {
        private readonly DapperContext _context;
        public CategoryDapperRepository(DapperContext context) => _context = context;
    }
}
