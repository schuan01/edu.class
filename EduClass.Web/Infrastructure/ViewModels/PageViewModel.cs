using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EduClass.Web.Infrastructure.ViewModels
{
    public class PageViewModel
    {
        public int Id { get; set; }

        [AllowHtml]
        [Required]
        [Display(Name = "Contenido")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Título")]
        public string Name { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public System.DateTime CreatedAt { get; set; }

        public Nullable<System.DateTime> UpdatedAt { get; set; }

        public bool Enabled { get; set; }
    }
}