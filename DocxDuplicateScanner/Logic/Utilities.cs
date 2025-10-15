using DocxDuplicateScanner.Models;
using System.Collections.Generic;
using System.Linq;

namespace DocxDuplicateScanner.Logic
{
    public static class Utilities
    {
        public static List<Person> FindDuplicates(List<Person> allPeople)
        {
             return allPeople
                .GroupBy(p => p.UniqueKey)
                .Where(g => g.Count() > 1)
                .Select(g =>
                {
                    var first = g.First();
                    foreach (var p in g.Skip(1))
                        first.Files.AddRange(p.Files);
                    return first;
                })
                .ToList();
        }
    }
}
