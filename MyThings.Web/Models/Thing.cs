using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyThings.Web.Models
{
    public class Thing
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [StringLength(255)]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Url]
        [StringLength(255)]
        [Display(Name = "Hiperlink Foto")]
        public string ImageLink { get; set; }  
        
        [StringLength(255)]
        public string UserName { get; set; }

        [Required]
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        public int? PersonID { get; set; }
        public virtual Person Person { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Empréstimo")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LentDate { get; set; }        
    }
}