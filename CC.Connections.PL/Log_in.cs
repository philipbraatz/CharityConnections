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
    
    public partial class Log_in
    {
        public int Log_in_ID { get; set; }
        public Nullable<int> MemeberID { get; set; }
        public string Password { get; set; }
    
        public virtual Member Member { get; set; }
    }
}
