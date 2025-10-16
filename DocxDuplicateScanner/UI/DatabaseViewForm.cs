using DocxDuplicateScanner.Logic;

public class DatabaseViewForm : Form
{
    private DataGridView dataGrid;
    private Button btnDeleteAll;

    public DatabaseViewForm()
    {
        Text = "Adatbázis rekordok";
        Size = new Size(700, 500);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.White;

        dataGrid = new DataGridView
        {
            Dock = DockStyle.Top,
            Height = 400,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        Controls.Add(dataGrid);

        btnDeleteAll = new Button
        {
            Text = "Teljes adatbázis törlése",
            Dock = DockStyle.Bottom,
            Height = 40,
            BackColor = Color.Maroon,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnDeleteAll.FlatAppearance.BorderSize = 0;
        btnDeleteAll.Click += (s, e) =>
        {
            if (MessageBox.Show("Biztosan törölni szeretnéd az összes rekordot?", "Megerősítés", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DatabaseService.Instance.DeleteAllRecords();
                LoadData();
            }
        };
        Controls.Add(btnDeleteAll);

        LoadData();
    }

    private void LoadData()
    {
        var records = DatabaseService.Instance.GetAllRecords();
        dataGrid.Rows.Clear();
        dataGrid.Columns.Clear();

        dataGrid.Columns.Add("Name", "Név");
        dataGrid.Columns.Add("Phone", "Telefonszám");
        dataGrid.Columns.Add("Address", "Cím");
        dataGrid.Columns.Add("Files", "Fájlok");

        foreach (var p in records)
        {
            dataGrid.Rows.Add(p.Name, p.Phone, p.Address, string.Join(", ", p.Files));
        }
    }
}
