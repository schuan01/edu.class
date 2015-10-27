using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.ViewModels
{
    public class GroupViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(75)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int KeyId { get; set; }

        [Required]
        public System.DateTime CreatedAt { get; set; }

        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public bool Enabled { get; set; }
    }
}