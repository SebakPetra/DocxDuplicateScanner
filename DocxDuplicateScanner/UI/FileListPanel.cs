using System;
using System.Collections.Generic;
using System.Drawing;
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
                Location = new Point(105 , 5),
                Size = new Size(740, 140),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                DrawMode = DrawMode.OwnerDrawFixed
            };
            listBoxFiles.DrawItem += ListBoxFiles_DrawItem;
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
                listBoxFiles.Items.Remove(file);
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


        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            int index = listBoxFiles.IndexFromPoint(e.Location);
            if (index >= 0)
            {
                int xWidth = 16;
                int xHeight = listBoxFiles.ItemHeight - 8;
                Rectangle rectX = new Rectangle(listBoxFiles.Right - xWidth - 4, listBoxFiles.Top + index * listBoxFiles.ItemHeight + 4, xWidth, xHeight);
                if (rectX.Contains(e.Location))
                {
                    RemoveFile(listBoxFiles.Items[index].ToString()!);
                }
            }
        }

    }
}
