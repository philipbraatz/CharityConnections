//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CC.Connections.PL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Charity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Charity()
        {
            this.CharityEvents = new HashSet<CharityEvent>();
        }
    
        public int Charity_ID { get; set; }
        public Nullable<int> Charity_ContactID { get; set; }
        public string Charity_EIN { get; set; }
        public Nullable<bool> Charity_Deductibility { get; set; }
        public string Charity_URL { get; set; }
        public string Charity_Cause { get; set; }
        public string Charity_Email { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CharityEvent> CharityEvents { get; set; }
    }
}