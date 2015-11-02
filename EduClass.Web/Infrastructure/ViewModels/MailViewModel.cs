using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace EduClass.Web.Infrastructure.ViewModels
{
    public class MailViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(320)]
        public string Subject { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public System.DateTime CreatedAt { get; set; }

        public System.DateTime ReadAt { get; set; }

        public bool Enabled { get; set; }

        [Required]
        public string PersonEmailFrom { get; set; }

        [Required]
        public int[] PersonIdTo { get; set; }
    }
}