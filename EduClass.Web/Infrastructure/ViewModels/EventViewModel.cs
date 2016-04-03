using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Comienzo")]
        public string Start { get; set; }

        [Required]
        [Display(Name = "Final")]
        public string End { get; set; }

        [Required]
        public string EventType { get; set; }

        [Required]
        public int CalendarId { get; set; }

        public bool Enabled { get; set; }
    }
}