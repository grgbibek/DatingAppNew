using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    [Table("Photos")] // for renaming the table as it will be automatically created as it is specified in the AppUser class
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }

        // added for relationship with AppUser
        public int AppUserId { get; set; }
        public AppUser AppUser {get;set;}
    }
}