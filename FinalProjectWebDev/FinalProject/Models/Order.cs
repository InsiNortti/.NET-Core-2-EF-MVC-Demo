using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public enum State { TILATTU, ALOITETTU, VALMIS, HYVÄKSYTTY, HYLÄTTY }

    public class Order
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Työn tila")]
        public State State { get; set; }

        [Required(ErrorMessage = "Anna työtehtävän kuvaus")]
        [Display(Name = "Kuvaus")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Työn kuvaus 10-200 merkkiä")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Tilauspäivämäärä")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Aloituspäivämäärä")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Valmistumispäivämäärä")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FinishDate { get; set; }

        [Display(Name = "Hyväksymispäivämäärä")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? AcceptDate { get; set; }

        [Display(Name = "Hylkäämispäivämäärä")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CancelDate { get; set; }

        [Display(Name = "Kommentti")]
        [StringLength(200, ErrorMessage = "Kommentti korkeintaan 200 merkkiä")]
        public string Comment { get; set; }

        [Display(Name = "Työtunnit")]
        public double? HourCount { get; set; }

        [Display(Name = "Hinta")]
        [DataType(DataType.Currency)]
        public double? Cost { get; set; }

        [Display(Name = "Kuluneet tarvikkeet")]
        [StringLength(200, ErrorMessage = "Selostus korkeintaan 200 merkkiä")]
        public string UsedAccessories { get; set; }

        // Foreign keys
        public int CustomerID { get; set; }
        // Navigation properties
        [Display(Name = "Asiakas")]
        public Customer Customer { get; set; }
    }
}
