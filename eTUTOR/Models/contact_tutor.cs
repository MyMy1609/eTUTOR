//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eTUTOR.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class contact_tutor
    {
        public int id { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public Nullable<int> tutor_id { get; set; }
    
        public virtual tutor tutor { get; set; }
    }
}
