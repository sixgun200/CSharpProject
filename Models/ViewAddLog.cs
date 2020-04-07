using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DiveLog.Models
{
    public class ViewAddLog
    {
        public Diver ThisDiver {get; set;}
        public DiverLog NewDiverLog {get; set;}
        public List<DiveSite> DiveLocations {get; set;}
    }
}
