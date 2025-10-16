using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DocxDuplicateScanner.Models;

namespace DocxDuplicateScanner.UI
{
    public class ResultsGrid : DataGridView
    {
        public event Action<Person> OnPersonDoubleClick;

        public ResultsGrid()
        {
            Width = 880;
            Height = 300;
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            AllowUserToAddRows = false;
            ReadOnly = true;

            Columns.Add("Name", "Név");
            Columns.Add("Phone", "Telefonszám");
            Columns.Add("Address", "Cím");
            Columns.Add("Errors", "Hibák");

            CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                    OnPersonDoubleClick?.Invoke(Rows[e.RowIndex].Tag as Person);
            };
        }

        public void UpdateGrid(List<Person> people)
        {
            Rows.Clear();
            foreach (var p in people)
            {
                int idx = Rows.Add(p.Name, p.Phone, p.Address, string.Join("; ", p.Errors));
                Rows[idx].Tag = p;
            }
        }

        public List<Person> GetDisplayedPeople()
        {
            var list = new List<Person>();
            foreach (DataGridViewRow row in Rows)
                if (row.Tag is Person p)
                    list.Add(p);
            return list;
        }
    }
}
