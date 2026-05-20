using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using SldWorks;
using SwConst;

namespace CaddiAddinPoc
{
    public class IssuePanel : UserControl
    {
        private FlowLayoutPanel issueContainer;
        private TextBox responseBox;
        private SldWorks.SldWorks swApp;

        public IssuePanel(SldWorks.SldWorks app)
        {
            swApp = app;
            BuildUI();
            LoadDummyData();
        }

        private void BuildUI()
        {
            this.Dock = DockStyle.Fill;

            TabControl tabs = new TabControl();
            tabs.Dock = DockStyle.Fill;

            TabPage issuesTab = new TabPage("ISSUES");
            TabPage detailsTab = new TabPage("DETAILS");

            tabs.TabPages.Add(issuesTab);
            tabs.TabPages.Add(detailsTab);

            this.Controls.Add(tabs);

            // ===== ISSUES TAB =====

            Panel issuePanel = new Panel();
            issuePanel.Dock = DockStyle.Fill;

            Button runButton = new Button();
            runButton.Text = "Run Review";
            runButton.Dock = DockStyle.Top;
            runButton.Height = 40;

            runButton.Click += async (s, e) =>
            {
                await RunReview();
            };

            issueContainer = new FlowLayoutPanel();
            issueContainer.Dock = DockStyle.Fill;
            issueContainer.FlowDirection = FlowDirection.TopDown;
            issueContainer.AutoScroll = true;

            issuePanel.Controls.Add(issueContainer);
            issuePanel.Controls.Add(runButton);

            issuesTab.Controls.Add(issuePanel);

            // ===== DETAILS TAB =====

            responseBox = new TextBox();
            responseBox.Multiline = true;
            responseBox.Dock = DockStyle.Fill;
            responseBox.ScrollBars = ScrollBars.Vertical;

            detailsTab.Controls.Add(responseBox);
        }

        private void LoadDummyData()
        {
            issueContainer.Controls.Clear();

            var issues = new List<IssueModel>
    {
        new IssueModel(
            "DR-05-004",
            "No CADDI Logo Found",
            "CADDI logo is missing in drawing.",
            "https://example.com",
            "Pending",
            "2026-04-22 11:30",
            new List<string>
            {
                "Please verify title block.",
                "Logo required before release."
            },
            0.10,
            0.10),

        new IssueModel(
            "DR-05-015",
            "Dimension Mismatch",
            "Detected inconsistent dimension values.",
            "https://example.com",
            "Pending",
            "2026-04-20 09:15",
            new List<string>(),
            0.15,
            0.05),

        new IssueModel(
            "DR-05-101",
            "Hole Alignment Issue",
            "Holes are not properly aligned.",
            "https://example.com",
            "Approved",
            "2026-04-18 15:45",
            new List<string>
            {
                "Checked by QA."
            },
            0.20,
            0.12)
    };

            foreach (var issue in issues)
            {
                issueContainer.Controls.Add(
                    new IssueCard(issue, swApp));
            }
        }

        private async Task RunReview()
        {
            ChecklistDialog dialog = new ChecklistDialog();

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            List<string> selectedChecklist = dialog.SelectedItems;

            MessageBox.Show("Step 1: Button Clicked");

            responseBox.Text = "Running review...\r\n";

            // Step 2
            string path = ExportDrawing();

            if (path == null)
            {
                MessageBox.Show("Step 2 FAILED: Export returned null");
                return;
            }

            MessageBox.Show("Step 2: Export Success\n" + path);

            // Step 3
            string result = await SendToApi(path, selectedChecklist);

            MessageBox.Show("Step 3: API Call Done");

            this.Invoke(new Action(() =>
            {
                responseBox.Text = result;
            }));

            // Step 4
            UpdateIssues();

            MessageBox.Show("Step 4: Issues Updated");
        }

        private string ExportDrawing()
        {
            ModelDoc2 model = swApp.ActiveDoc as ModelDoc2;

            if (model == null || model.GetType() != (int)swDocumentTypes_e.swDocDRAWING)
            {
                MessageBox.Show("Open a drawing first!");
                return null;
            }

            string path = Path.Combine(Path.GetTempPath(), "drawing.png");

            int errors = 0;
            int warnings = 0;

            model.Extension.SaveAs(
                path,
                (int)swSaveAsVersion_e.swSaveAsCurrentVersion,
                (int)swSaveAsOptions_e.swSaveAsOptions_Silent,
                null,
                ref errors,
                ref warnings
            );

            return path;
        }

        private async Task<string> SendToApi(
    string path,
    List<string> checklist)
        {
            using (HttpClient client = new HttpClient())
            {
                var content = new MultipartFormDataContent();

                // =========================
                // Add Image
                // =========================

                var bytes = File.ReadAllBytes(path);

                var imageContent = new ByteArrayContent(bytes);

                imageContent.Headers.ContentType =
                    MediaTypeHeaderValue.Parse("image/png");

                content.Add(imageContent, "file", "drawing.png");

                // =========================
                // Add Checklist Values
                // =========================

                foreach (var item in checklist)
                {
                    content.Add(
                        new StringContent(item),
                        "checklists");
                }

                // =========================
                // Dummy API
                // =========================

                var response = await client.PostAsync(
                    "https://jsonplaceholder.typicode.com/posts",
                    content);

                return await response.Content.ReadAsStringAsync();
            }
        }

        private void UpdateIssues()
        {
            issueContainer.Controls.Clear();

            var issues = new List<IssueModel>
    {
        new IssueModel(
            "DR-05-004",
            "No CADDI Logo Found",
            "CADDI logo is missing in drawing.",
            "https://example.com",
            "Pending",
            "2026-04-22 11:30",
            new List<string>
            {
                "Please verify title block.",
                "Logo required before release."
            },
            0.10,
            0.10),

        new IssueModel(
            "DR-05-015",
            "Dimension Mismatch",
            "Detected inconsistent dimension values.",
            "https://example.com",
            "Pending",
            "2026-04-20 09:15",
            new List<string>(),
            0.15,
            0.05),

        new IssueModel(
            "DR-05-101",
            "Hole Alignment Issue",
            "Holes are not properly aligned.",
            "https://example.com",
            "Approved",
            "2026-04-18 15:45",
            new List<string>
            {
                "Checked by QA."
            },
            0.20,
            0.12)
    };

            foreach (var issue in issues)
            {
                issueContainer.Controls.Add(
                    new IssueCard(issue, swApp));
            }
        }
    }


}