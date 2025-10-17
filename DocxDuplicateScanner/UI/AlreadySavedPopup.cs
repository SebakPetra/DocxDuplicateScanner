using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DocxDuplicateScanner.Logic;
using DocxDuplicateScanner.Models;

namespace DocxDuplicateScanner.UI
{
    public class AlreadySavedPopup
    {
        private Form popup;
        private FlowLayoutPanel filesPanel;
        private List<string> allFiles;
        private List<string> existingFiles;
        private ResultsGrid resultsGrid;
        private PopupManager popupManager;

        public AlreadySavedPopup(List<string> allFiles, List<string> existingFiles, ResultsGrid resultsGrid, PopupManager popupManager)
        {
            this.allFiles = allFiles;
            this.existingFiles = existingFiles;
            this.resultsGrid = resultsGrid;
            this.popupManager = popupManager;
            InitializePopup();
        }

        private void InitializePopup()
        {
            popup = new Form
            {
                Text = "Már mentett fájlok",
                Size = new Size(520, 300),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                BackColor = Color.White,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblMessage = new Label
            {
                Text = "A következő fájlok már az adatbázisban vannak:",
                AutoSize = false,
                Size = new Size(480, 30),
                Location = new Point(10, 10),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10)
            };
            popup.Controls.Add(lblMessage);

            filesPanel = new FlowLayoutPanel
            {
                Location = new Point(10, 50),
                Size = new Size(480, 150),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            popup.Controls.Add(filesPanel);

            foreach (var file in existingFiles)
            {
                var lbl = new Label
                {
                    Text = file,
                    AutoSize = false,
                    Size = new Size(460, 20),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 9)
                };
                filesPanel.Controls.Add(lbl);
            }

            int yButtons = 210;

            Button btnContinue = new Button
            {
                Text = "Folytatás így is",
                Size = new Size(120, 35),
                Location = new Point(10, yButtons),
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnContinue.FlatAppearance.BorderSize = 0;
            btnContinue.Click += (s, e) =>
            {
                ScanFiles(allFiles);
                popup.Close();
            };
            popup.Controls.Add(btnContinue);

            Button btnSkip = new Button
            {
                Text = "Már létezők kihagyása",
                Size = new Size(200, 35),
                Location = new Point(140, yButtons),
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSkip.FlatAppearance.BorderSize = 0;
            btnSkip.Click += (s, e) =>
            {
                var newFiles = allFiles.Except(existingFiles).ToList();
                ScanFiles(newFiles);
                popup.Close();
            };
            popup.Controls.Add(btnSkip);

            // Mégse
            Button btnCancel = new Button
            {
                Text = "Mégse",
                Size = new Size(120, 35),
                Location = new Point(360, yButtons),
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) =>
            {
                resultsGrid.UpdateGrid(new List<Person>());
                popup.Close();
            };
            popup.Controls.Add(btnCancel);
        }

        public void Show()
        {
            popup.ShowDialog();
        }

        private void ScanFiles(List<string> filesToScan)
        {
            if (filesToScan == null || filesToScan.Count == 0)
            {
                resultsGrid.UpdateGrid(new List<Person>());
                popupManager.ShowInfo("Hiba", "Nincs scannelhető fájl.");
                return;
            }

            var allDuplicates = new List<Person>();

            foreach (var file in filesToScan)
            {
                var people = DocxProcessor.Process(file);
                var duplicates = DatabaseService.Instance.FindDuplicates(people);
                allDuplicates.AddRange(duplicates);
            }

            resultsGrid.UpdateGrid(allDuplicates);
        }
    }
}
