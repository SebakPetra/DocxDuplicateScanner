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
        private List<string> files = new List<string>();

        public event Action<string> OnFileRemoved;

        public FileListPanel()
        {
            Size = new Size(860, 150);
            BackColor = Color.LightGray;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            lblFiles = new Label
            {
                Text = "Fájlok:",
                Location = new Point(5, 5),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            Controls.Add(lblFiles);

            listBoxFiles = new ListBox
            {
                Location = new Point(105, 5),
                Size = new Size(740, 140),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                DrawMode = DrawMode.OwnerDrawFixed,
                ItemHeight = 26
            };
            listBoxFiles.DrawItem += ListBoxFiles_DrawItem;
            listBoxFiles.MouseClick += ListBoxFiles_MouseClick;
            Controls.Add(listBoxFiles);
        }

        public void AddFile(string file)
        {
            if (!files.Contains(file))
            {
                files.Add(file);
                listBoxFiles.Items.Add(file);
                listBoxFiles.Refresh();
            }
        }

        public List<string> GetFiles() => files.ToList();

        public void RemoveFile(string file)
        {
            if (files.Contains(file))
            {
                files.Remove(file);
                listBoxFiles.Items.Clear();
                foreach (var f in files)
                    listBoxFiles.Items.Add(f);
                OnFileRemoved?.Invoke(file);
                listBoxFiles.Refresh();
            }
        }

        private void ListBoxFiles_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index < 0) return;

            string fullPath = listBoxFiles.Items[e.Index].ToString()!;
            string fileName = Path.GetFileName(fullPath);

            using (Brush textBrush = new SolidBrush(Color.Black))
                e.Graphics.DrawString(fileName, e.Font, textBrush, e.Bounds.Left + 5, e.Bounds.Top + 2);

            using (Brush xBrush = new SolidBrush(Color.DarkRed))
                e.Graphics.DrawString("✖", e.Font, xBrush, e.Bounds.Right - 20, e.Bounds.Top + 2);

            e.DrawFocusRectangle();
        }

        private void ListBoxFiles_MouseClick(object sender, MouseEventArgs e)
        {
            var idx = listBoxFiles.IndexFromPoint(e.Location);
            if (idx < 0 || idx >= listBoxFiles.Items.Count) return;

            Rectangle itemRect = listBoxFiles.GetItemRectangle(idx);

            int xWidth = 12;
            int xHeight = 12;
            Rectangle rectX = new Rectangle(itemRect.Right - xWidth - 8, itemRect.Top + (itemRect.Height - xHeight) / 2, xWidth, xHeight);

            if (rectX.Contains(e.Location))
            {
                string fullPath = listBoxFiles.Items[idx].ToString()!;
                string fileName = Path.GetFileName(fullPath);

                var res = MessageBox.Show($"Biztosan törlöd a fájlt a listából?\n\n{fileName}", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    RemoveFile(fullPath);
                }
            }
        }
    }
}
