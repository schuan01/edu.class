using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.ViewModels
{
	public class QuestionViewModel
	{
		public int Id { get; set; }

		[Required]
		public string Content { get; set; }
		
		public System.DateTime CreatedAt { get; set; }
		
		public Nullable<System.DateTime> UpdatedAt { get; set; }
		
		[Required]
		public int TestId { get; set; }

		[Required]
		public int QuestionType { get; set; }
		
		public bool Enabled { get; set; }
	}
}