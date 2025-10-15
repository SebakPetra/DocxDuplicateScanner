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
                Location = new Point(150, 120),
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
                Size = new Size(450, 25),
                Location = new Point(20, 10),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.TopLeft
            };
            popup.Controls.Add(lblTitle);

            Panel panelFiles = new Panel
            {
                Location = new Point(20, 45),
                Size = new Size(450, 150),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            popup.Controls.Add(panelFiles);

            if (person.Files != null)
            {
                int y = 5;
                int rowHeight = 22;

                foreach (var file in person.Files.Where(f => !string.IsNullOrEmpty(f)))
                {
                    Label lblFile = new Label
                    {
                        Text = file,
                        AutoSize = false,
                        Size = new Size(panelFiles.Width - 10, rowHeight),
                        Location = new Point(5, y),
                        Font = new Font("Segoe UI", 10),
                        TextAlign = ContentAlignment.MiddleLeft
                    };
                    panelFiles.Controls.Add(lblFile);
                    y += rowHeight;
                }
            }

            Button btnClose = new Button
            {
                Text = "Bezár",
                Size = new Size(80, 30),
                Location = new Point((popup.ClientSize.Width - 80) / 2, popup.ClientSize.Height - 50),
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
