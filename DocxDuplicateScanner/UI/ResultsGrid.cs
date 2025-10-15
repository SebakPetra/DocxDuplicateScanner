
using DocxDuplicateScanner.Models;

namespace DocxDuplicateScanner.UI
{
    public class ResultsGrid : DataGridView
    {
        public event Action<Person> OnPersonDoubleClick;

        public ResultsGrid()
        {
            Size = new Size(860, 220);
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            BackgroundColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            ReadOnly = true;

            Columns.Add("Name", "Név");
            Columns.Add("Phone", "Telefonszám");
            Columns.Add("Address", "Cím");
            Columns.Add("Files", "Fájlok");

            CellDoubleClick += ResultsGrid_CellDoubleClick;
        }

        private void ResultsGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < Rows.Count)
            {
                var row = Rows[e.RowIndex];
                var person = new Person
                {
                    Name = row.Cells["Name"].Value?.ToString(),
                    Phone = row.Cells["Phone"].Value?.ToString(),
                    Address = row.Cells["Address"].Value?.ToString(),
                    Files = row.Cells["Files"].Value?.ToString().Split(';').ToList()
                };
                OnPersonDoubleClick?.Invoke(person);
            }
        }

        public void UpdateGrid(List<Person> duplicates)
        {
            Rows.Clear();

            foreach (var p in duplicates)
            {
                Rows.Add(p.Name, p.Phone, p.Address, string.Join("; ", p.Files ?? new List<string>()));
            }
        }
    }
}
