using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DiveLog.Models
{
    public class DiverLog
    {
        [Key]
        public int DiverLogID {get; set;}

        [Required]
        public int DiverID {get; set;}

        public Diver Diver {get; set;}
        [Required]
        public int DiveSiteID {get; set;}
        public DiveSite DiveSite {get; set;}

        [Required]
        [Display(Name="Dive Number")]
        public int DiveNumber {get; set;}

        [Required]
        [Display(Name="Date of Dive")]
        public DateTime DiveDate {get; set;}

        [Required]
        [Display(Name="Bottom Time")]
        [Range(1,205)]
        public int BottomTime {get; set;}

        [Required]
        [Range(1,140)]
        [Display(Name="Maximum Depth")]
        public int MaxDepth {get; set;}

        [Required]
        [Display(Name="Tank Starting PSI")]
        public int TankStartPSI {get; set;}

        [Required]
        [Display(Name="Tank Ending PSI")]
        public int TankEndPSI {get; set;}

        [Required]
        [Display(Name="Dive Start Time")]
        public DateTime DiveStartTime {get; set;}

        [Required]
        [Display(Name="Dive Stop Time")]
        public DateTime DiveStopTime {get; set;}

        [Display(Name="Safety Stop Depth")]
        public int SafetyStopDepth {get; set;}

        [Display(Name="Safety Stop Time")]
        public int SafetyStopTime {get; set;}

        [Required]
        public char PressureGroupEntry {get; set;}

        [Required]
        public char PressureGroupExit {get; set;}

        [Required]
        [Display(Name="Surface Interval")]
        public int SurfaceInterval {get; set;} //time spent at the surface between dives

        [Required]
        [Display(Name="Water Temperature")]
        public int WaterTemp {get; set;}    //water temp below 60 degrees Farenheit increases depth calculations by 10 feet
    }
}