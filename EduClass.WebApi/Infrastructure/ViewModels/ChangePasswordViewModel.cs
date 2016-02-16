using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EduClass.WebApi.Infrastructure.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string PersonId { get; set; }

        [Required(ErrorMessage = "Contraseña anterior es requerida")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Contraseña nueva es requerida")]
        [Display(Name = "Contraseña nueva")]
        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "La contraseña debe estar entre 6 y 255 caracteres", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirmar contraseña nueva es requerida")]
        [Display(Name = "Confirmar contraseña")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas deben cohincidir")]
        public string ConfirmNewPassword { get; set; }
    }
}