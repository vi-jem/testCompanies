using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace myrestful.Models
{
    public class Employee : IEntity
    {
        [Key]
        public long ID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [Required]
        public EJobTitle JobTitle { get; set; }

        [JsonIgnore]
        public virtual Company Company { get; set; }
    }
}