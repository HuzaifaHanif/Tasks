using System.ComponentModel.DataAnnotations.Schema;

namespace Task8.Models.Employees
{
    [Table("Employees")]
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? HireDate { get; set; }

        public float? Salary { get; set; }
    }
}
