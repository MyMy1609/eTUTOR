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
    
    public partial class subject
    {
        public subject()
        {
            this.sessions = new HashSet<session>();
        }
    
        public int subject_id { get; set; }
        public int tutor_id { get; set; }
        public string subject_name { get; set; }
    
        public virtual ICollection<session> sessions { get; set; }
        public virtual tutor tutor { get; set; }
    }
}
