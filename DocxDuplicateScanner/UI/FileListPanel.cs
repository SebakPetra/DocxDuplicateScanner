using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DocxDuplicateScanner.UI
{
    public class FileListPanel : Panel
    {
        private Label lblFiles;
        private ListBox listBoxFiles;
        private List<string> files = new();

        public event Action<string> OnFileRemoved;

        public FileListPanel()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            Size = new Size(860, 150);
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = Color.LightGray;

            lblFiles = new Label
            {
                Text = "Fájlok:",
                Location = new Point(10, 10),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            Controls.Add(lblFiles);

            listBoxFiles = new ListBox
            {
                Location = new Point(120, 10),
                Size = new Size(720, 130),
                BackColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.FixedSingle,
                DrawMode = DrawMode.OwnerDrawFixed
            };
            listBoxFiles.DrawItem += ListBoxFiles_DrawItem;
            listBoxFiles.MouseClick += ListBoxFiles_MouseClick;
            Controls.Add(listBoxFiles);
        }

        public void AddFiles(IEnumerable<string> filePaths)
        {
            foreach (var filePath in filePaths)
                AddFile(filePath);
        }

        public void AddFile(string filePath)
        {
            if (!File.Exists(filePath) || files.Contains(filePath)) return;
            files.Add(filePath);
            listBoxFiles.Items.Add(Path.GetFileName(filePath));
        }

        public List<string> GetSelectedFiles()
        {
            return files.ToList();
        }

        private void ListBoxFiles_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index < 0) return;

            string fileName = listBoxFiles.Items[e.Index].ToString();

            using (Brush textBrush = new SolidBrush(Color.Black))
                e.Graphics.DrawString(fileName, e.Font, textBrush, e.Bounds.Left + 5, e.Bounds.Top + 2);

            using (Brush xBrush = new SolidBrush(Color.DarkRed))
                e.Graphics.DrawString("✖", e.Font, xBrush, e.Bounds.Right - 20, e.Bounds.Top + 2);

            e.DrawFocusRectangle();
        }

        private void ListBoxFiles_MouseClick(object sender, MouseEventArgs e)
        {
            int index = listBoxFiles.IndexFromPoint(e.Location);
            if (index < 0) return;

            Rectangle itemRect = listBoxFiles.GetItemRectangle(index);
            if (e.X >= itemRect.Right - 25)
            {
                string fileName = listBoxFiles.Items[index].ToString();
                string fullPath = files[index];

                var result = MessageBox.Show(
                    $"Biztos törölni szeretnéd a(z) {fileName} fájlt a listából?",
                    "Megerősítés",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    files.RemoveAt(index);
                    listBoxFiles.Items.RemoveAt(index);
                    OnFileRemoved?.Invoke(fullPath);
                }
            }
        }
    }
}
