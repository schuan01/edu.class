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
        public string QuestionId { get; set; }
        public string TrueOrFalse { get; set; }
        public string QuestionOptionId { get; set; }
    
        public virtual Question Question { get; set; }
        public virtual QuestionOption QuestionOption { get; set; }
        public virtual Student Student { get; set; }
    }
}
