using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EduClass.WebApi.Infrastructure.ViewModels
{
    public class GroupViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [MaxLength(75)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        //TODO Requiered
        [MinLength(8)]
        [Display(Name = "Clave")]
        public string Key { get; set; }
    }
}