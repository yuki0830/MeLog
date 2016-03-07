namespace Melog.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Authorities
    {
        [Key]
        public int AuthorityId { get; set; }

        [Required]
        [StringLength(62)]
        public string AuthorityName { get; set; }
    }
}
