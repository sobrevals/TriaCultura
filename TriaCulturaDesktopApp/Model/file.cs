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
    
    public partial class file
    {
        public int id_file { get; set; }
        public string name { get; set; }
        public string extension { get; set; }
        public string path { get; set; }
        public int project_id { get; set; }
    
        public virtual project project { get; set; }
    }
}
