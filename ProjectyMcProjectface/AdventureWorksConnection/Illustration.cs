namespace AdventureWorksConnection
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Production.Illustration")]
    public partial class Illustration
    {
        public int IllustrationID { get; set; }

        [Column(TypeName = "xml")]
        public string Diagram { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
