using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public enum ApartmentType { Omakotitalo, Kerrostalo, Rivitalo }

    public class Apartment
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Valitse asuntotyyppi")]
        [Display(Name = "Asuntotyyppi")]
        public ApartmentType ApartmentType { get; set; }

        [Required(ErrorMessage = "Asunnon pinta-ala pakollinen")]
        [Display(Name = "Asunnon pinta-ala")]
        [Range(10, 500, ErrorMessage = "Asunnon pinta-ala väliltä 10 - 500 m2")]
        public double ApartmentArea { get; set; }

        [Required(ErrorMessage = "Tontin pinta-ala pakollinen")]
        [Display(Name = "Tontin pinta-ala")]
        [Range(0, 1000000, ErrorMessage = "Tontin pinta-ala väliltä 0 - 100 0000 m2")]
        public double PropertyArea { get; set; }

        // Foreign keys
        public int CustomerID { get; set; }
        // Navigation properties
        [Display(Name = "Asiakas")]
        public Customer Customer { get; set; }
    }
}
