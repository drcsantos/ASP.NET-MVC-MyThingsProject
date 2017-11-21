using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyThings.Web.Models
{
    public class Person
    {
        public int PersonID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name="Nome")]
        public string Name { get; set; }

        [StringLength(50)]
        [Display(Name = "Apelido")]
        public string NickName { get; set; }

        [Url]
        [StringLength(255)]
        [Display(Name = "Hiperlink Foto")]
        public string ImageLink { get; set; }
        
        [StringLength(255)]
        public string UserName { get; set; }

        public string FullName
        {
            get
            {
                return Name + (string.IsNullOrEmpty(NickName) ? string.Empty :  " (" + NickName + ")");
            }
        }

        public ICollection<Thing> Things { get; set; }
    }
}