using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EduClass.WebApi.Infrastructure.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Start { get; set; }

        [Required]
        public string End { get; set; }

        [Required]
        public string EventType { get; set; }

        [Required]
        public int CalendarId { get; set; }

        public bool Enabled { get; set; }
    }
}