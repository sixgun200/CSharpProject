using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace DiveLog.Models
{
    public class DiveSite
    {
        [Key]
        public int DiveSiteID {get; set;}

        [Required]
        [MinLength(3)]
        [Display(Name="Dive Site Name")]
        public string SiteName {get; set;}

        [Required]
        public string City {get; set;}

        [Display(Name="State or Province")]
        public string StProv {get; set;}

        [Required]
        public string Country {get; set;}

        [Required]
        public double Latitude {get; set;}

        [Required]
        public double Longitude {get; set;}

        public int Altitude {get; set;}

        public List<DiverLog> WaterLovers {get; set;}
    }
}