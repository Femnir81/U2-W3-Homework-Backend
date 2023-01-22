namespace U2_W3_Homework_Backend.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ordini")]
    public partial class Ordini
    {
        public int ID { get; set; }

        [Required]
        [DisplayName("Quantità")]
        public int Quantita { get; set; }

        [DisplayName("Indirizzo di Spedizione")]
        public string IndirizzoSpedizione { get; set; }

        public string Nota { get; set; }

        [DisplayName("Ordine Confermato")]
        public bool OrdineConfermato { get; set; }

        [DisplayName("Ordine Consegnato")]
        public bool OrdineConsegnato { get; set; }

        [DisplayName("Data Ordinazione")]
        public DateTime DataOrdine { get; set; }

        public int IDPizze { get; set; }

        public int IDUtenti { get; set; }

        public virtual Pizze Pizze { get; set; }

        public virtual Utenti Utenti { get; set; }
    }
}
