namespace AdventureWorksConnection
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sales.CurrencyRate")]
    public partial class CurrencyRate
    {
        public int CurrencyRateID { get; set; }

        public DateTime CurrencyRateDate { get; set; }

        [Required]
        [StringLength(3)]
        public string FromCurrencyCode { get; set; }

        [Required]
        [StringLength(3)]
        public string ToCurrencyCode { get; set; }

        [Column(TypeName = "money")]
        public decimal AverageRate { get; set; }

        [Column(TypeName = "money")]
        public decimal EndOfDayRate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
