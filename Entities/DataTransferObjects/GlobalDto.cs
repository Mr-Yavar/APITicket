using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{


    public class glbConcertsDto{
    
        public List<ConcertInfoDto> Concerts {  get; set; }   


    }

    public class ConcertInfoDto
    {
        public Guid Concertid { get; set; }
        public string Category { get; set; }
        public string img_Url { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }
        public PlaceInfoDto Place { get; set; }

        public DateTime EndDate { get; set; }

        
    }


    public class PlaceInfoDto
    {

        public string Name { get; set; }
        public string Description { get; set; }

      
    }

}
