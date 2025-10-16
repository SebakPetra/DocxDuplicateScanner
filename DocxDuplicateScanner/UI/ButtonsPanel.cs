using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DocxDuplicateScanner.UI
{
    public class ButtonsPanel : Panel
    {
        public event EventHandler OnScanClick;
        public event EventHandler OnScanDbClick;
        public event EventHandler OnExportClick;
        public event EventHandler OnSaveDbClick;
        public event EventHandler OnViewDbClick;

        public Button btnScan;
        public Button btnScanDb;
        public Button btnExport;
        public Button btnSaveDb;
        public Button btnViewDb;

        public ButtonsPanel()
        {
            Size = new Size(860, 100);
            InitializeButtons();
        }

        private void InitializeButtons()
        {
            int btnTopWidth = 280;
            int btnBottomWidth = 160;
            int btnHeightTop = 35;
            int btnHeightBottom = 28;

            btnScan = new Button
            {
                Text = "Duplikáció keresése a fájlok között",
                Size = new Size(btnTopWidth, btnHeightTop),
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(0, 0, 0, 2)
            };
            btnScan.FlatAppearance.BorderSize = 0;
            btnScan.Location = new Point(0, 0);
            btnScan.Paint += (s, e) =>
            {
                int radius = 10;
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(btnScan.Width - radius - 1, 0, radius, radius, 270, 90);
                    path.AddArc(btnScan.Width - radius - 1, btnScan.Height - radius - 1, radius, radius, 0, 90);
                    path.AddArc(0, btnScan.Height - radius - 1, radius, radius, 90, 90);
                    path.CloseFigure();

                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    btnScan.Region = new Region(path);
                }
            };
            btnScan.Click += (s, e) => OnScanClick?.Invoke(s, e);

            btnScanDb = new Button
            {
                Text = "Duplikáció keresése az adatbázisban",
                Size = new Size(btnTopWidth, btnHeightTop),
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(0, 0, 0, 2)
            };
            btnScanDb.FlatAppearance.BorderSize = 0;
            btnScanDb.Location = new Point(btnTopWidth + 10, 0);
            btnScanDb.Paint += (s, e) =>
            {
                int radius = 10;
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(btnScanDb.Width - radius - 1, 0, radius, radius, 270, 90);
                    path.AddArc(btnScanDb.Width - radius - 1, btnScan.Height - radius - 1, radius, radius, 0, 90);
                    path.AddArc(0, btnScanDb.Height - radius - 1, radius, radius, 90, 90);
                    path.CloseFigure();

                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    btnScanDb.Region = new Region(path);
                }
            };
            btnScanDb.Click += (s, e) => OnScanDbClick?.Invoke(s, e);

            btnExport = new Button
            {
                Text = "Export Excelbe",
                Size = new Size(btnBottomWidth, btnHeightBottom),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Location = new Point(0, btnHeightTop + 10);
            btnExport.Click += (s, e) => OnExportClick?.Invoke(s, e);

            btnSaveDb = new Button
            {
                Text = "Adatok mentése az adatbázisba",
                Size = new Size(btnBottomWidth + 20, btnHeightBottom),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnSaveDb.FlatAppearance.BorderSize = 0;
            btnSaveDb.Location = new Point(btnBottomWidth + 10, btnHeightTop + 10);
            btnSaveDb.Click += (s, e) => OnSaveDbClick?.Invoke(s, e);

            btnViewDb = new Button
            {
                Text = "Adatbázis megtekintése",
                Size = new Size(btnBottomWidth, btnHeightBottom),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnViewDb.FlatAppearance.BorderSize = 0;
            btnViewDb.Location = new Point(2 * (btnBottomWidth + 10) + 20, btnHeightTop + 10);
            btnViewDb.Click += (s, e) => OnViewDbClick?.Invoke(s, e);

            Controls.AddRange(new Control[] { btnScan, btnScanDb, btnExport, btnSaveDb, btnViewDb });
        }
    }
}
