//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BloodBankSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BloodBag
    {
        public int bagID { get; set; }
        public string bloodGroup { get; set; }
        public int branchId { get; set; }
    
        public virtual BloodGroup BloodGroup1 { get; set; }
    }
}
