namespace DocxDuplicateScanner.UI
{
    public class ScanButton : Button
    {
        public ScanButton()
        {
            Text = "Scan";
            Size = new Size(130, 35);
            BackColor = Color.FromArgb(150, 0, 0); // bordó
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 11, FontStyle.Bold);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 8, 8));
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 8, 8));
        }

        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );
    }
}
