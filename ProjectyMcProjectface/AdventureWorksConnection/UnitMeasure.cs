namespace AdventureWorksConnection
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Production.UnitMeasure")]
    public partial class UnitMeasure
    {
        [Key]
        [StringLength(3)]
        public string UnitMeasureCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
