
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
    
public partial class Helping_Action
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Helping_Action()
    {

        this.Member_Action = new HashSet<Member_Action>();

    }


    public int Helping_Action_ID { get; set; }

    public Nullable<int> HelpingActionCategory_ID { get; set; }

    public string HelpingActionDescription { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Member_Action> Member_Action { get; set; }

}

}
