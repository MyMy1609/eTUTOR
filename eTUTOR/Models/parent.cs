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
    
    public partial class parent
    {
        public parent()
        {
            this.students = new HashSet<student>();
        }
    
        public int parent_id { get; set; }
        public string fullname { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public Nullable<System.DateTime> birthday { get; set; }
        public Nullable<int> status { get; set; }
    
        public virtual status status1 { get; set; }
        public virtual ICollection<student> students { get; set; }
    }
}
