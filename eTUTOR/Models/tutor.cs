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
    
    public partial class tutor
    {
        public tutor()
        {
            this.comments = new HashSet<comment>();
            this.history_lessons = new HashSet<history_lessons>();
            this.schedules = new HashSet<schedule>();
            this.sessions = new HashSet<session>();
            this.contact_tutor = new HashSet<contact_tutor>();
            this.subjects = new HashSet<subject>();
        }
    
        public int tutor_id { get; set; }
        public string fullname { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public Nullable<System.DateTime> birthday { get; set; }
        public string specialized { get; set; }
        public string job { get; set; }
        public string experience { get; set; }
        public string certificate { get; set; }
        public Nullable<int> status { get; set; }
        public string avatar { get; set; }
    
        public virtual ICollection<comment> comments { get; set; }
        public virtual ICollection<history_lessons> history_lessons { get; set; }
        public virtual ICollection<schedule> schedules { get; set; }
        public virtual ICollection<session> sessions { get; set; }
        public virtual status status1 { get; set; }
        public virtual ICollection<contact_tutor> contact_tutor { get; set; }
        public virtual ICollection<subject> subjects { get; set; }
    }
}
