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
    
    public partial class schedule
    {
        public schedule()
        {
            this.sessions = new HashSet<session>();
        }
    
        public int schedule_id { get; set; }
        public string day_otw { get; set; }
        public Nullable<System.TimeSpan> start_time { get; set; }
        public Nullable<System.TimeSpan> end_time { get; set; }
        public string note { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<int> tutor_id { get; set; }
        public Nullable<System.DateTime> dateCreate { get; set; }
    
        public virtual status status1 { get; set; }
        public virtual tutor tutor { get; set; }
        public virtual ICollection<session> sessions { get; set; }
    }
}
