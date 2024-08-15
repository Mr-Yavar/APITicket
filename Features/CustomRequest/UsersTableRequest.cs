
using Features.RequestFeatures;
using Features.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.CustomRequest
{
    public class UsersTableRequest : Features.RequestFeatures.RequestFeatures
    {
        public string FullName { get; set; } = string.Empty;


    }
}
