using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocxDuplicateScanner.Models;

namespace DocxDuplicateScanner.Logic
{
    public static class DocxProcessor
    {
        public static List<Person> Process(string filePath)
        {
            string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".docx");
            File.Copy(filePath, tempFile, true);

            var people = new List<Person>();

            try
            {
                using (WordprocessingDocument doc = WordprocessingDocument.Open(tempFile, false))
                {
                    var paragraphs = new List<Paragraph>(doc.MainDocumentPart.Document.Body.Elements<Paragraph>());

                    for (int i = 0; i < paragraphs.Count; i++)
                    {
                        string line = paragraphs[i].InnerText.Trim();
                        if (string.IsNullOrEmpty(line)) continue;

                        var match = Regex.Match(line, @"^(\d+)\.\s*(.+)$");
                        if (!match.Success) continue;

                        string serialNumber = match.Groups[1].Value;
                        string content = match.Groups[2].Value;
                        var person = new Person();

                        var phoneMatch = Regex.Match(content, @"(\+?\d[\d\s-]{5,})");
                        if (phoneMatch.Success)
                        {
                            person.Phone = phoneMatch.Groups[1].Value.Trim();
                            person.Name = content.Substring(0, phoneMatch.Index).Trim();
                        }
                        else
                        {
                            person.Errors.Add("Telefonszám hiányzik vagy rossz formátum.");
                            person.Name = content;
                        }

                        if (i + 1 < paragraphs.Count)
                        {
                            string nextLine = paragraphs[i + 1].InnerText.Trim();
                            if (!string.IsNullOrEmpty(nextLine) && !Regex.IsMatch(nextLine, @"^\d+\."))
                            {
                                person.Address = nextLine;
                                i++;
                            }
                            else
                            {
                                person.Errors.Add("Cím hiányzik.");
                            }
                        }
                        else
                        {
                            person.Errors.Add("Cím hiányzik.");
                        }

                        person.Files.Add($"{Path.GetFileName(filePath)} {serialNumber}. sor");

                        people.Add(person);
                    }
                }
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }

            return people;
        }
    }
}
