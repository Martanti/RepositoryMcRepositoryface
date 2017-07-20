namespace ProjectyMcProjectface
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RegisteredUser
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        [Required]
        [StringLength(64)]
        public string PassWord { get; set; }

        public virtual ICollection<ConnectionString> ConnectionStrings { get; set; }
    }
}
