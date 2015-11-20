using System;

namespace Dashboard.Models.Primavera.Model
{
    public class Pending
    {
        public String DocumentType { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime DueDate { get; set; }
        public String State { get; set; }
        public String Entity { get; set; }
        public String EntityType { get; set; }
        public Money PendingValue { get; set; }
    }
}