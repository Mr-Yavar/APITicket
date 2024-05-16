using Contracts.IDapperRepository;
using Contracts.IDapperRepository;
using Dapper;
using Dapper.DapperReporsitory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.DapperReporsitory
{
    public class HallDapperRepository :IHallDapperRepository
    {
        private readonly DapperContext _context;
        public HallDapperRepository(DapperContext context) => _context = context;
    }
}
