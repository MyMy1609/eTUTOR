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
    
    public partial class student
    {
        public student()
        {
            this.comments = new HashSet<comment>();
            this.history_lessons = new HashSet<history_lessons>();
            this.sessions = new HashSet<session>();
        }
    
        public int student_id { get; set; }
        public string fullname { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<int> parent_id { get; set; }
    
        public virtual ICollection<comment> comments { get; set; }
        public virtual ICollection<history_lessons> history_lessons { get; set; }
        public virtual parent parent { get; set; }
        public virtual ICollection<session> sessions { get; set; }
    }
}
