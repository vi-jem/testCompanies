using System;
using System.Collections.Generic;

namespace myrestful.Models
{
    public class SearchQuery
    {
        public string Keyword { get; set; }
        public DateTime? EmployeeDateOfBirthFrom { get; set; }
        public DateTime? EmployeeDateOfBirthTo { get; set; }
        public IEnumerable<EJobTitle> EmployeeJobTitles { get; set; } = new List<EJobTitle>();
    }
}