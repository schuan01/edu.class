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
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public System.DateTime Date { get; set; }

        [Required]
        public int EventTypeId { get; set; }
        
        [Required]
        public System.DateTime CreatedAt { get; set; }

        public Nullable<System.DateTime> UpdatedAt { get; set; }

        public bool Enabled { get; set; }
    }
}