using System;

namespace Dashboard.Models.Primavera.Model
{
    public class Sale : IDocument
    {        
        public String ID { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime DueDate { get; set; } // "Data de Vencimento"
        public DateTime ReceptionDate { get; set; }  // "Data de Descarga"
        public DateTime LoadingDate { get; set; }
        public String DocumentType { get; set; }
        public String ClientId { get; set; }
        public String ClientName { get; set; }
        public Product Product { get; set; }
        public Money Value { get; set; }
        public Double Iva { get; set; }
    }
}