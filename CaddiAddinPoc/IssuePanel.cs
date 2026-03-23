using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaddiAddinPoc
{
    public class IssuePanel : UserControl
    {
        private FlowLayoutPanel issueContainer;

        // DETAILS TAB CONTROLS
        private Button testHttpButton;
        private TextBox responseBox;

        public IssuePanel()
        {
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

            // =========================
            // ISSUES TAB
            // =========================
            issueContainer = new FlowLayoutPanel();
            issueContainer.Dock = DockStyle.Fill;
            issueContainer.FlowDirection = FlowDirection.TopDown;
            issueContainer.WrapContents = false;
            issueContainer.AutoScroll = true;

            issuesTab.Controls.Add(issueContainer);

            // =========================
            // DETAILS TAB
            // =========================

            // Panel layout for details
            Panel detailsPanel = new Panel();
            detailsPanel.Dock = DockStyle.Fill;

            // Test HTTP Button
            testHttpButton = new Button();
            testHttpButton.Text = "Test HTTP Call";
            testHttpButton.Height = 40;
            testHttpButton.Dock = DockStyle.Top;

            testHttpButton.Click += async (s, e) =>
            {
                await TestHttpCall();
            };

            // Response Text Box
            responseBox = new TextBox();
            responseBox.Multiline = true;
            responseBox.Dock = DockStyle.Fill;
            responseBox.ScrollBars = ScrollBars.Vertical;

            detailsPanel.Controls.Add(responseBox);
            detailsPanel.Controls.Add(testHttpButton);

            detailsTab.Controls.Add(detailsPanel);
        }

        private void LoadDummyData()
        {
            List<IssueModel> issues = new List<IssueModel>
            {
                new IssueModel("DR-001", "Duplicate dimension detected",
                    "https://example.com/1"),

                new IssueModel("DR-015", "Tolerance mismatch found",
                    "https://example.com/2"),

                new IssueModel("DR-101", "Hole alignment issue",
                    "https://example.com/3")
            };

            foreach (var issue in issues)
            {
                issueContainer.Controls.Add(new IssueCard(issue));
            }
        }

        // =========================
        // Dummy HTTP Call
        // =========================
        private async Task TestHttpCall()
        {
            responseBox.Text = "Calling API...\r\n";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Public dummy API for testing
                    string url = "https://jsonplaceholder.typicode.com/todos/1";

                    var response = await client.GetAsync(url);
                    string content = await response.Content.ReadAsStringAsync();

                    responseBox.Text = "Response received:\r\n\r\n" + content;
                }
            }
            catch (Exception ex)
            {
                responseBox.Text = "Error:\r\n" + ex.Message;
            }
        }
    }
}