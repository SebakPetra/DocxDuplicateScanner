using System;
using System.Drawing;
using System.Windows.Forms;

namespace DocxDuplicateScanner.UI
{
    public class DragDropPanel : Panel
    {
        public event Action<string[]> FilesDropped;

        public DragDropPanel()
        {
            Size = new Size(860, 100);
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = Color.LightGray;

            Label lbl = new Label
            {
                Text = "Húzd ide a DOCX fájlokat",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            Controls.Add(lbl);

            AllowDrop = true;
            DragEnter += (s, e) =>
            {
                e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
                BackColor = Color.LightGreen;
            };
            DragLeave += (s, e) => BackColor = Color.LightGray;
            DragDrop += (s, e) =>
            {
                BackColor = Color.LightGray;
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    FilesDropped?.Invoke(files);
                }
            };
        }
    }
}
