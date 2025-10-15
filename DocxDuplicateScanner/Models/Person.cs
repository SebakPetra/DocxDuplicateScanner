using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocxDuplicateScanner.Models
{
    public class Person
    {
        public string Name { get; set; } = "N/A";
        public string Phone { get; set; } = "N/A";
        public string Address { get; set; } = "N/A";
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Files { get; set; } = new List<string>();
        public string UniqueKey => $"{Name.ToLowerInvariant()}_{Phone}";
    }
}
