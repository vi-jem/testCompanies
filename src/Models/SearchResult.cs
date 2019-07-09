using System;
using System.Collections.Generic;

namespace myrestful.Models
{
    public class SearchResult
    {
        public IEnumerable<Company> Results { get; set; }
    }
}