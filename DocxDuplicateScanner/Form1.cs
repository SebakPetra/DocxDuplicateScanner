using DocxDuplicateScanner.UI;
using DocxDuplicateScanner.Logic;
using DocxDuplicateScanner.Models;

namespace DocxDuplicateScanner
{
    public partial class Form1 : Form
    {
        private DragDropPanel dragDropPanel;
        private BrowseButton browseButton;
        private FileListPanel fileListPanel;
        private ScanButton scanButton;
        private ResultsGrid resultsGrid;
        private PopupManager popupManager;

        private List<string> draggedFiles = new List<string>();

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            Text = "Docx Duplicate Scanner";
            Size = new Size(900, 680);
            BackColor = Color.WhiteSmoke;

            popupManager = new PopupManager();

            dragDropPanel = new DragDropPanel();
            dragDropPanel.Location = new Point(10, 10);
            Controls.Add(dragDropPanel);

            browseButton = new BrowseButton();
            browseButton.Location = new Point(10, dragDropPanel.Bottom + 5);
            browseButton.OnFilesSelected += FilesDropped;
            Controls.Add(browseButton);

            fileListPanel = new FileListPanel();
            fileListPanel.Location = new Point(10, 170);
            Controls.Add(fileListPanel);

            scanButton = new ScanButton();
            scanButton.Location = new Point(10, 325);
            scanButton.Click += ScanButton_Click;
            Controls.Add(scanButton);

            resultsGrid = new ResultsGrid();
            resultsGrid.Location = new Point(10, 385);
            resultsGrid.OnPersonDoubleClick += ShowPersonLocations;
            Controls.Add(resultsGrid);
        }

        private void FilesDropped(IEnumerable<string> files)
        {
            var docxFiles = files
                .Where(f => f.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var newFiles = docxFiles.Where(f => !draggedFiles.Contains(f)).ToList();
            if (!newFiles.Any()) return;

            draggedFiles.AddRange(newFiles);
            fileListPanel.AddFiles(newFiles);
        }

        private void ScanButton_Click(object sender, EventArgs e)
        {
            var files = fileListPanel.GetSelectedFiles();

            if (files.Count == 0)
            {
                popupManager.ShowInfo("Hiba", "Nincs behúzott vagy kiválasztott fájl.");
                return;
            }

            List<Person> allPeople = new List<Person>();
            foreach (var file in files)
                allPeople.AddRange(DocxProcessor.Process(file));

            var duplicates = Utilities.FindDuplicates(allPeople);
            resultsGrid.UpdateGrid(duplicates);

            if (duplicates.Count == 0)
                popupManager.ShowInfo("Nincs duplikált", "A dokumentumokban nem található duplikált bejegyzés.");
        }

        private void ShowPersonLocations(Person person)
        {
            popupManager.ShowPersonFiles(person);
        }
    }
}
