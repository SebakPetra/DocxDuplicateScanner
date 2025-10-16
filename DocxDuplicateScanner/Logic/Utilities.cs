using System.Collections.Generic;
using System.Linq;
using DocxDuplicateScanner.Models;

namespace DocxDuplicateScanner.Logic
{
    public static class Utilities
    {
        public static List<Person> FindDuplicates(List<Person> allPeople)
        {
            return allPeople
               .GroupBy(p => p.UniqueHash)
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

        public static List<Person> FindDuplicatesAgainstDatabase(List<Person> people)
        {
            var dbPeople = DatabaseService.Instance.GetAllRecords();
            var dbHashes = dbPeople.Select(p => p.UniqueHash).ToHashSet();

            return people.Where(p => dbHashes.Contains(p.UniqueHash)).ToList();
        }
    }
}
