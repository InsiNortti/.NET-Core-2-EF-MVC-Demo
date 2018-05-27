using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Customer
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Nimi pakollinen")]
        [Display(Name = "Nimi")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Nimi 3-50 merkkiä")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Osoite pakollinen")]
        [Display(Name = "Osoite")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Osoite 5-50 merkkiä")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Laskutusosoite pakollinen")]
        [Display(Name = "Laskutusosoite")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Laskutusosoite 5-50 merkkiä")]
        public string BillingAddress { get; set; }

        [Required(ErrorMessage = "Puhelinnumero pakollinen")]
        [Display(Name = "Puhelin")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Puhelin 5-15 merkkiä")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Sähköposti pakollinen")]
        [Display(Name = "Sähköposti")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Sähköposti 5-50 merkkiä")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Salasana pakollinen")]
        [Display(Name = "Salasana")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Salasana 5-20 merkkiä")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Salasanan vahvistus pakollinen")]
        [NotMapped]
        [Display(Name = "Salasana vahvistus")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Salasanat eivät täsmää")]
        public string ConfirmPassword { get; set; }

        // Navigation properties
        [Display(Name = "Asunto")]
        public Apartment Apartment { get; set; }
        [Display(Name = "Tilaukset")]
        public ICollection<Order> Orders { get; set; }
    }
}
