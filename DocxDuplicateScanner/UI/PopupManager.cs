using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DocxDuplicateScanner.Models;

namespace DocxDuplicateScanner.UI
{
    public class PopupManager
    {
        public void ShowInfo(string title, string message)
        {
            using Form popup = new Form
            {
                Text = title,
                Size = new Size(400, 200),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                BackColor = Color.White
            };

            Label lbl = new Label
            {
                Text = message,
                AutoSize = false,
                Size = new Size(360, 80),
                Location = new Point(20, 30),
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Segoe UI", 10)
            };
            Button btnOk = new Button
            {
                Text = "OK",
                Size = new Size(80, 30),
                Location = new Point((popup.Width - 80) / 2, popup.Height - 50),
                BackColor = Color.FromArgb(150, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.Click += (s, e) => popup.Close();

            popup.Controls.Add(lbl);
            popup.Controls.Add(btnOk);
            popup.ShowDialog();
        }

        public void ShowPersonFiles(Person person)
        {
            using Form popup = new Form
            {
                Text = $"{person.Name}",
                Size = new Size(500, 300),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                BackColor = Color.White
            };

            Label lblTitle = new Label
            {
                Text = "Az alábbi fájlokban megtalálható:",
                AutoSize = false,
                Size = new Size(450, 20),
                Location = new Point(20, 10),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.TopLeft
            };
            popup.Controls.Add(lblTitle);

            ListBox list = new ListBox
            {
                Location = new Point(20, 40),
                Size = new Size(440, 180),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };
            if (person.Files != null)
                list.Items.AddRange(person.Files.Where(f => !string.IsNullOrEmpty(f)).ToArray());
            popup.Controls.Add(list);

            Button btnClose = new Button
            {
                Text = "Bezár",
                Size = new Size(80, 30),
                Location = new Point(200, 230),
                BackColor = Color.FromArgb(150, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => popup.Close();
            popup.Controls.Add(btnClose);

            popup.ShowDialog();
        }
    }
}
