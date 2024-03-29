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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Mail()
        {
            this.PersonsTo = new HashSet<Person>();
        }
    
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public System.DateTime CreateAt { get; set; }
        public Nullable<System.DateTime> ReadAt { get; set; }
        public int PersonFromId { get; set; }
        public bool Enabled { get; set; }
    
        public virtual Person PersonFrom { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Person> PersonsTo { get; set; }
    }
}
