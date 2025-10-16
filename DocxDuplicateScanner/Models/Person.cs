using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DocxDuplicateScanner.Models
{
    public class Person
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<string> Files { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = new List<string>();

        public string UniqueHash
        {
            get
            {
                using var sha = SHA256.Create();
                string raw = $"{Name.Trim().ToLower()}|{Phone.Trim()}|{Address.Trim().ToLower()}";
                byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
                return BitConverter.ToString(hashBytes).Replace("-", "");
            }
        }
    }
}
