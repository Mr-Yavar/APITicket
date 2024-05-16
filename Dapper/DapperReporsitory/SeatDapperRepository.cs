using Contracts.IDapperRepository;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts.IDapperRepository;
using Dapper.DapperReporsitory;

namespace Dapper.DapperReporsitory
{
    public class SeatDapperRepository :  ISeatDapperRepository
    {
        private readonly DapperContext _context;
        public SeatDapperRepository(DapperContext context) => _context = context;
    }
}
