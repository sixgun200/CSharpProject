using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace DiveLog.Models
{
    public class Diver
    {
        [Key]
        public int DiverID {get; set;}

        [Required]
        [MinLength(3)]
        [Display(Name="User Name")]
        public string Username {get; set;}

        [Required]
        [MinLength(2)]
        [Display(Name="First Name")]
        public string FName {get; set;}

        [Required]
        [MinLength(2)]
        [Display(Name="Last Name")]
        public string LName {get; set;}

        [Required]
        [DataType(DataType.Date)]
        [Display(Name="Birth Date")]
        public DateTime BirthDate {get; set;}

        [EmailAddress]
        [Display(Name="e-mail address")]
        public string Email {get; set;}

        [MinLength(8, ErrorMessage="Certificate Number must be 8-10 characters")]
        [MaxLength(10)]
        [Display(Name="Diver Certification Number")]
        public string DiveCertID {get; set;}

        public char CertificateAuthority {get; set;}    // Certificate Authorities are PADI = P, NAUI = N

        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters")]
        public string Password {get; set;}

        [NotMapped]
        [Compare("Password", ErrorMessage="Passwords do not match")]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        public string ConfirmPassword {get; set;}

        public List<DiverLog> DiverLogs {get; set;}
    }
}