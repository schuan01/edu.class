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
    
    public partial class Mail
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public System.DateTime CreateAt { get; set; }
        public Nullable<System.DateTime> ReadAt { get; set; }
        public int PersonId { get; set; }
        public bool Enabled { get; set; }
    
        public virtual Person Person { get; set; }
    }
}
