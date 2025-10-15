using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DocxDuplicateScanner.UI
{
    public class BrowseButton : Button
    {
        public event Action<IEnumerable<string>> OnFilesSelected;

        private Color defaultColor = Color.LightGray;
        private Color hoverColor = Color.DimGray;

        public BrowseButton()
        {
            Text = "Tallózás...";
            BackColor = defaultColor;
            ForeColor = Color.Black;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Font = new Font("Segoe UI", 10, FontStyle.Bold);
            Size = new Size(120, 30);
            Cursor = Cursors.Hand;

            MouseEnter += (s, e) => BackColor = hoverColor;
            MouseLeave += (s, e) => BackColor = defaultColor;

            Click += BrowseButton_Click;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Word dokumentumok (*.docx)|*.docx";
                ofd.Multiselect = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var validFiles = ofd.FileNames
                        .Where(f => f.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (validFiles.Any())
                    {
                        OnFilesSelected?.Invoke(validFiles);
                    }
                }
            }
        }
    }
}
