//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TriaCulturaDesktopApp.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class place
    {
        public place()
        {
            this.events = new HashSet<@event>();
            this.place_has_capacity = new HashSet<place_has_capacity>();
            this.requests = new HashSet<request>();
        }
    
        public int id_place { get; set; }
        public string name { get; set; }
        public int capacity { get; set; }
        public string address { get; set; }

        public int count { get { return requests.Count; } }
    
        public virtual ICollection<@event> events { get; set; }
        public virtual ICollection<place_has_capacity> place_has_capacity { get; set; }
        public virtual ICollection<request> requests { get; set; }
    }
}
