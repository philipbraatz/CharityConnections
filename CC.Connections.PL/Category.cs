
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
    
public partial class Category
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Category()
    {

        this.Helping_Action = new HashSet<Helping_Action>();

        this.Member_Categories = new HashSet<Member_Categories>();

    }


    public int Categories_ID { get; set; }

    public string Categories_Desc { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Helping_Action> Helping_Action { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Member_Categories> Member_Categories { get; set; }

}

}
