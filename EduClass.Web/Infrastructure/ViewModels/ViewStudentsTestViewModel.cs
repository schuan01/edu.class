using EduClass.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.ViewModels
{
    public class ViewStudentsTestViewModel
    {
        public Student Student { get; set; }
        public int ReponseCorrect { get; set; }
    }
}