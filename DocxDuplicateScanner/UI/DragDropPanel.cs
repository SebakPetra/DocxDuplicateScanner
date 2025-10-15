using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DocxDuplicateScanner.UI
{
    public class DragDropPanel : Panel
    {
        private Label lblDragDrop;
        private Color originalColor;
        public event Action<string[]> FilesDropped;

        public DragDropPanel()
        {
            Size = new Size(860, 100);
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = Color.LightGray;
            originalColor = BackColor;
            AllowDrop = true;

            lblDragDrop = new Label
            {
                Text = "Húzd ide a DOCX fájlokat vagy tallózz...",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            Controls.Add(lblDragDrop);

            DragEnter += OnDragEnter;
            DragLeave += OnDragLeave;
            DragDrop += OnDragDrop;
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                BackColor = Color.LightGreen;
            }
        }

        private void OnDragLeave(object sender, EventArgs e)
        {
            BackColor = originalColor;
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            BackColor = originalColor;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            FilesDropped?.Invoke(files);
        }
    }
}
