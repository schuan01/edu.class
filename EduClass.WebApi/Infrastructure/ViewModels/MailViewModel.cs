using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EduClass.WebApi.Infrastructure.ViewModels
{
    public class MailViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(320)]
        [Display(Name = "Asunto")]
        public string Subject { get; set; }

        
        [Required]
        [AllowHtml]
        [Display(Name = "Descripcion")]
        public string Description { get; set; }

        [Display(Name = "Remitente")]
        public string PersonEmailFrom { get; set; }

        [Required]
        public System.DateTime CreatedAt { get; set; }

        public System.DateTime ReadAt { get; set; }

        public bool Enabled { get; set; }


        [Required]
        [Display(Name = "Destinatarios")]
        public int[] PersonIdTo { get; set; }
    }
}