namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Medicaments
    {
        public int ID { get; set; }

        public int RecordID { get; set; }

        [Required]
        public string Medicine { get; set; }

        public virtual RecordInPatientFile RecordInPatientFile { get; set; }
    }
}
