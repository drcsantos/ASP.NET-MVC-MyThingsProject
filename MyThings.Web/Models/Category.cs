using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyThings.Web.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Icone")]
        public string Icon { get; set; }

        [StringLength(255)]
        public string UserName { get; set; }

        public ICollection<Thing> Things { get; set; }
    }
}