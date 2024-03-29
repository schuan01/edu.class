using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.ViewModels
{
    public class TestViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Fecha Inicio")]
        public string StartDate { get; set; }

        [Required]
        [Display(Name = "Fecha Finalización")]
        public string EndDate { get; set; }

        [Required]
        public int GroupId { get; set; }

        public string UpdatedAt { get; set; }

        public bool Enabled { get; set; }
    }
}