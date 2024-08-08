using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Enum
{
   public enum FileTypeEnum : byte
    {

        None = 0,
        SeatMap = 1,
        Place = 2,
        Hall = 3,
        Event = 4,
        Slider = 5,
        Promoter = 6,
        DynParam = 7,
    }


    public static class
    FileType
    {

        public static readonly Dictionary<FileTypeEnum,string> FileTypes= new Dictionary<FileTypeEnum, string>() { 
        
            [FileTypeEnum.None] = "",
            [FileTypeEnum.SeatMap] = "Maps",
            [FileTypeEnum.Place] = "Places",
            [FileTypeEnum.Hall] = "Halls",
            [FileTypeEnum.Event] = "Events",
            [FileTypeEnum.Slider] = "Sliders",
            [FileTypeEnum.Promoter] = "Promoters",
            [FileTypeEnum.DynParam] = "DynParams",

        };

    }


}
