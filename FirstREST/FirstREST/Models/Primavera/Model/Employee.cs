using System;

namespace Dashboard.Models.Primavera.Model
{
    public class Employee
    {
        public enum GenderType 
        { 
            Female, Male
        };

        public String ID { get; set; }
        public String Name { get; set; }
        public GenderType Gender { get; set; }
        public DateTime HiredOn { get; set; }
        public DateTime FiredOn { get; set; }
        public Money Salary { get; set; }
    }
}