//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HowToWebApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class images
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Nullable<int> articlesId { get; set; }
        public Nullable<int> usersId { get; set; }
        public Nullable<bool> isMain { get; set; }
    
        public virtual articles articles { get; set; }
        public virtual users users { get; set; }
    }
}
