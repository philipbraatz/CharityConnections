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
    
    public partial class MemberAction
    {
        public System.Guid ID { get; set; }
        public string MemberEmail { get; set; }
        public Nullable<System.Guid> ActionID { get; set; }
    
        public virtual HelpingAction HelpingAction { get; set; }
    }
}