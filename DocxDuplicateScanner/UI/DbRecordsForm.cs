using System;
using System.Drawing;
using System.Windows.Forms;
using DocxDuplicateScanner.Logic;
using DocxDuplicateScanner.Models;

namespace DocxDuplicateScanner.UI
{
    public class DbRecordsForm : Form
    {
        private DataGridView grid;
        private Button btnDeleteAll;
        private DatabaseService dbService;

        public DbRecordsForm()
        {
            Text = "Mentett rekordok";
            Size = new Size(700, 400);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;

            grid = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 300,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            grid.Columns.Add("Name", "Név");
            grid.Columns.Add("Phone", "Telefonszám");
            grid.Columns.Add("Address", "Cím");
            Controls.Add(grid);

            btnDeleteAll = new Button
            {
                Text = "Teljes adatbázis törlése",
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(220, 40),
                Location = new Point((Width - 240) / 2, 310)
            };
            btnDeleteAll.FlatAppearance.BorderSize = 0;
            btnDeleteAll.Click += BtnDeleteAll_Click;
            Controls.Add(btnDeleteAll);

            LoadData();
        }

        private void LoadData()
        {
            var records = DatabaseService.Instance.GetAllRecords();
            grid.Rows.Clear();
            foreach (var p in records)
                grid.Rows.Add(p.Name, p.Phone, p.Address);
        }

        private void BtnDeleteAll_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Biztosan törölni szeretnéd az összes rekordot?", "Megerősítés",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                DatabaseService.Instance.DeleteAllRecords();
                LoadData();
                MessageBox.Show("Az adatbázis kiürült.", "Törölve");
            }
        }
    }
}
