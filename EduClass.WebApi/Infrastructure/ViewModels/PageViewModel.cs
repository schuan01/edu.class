using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EduClass.WebApi.Infrastructure.ViewModels
{
    public class PageViewModel
    {
        public int Id { get; set; }

        [AllowHtml]
        [Required]
        public string Content { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public System.DateTime CreatedAt { get; set; }

        public Nullable<System.DateTime> UpdatedAt { get; set; }

        public bool Enabled { get; set; }
    }
}