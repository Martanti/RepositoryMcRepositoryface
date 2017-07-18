namespace AdventureWorksConnection
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Purchasing.Vendor")]
    public partial class Vendor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BusinessEntityID { get; set; }

        [Required]
        [StringLength(15)]
        public string AccountNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public byte CreditRating { get; set; }

        public bool PreferredVendorStatus { get; set; }

        public bool ActiveFlag { get; set; }

        [StringLength(1024)]
        public string PurchasingWebServiceURL { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
