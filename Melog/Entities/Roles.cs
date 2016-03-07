namespace Melog.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Roles
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [StringLength(64)]
        public string RoleName { get; set; }
    }
}
