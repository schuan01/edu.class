//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EduClass.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Response
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public int StudentId { get; set; }
        public Nullable<bool> TrueOrFalse { get; set; }
        public int QuestionId { get; set; }
        public Nullable<bool> IsCorrect { get; set; }
        public int QuestionOptionId { get; set; }
    
        public virtual Student Student { get; set; }
        public virtual Question Question { get; set; }
        public virtual QuestionOption QuestionOption { get; set; }
    }
}
