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
    public class FloorSeatDapperRepository : IFloorSeatDapperRepository
    {
        private readonly DapperContext _context;
        public FloorSeatDapperRepository(DapperContext context) => _context = context;

    }
}
