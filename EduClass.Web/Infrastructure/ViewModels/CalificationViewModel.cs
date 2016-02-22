using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EduClass.Web.Infrastructure.ViewModels
{
    public class CalificationViewModel
    {
        public int Id { get; set; }

        [Required]
        public int Oral { get; set; }

        [Required]
        public int Test { get; set; }

        [Required]
        public double Average { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public System.DateTime CreatedAt { get; set; }

        public Nullable<System.DateTime> UpdatedAt { get; set; }

    }
}