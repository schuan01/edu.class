using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EduClass.WebApi.Infrastructure.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(2500)]
        public string Content { get; set; }

        
        public int GroupId { get; set; }

        public int PersonId { get; set; }

        [Required]
        public int PostType { get; set; }


        public System.DateTime CreatedAt { get; set; }

        public Nullable<System.DateTime> UpdatedAt { get; set; }
        
        public bool Enabled { get; set; }
    }
}