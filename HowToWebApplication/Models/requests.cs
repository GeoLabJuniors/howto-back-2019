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
 

    public partial class requests
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public requests()
        {
            this.requestsArticles = new HashSet<requestsArticles>();
        }
    
        public int Id { get; set; }
        public string title { get; set; }
        public int number { get; set; }
        public string content { get; set; }
        public int upvote { get; set; }
        public bool isDone { get; set; }
        public int usersId { get; set; }
        public System.DateTime date { get; set; }
    
        public virtual users users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<requestsArticles> requestsArticles { get; set; }
    }
}
