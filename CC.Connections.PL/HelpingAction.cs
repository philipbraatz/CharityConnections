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
    
    public partial class HelpingAction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HelpingAction()
        {
            this.MemberActions = new HashSet<MemberAction>();
        }
    
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> CategoryID { get; set; }
        public string Description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberAction> MemberActions { get; set; }
    }
}