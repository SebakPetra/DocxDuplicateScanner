using System;
using System.Drawing;
using System.Windows.Forms;

namespace DocxDuplicateScanner.UI
{
    public class BrowseButton : Button
    {
        public event Action<string[]> OnFilesSelected;

        public BrowseButton()
        {
            Text = "Tallózás...";
            Size = new Size(120, 35);
            BackColor = Color.LightGray;
            ForeColor = Color.Black;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;

            var tooltip = new ToolTip();
            tooltip.SetToolTip(this, "Fájlok kiválasztása");

            MouseEnter += (s, e) => BackColor = Color.Gray;
            MouseLeave += (s, e) => BackColor = Color.LightGray;

            Click += (s, e) =>
            {
                using OpenFileDialog ofd = new OpenFileDialog
                {
                    Filter = "Word Documents|*.docx",
                    Multiselect = true
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                    OnFilesSelected?.Invoke(ofd.FileNames);
            };
        }
    }
}
