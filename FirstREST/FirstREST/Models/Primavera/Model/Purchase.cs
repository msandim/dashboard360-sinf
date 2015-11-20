using System;

namespace Dashboard.Models.Primavera.Model
{
    public class Purchase
    {
        public String ID { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime DueDate { get; set; } // "Data de Vencimento"
        public DateTime ReceptionDate { get; set; }  // "Data de Descarga"
        public String DocumentType { get; set; }
        public String EntityId { get; set; }
        public String EntityName { get; set; }
        public Product Product { get; set; }
        public Money Value { get; set; }
    }
}
