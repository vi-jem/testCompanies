using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace myrestful.Models
{
    public class Company : IEntity
    {
        [Key]
        public long ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int? EstablishmentYear { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}