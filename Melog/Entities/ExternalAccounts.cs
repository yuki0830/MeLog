namespace Melog.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ExternalAccounts
    {
        [Key]
        public long ExternalAccountId { get; set; }

        [StringLength(64)]
        public string TwitterId { get; set; }

        [StringLength(64)]
        public string FacebookId { get; set; }

        [StringLength(64)]
        public string GoogleId { get; set; }
    }
}
