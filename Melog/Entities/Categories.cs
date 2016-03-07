namespace Melog.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Categories
    {
        [Key]
        public long CategoryId { get; set; }

        [Required]
        [StringLength(64)]
        public string CategoryName { get; set; }
    }
}
