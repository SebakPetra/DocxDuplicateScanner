using DocxDuplicateScanner.Models;
using OfficeOpenXml;

namespace DocxDuplicateScanner.Logic
{
    public static class ExcelExporter
    {
        public static void ExportToExcel(List<Person> people, string filePath)
        {

            if (people == null || people.Count == 0)
                throw new ArgumentException("Nincs exportálható adat.", nameof(people));

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Adatok");

            worksheet.Cells[1, 1].Value = "Név";
            worksheet.Cells[1, 2].Value = "Telefon";
            worksheet.Cells[1, 3].Value = "Cím";
            worksheet.Cells[1, 4].Value = "Fájlok";

            for (int i = 0; i < people.Count; i++)
            {
                var p = people[i];
                worksheet.Cells[i + 2, 1].Value = p.Name;
                worksheet.Cells[i + 2, 2].Value = p.Phone;
                worksheet.Cells[i + 2, 3].Value = p.Address;
                worksheet.Cells[i + 2, 4].Value = string.Join(", ", p.Files);
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            FileInfo file = new FileInfo(filePath);
            package.SaveAs(file);
        }
    }
}