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
    
    public partial class status
    {
        public status()
        {
            this.parents = new HashSet<parent>();
            this.sessions = new HashSet<session>();
            this.tutors = new HashSet<tutor>();
            this.sessions1 = new HashSet<session>();
            this.sessions2 = new HashSet<session>();
        }
    
        public int status_id { get; set; }
        public string name { get; set; }
    
        public virtual ICollection<parent> parents { get; set; }
        public virtual ICollection<session> sessions { get; set; }
        public virtual ICollection<tutor> tutors { get; set; }
        public virtual ICollection<session> sessions1 { get; set; }
        public virtual ICollection<session> sessions2 { get; set; }
    }
}
