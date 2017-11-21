using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyThings.Web.ViewModels
{
    public class LendViewModel
    {
        public Guid ThingID { get; set; }
        public string ThingName { get; set; }
        public string ThingImageLink { get; set; }
        public string ThingCategoryName { get; set; }

        [Required]
        public int? PersonID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Empréstimo")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LentDate { get; set; }
    }
}