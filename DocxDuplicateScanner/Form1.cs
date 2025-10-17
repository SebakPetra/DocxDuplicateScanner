using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DocxDuplicateScanner.Models;
using DocxDuplicateScanner.Logic;
using DocxDuplicateScanner.UI;

namespace DocxDuplicateScanner
{
    public partial class Form1 : Form
    {
        private DragDropPanel dragDropPanel;
        private BrowseButton browseButton;
        private FileListPanel fileListPanel;
        private ButtonsPanel buttonsPanel;
        private ResultsGrid resultsGrid;
        private PopupManager popupManager;

        private Label lblDuplicateRecords;

        private List<string> draggedFiles = new List<string>();
        private List<Person> duplicates = new List<Person>();

        public Form1()
        {
            InitializeComponent();
            InitializeUI();

            Icon = Properties.Resources.gpc_dds_icon;
        }

        private void InitializeUI()
        {
            Text = "GPC Docx Duplicate Scanner";
            Size = new Size(915, 890);
            BackColor = Color.WhiteSmoke;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            var headerPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(this.ClientSize.Width, 65),
                BackColor = Color.WhiteSmoke
            };
            this.Controls.Add(headerPanel);

            // Logo
            var logo = new PictureBox
            {
                Image = Properties.Resources.gpc_dds_image_bold,
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(13, 7),
                Size = new Size(60, 60)
            };
            headerPanel.Controls.Add(logo);

            // Cím
            var titleLabel = new Label
            {
                Text = "GPC Docx Duplicate Scanner",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(70, 15),
                AutoSize = true,
                ForeColor = Color.Black
            };
            headerPanel.Controls.Add(titleLabel);

            popupManager = new PopupManager();

            // Drag & Drop panel
            dragDropPanel = new DragDropPanel { Location = new Point(10, 75) };
            dragDropPanel.FilesDropped += FilesDropped;
            Controls.Add(dragDropPanel);

            // Browse button
            browseButton = new BrowseButton { Location = new Point(10, dragDropPanel.Bottom + 8) };
            browseButton.OnFilesSelected += FilesDropped;
            Controls.Add(browseButton);

            // File list panel
            fileListPanel = new FileListPanel { Location = new Point(10, browseButton.Bottom + 20) };
            fileListPanel.OnFileRemoved += FileListPanel_OnFileRemoved;
            Controls.Add(fileListPanel);

            // Buttons panel
            buttonsPanel = new ButtonsPanel { Location = new Point(10, fileListPanel.Bottom + 10) };
            buttonsPanel.OnScanClick += ScanButton_Click;
            buttonsPanel.OnScanDbClick += ScanWithDbButton_Click;
            buttonsPanel.OnExportClick += ExportButton_Click;
            buttonsPanel.OnSaveDbClick += SaveToDbButton_Click;
            buttonsPanel.OnViewDbClick += ViewRecordsButton_Click;
            Controls.Add(buttonsPanel);

            lblDuplicateRecords = new Label
            {
                Text = "Duplikált nevek száma: 0",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(10, buttonsPanel.Bottom + 5),
                AutoSize = true
            };
            Controls.Add(lblDuplicateRecords);

            // Results grid
            resultsGrid = new ResultsGrid { Location = new Point(10, lblDuplicateRecords.Bottom + 5) };
            resultsGrid.OnPersonDoubleClick += ShowPersonLocations;
            Controls.Add(resultsGrid);
        }

        private void FileListPanel_OnFileRemoved(string file)
        {
            draggedFiles.Remove(file);
        }

        private void FilesDropped(IEnumerable<string> files)
        {
            foreach (var file in files.Where(f => f.EndsWith(".docx", StringComparison.OrdinalIgnoreCase)))
            {
                if (!draggedFiles.Contains(file))
                {
                    draggedFiles.Add(file);
                    fileListPanel.AddFile(file);
                }
            }
        }
        private void UpdateDuplicateStats(List<Person> allPeople)
        {
            var duplicates = allPeople.Where(p => p.Files.Count > 1).ToList();
            int duplicateRecords = duplicates.Count;

            lblDuplicateRecords.Text = $"Duplikált nevek száma: {duplicateRecords}";
        }

        private void ScanButton_Click(object sender, EventArgs e)
        {
            var files = fileListPanel.GetFiles();
            if (!files.Any())
            {
                popupManager.ShowInfo("Hiba", "Nincs kiválasztott vagy behúzott fájl.");
                return;
            }

            List<Person> allPeople = new List<Person>();
            foreach (var file in files)
                allPeople.AddRange(DocxProcessor.Process(file));

            duplicates = Utilities.FindDuplicates(allPeople);
            resultsGrid.UpdateGrid(duplicates.Distinct().ToList());

            UpdateDuplicateStats(duplicates.Distinct().ToList());

            if (!duplicates.Any())
                popupManager.ShowInfo("Nincs duplikált", "A dokumentumokban nem található duplikált bejegyzés.");
        }

        private void ScanWithDbButton_Click(object sender, EventArgs e)
        {
            var files = fileListPanel.GetFiles();
            var existing = DatabaseService.Instance.GetFilesAlreadySaved(files);

            if (existing.Count > 0)
            {
                var popup = new AlreadySavedPopup(files, existing, resultsGrid, popupManager);
                popup.Show();
            }
            else
            {
                var people = new List<Person>();
                foreach (var file in files)
                    people.AddRange(DocxProcessor.Process(file));

                var duplicates = DatabaseService.Instance.FindDuplicates(people);
                resultsGrid.UpdateGrid(duplicates);

                UpdateDuplicateStats(duplicates.Distinct().ToList());

                if (!duplicates.Any())
                    popupManager.ShowInfo("Nincs duplikált", "Az adatbázisban nem található duplikált bejegyzés.");
            }
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            var people = resultsGrid.GetDisplayedPeople();
            if (!people.Any())
            {
                popupManager.ShowInfo("Hiba", "Nincs mit exportálni.");
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                FileName = "Duplikaltak.xlsx"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                ExcelExporter.ExportToExcel(people, sfd.FileName);
                popupManager.ShowInfo("Kész", "Exportálás megtörtént.");
            }
        }

        private void SaveToDbButton_Click(object sender, EventArgs e)
        {
            var files = fileListPanel.GetFiles();
            if (!files.Any())
            {
                popupManager.ShowInfo("Hiba", "Nincs kiválasztott vagy behúzott fájl.");
                return;
            }

            List<Person> people = new List<Person>();
            foreach (var file in files)
                people.AddRange(DocxProcessor.Process(file));

            if (!people.Any())
            {
                popupManager.ShowInfo("Hiba", "Nincs menteni való adat.");
                return;
            }

            DatabaseService.Instance.SaveOrUpdatePeople(people.Distinct().ToList());
            popupManager.ShowInfo("Mentés kész", "Az adatok mentésre kerültek az adatbázisba.");
        }

        private void ViewRecordsButton_Click(object sender, EventArgs e)
        {
            var allRecords = DatabaseService.Instance.GetAllRecords();
            var dbForm = new DatabaseViewForm();
            dbForm.ShowDialog();
        }

        private void ShowPersonLocations(Person person)
        {
            popupManager.ShowPersonFiles(person);
        }
    }
}
