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
    
    public partial class @event
    {
        public int id_event { get; set; }
        public System.DateTime date_in { get; set; }
        public System.DateTime date_out { get; set; }
        public int place_id { get; set; }
    
        public virtual place place { get; set; }
    }
}
