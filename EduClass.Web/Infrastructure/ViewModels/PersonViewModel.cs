using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.ViewModels
{
    public class PersonViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(75)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [MaxLength(75)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(75)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Birthday { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string IdentificationCard { get; set; }

        [Required]
        public string PersonType { get; set; }

        public System.DateTime CreatedAt { get; set; }

        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public bool Enabled { get; set; }

        
    }
}