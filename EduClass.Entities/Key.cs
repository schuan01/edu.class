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
    
    public partial class Key
    {
        public int Id { get; set; }
        public string HashKey { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public int GroupId { get; set; }
        public bool Enabled { get; set; }
    
        public virtual Group Group { get; set; }
    }
}
