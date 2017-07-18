namespace AdventureWorksConnection
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sales.SalesTerritory")]
    public partial class SalesTerritory
    {
        [Key]
        public int TerritoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(3)]
        public string CountryRegionCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Group { get; set; }

        [Column(TypeName = "money")]
        public decimal SalesYTD { get; set; }

        [Column(TypeName = "money")]
        public decimal SalesLastYear { get; set; }

        [Column(TypeName = "money")]
        public decimal CostYTD { get; set; }

        [Column(TypeName = "money")]
        public decimal CostLastYear { get; set; }

        public Guid rowguid { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
