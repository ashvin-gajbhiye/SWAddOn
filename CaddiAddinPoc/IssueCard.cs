using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using SldWorks;
using SwConst;

namespace CaddiAddinPoc
{
    public class IssueCard : UserControl
    {
        private SldWorks.SldWorks swApp;

        public IssueCard(
            IssueModel issue,
            SldWorks.SldWorks app)
        {
            swApp = app;

            this.Width = 340;
            this.AutoSize = true;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Margin = new Padding(5);
            this.BackColor = Color.White;

            int y = 10;

            // =========================
            // CODE
            // =========================

            Label code = new Label();
            code.Text = issue.Code;
            code.Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold);

            code.Location = new Point(10, y);
            code.AutoSize = true;

            this.Controls.Add(code);

            // =========================
            // STATUS
            // =========================

            ComboBox status = new ComboBox();

            status.Items.AddRange(new string[]
            {
                "Pending",
                "Approved",
                "Rejected"
            });

            status.SelectedItem = issue.Status;

            status.Width = 100;
            status.Location = new Point(220, y);

            this.Controls.Add(status);

            y += 35;

            // =========================
            // TITLE
            // =========================

            Label title = new Label();

            title.Text = issue.Title;

            title.Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold);

            title.Location = new Point(10, y);

            title.AutoSize = true;

            this.Controls.Add(title);

            y += 30;

            // =========================
            // DESCRIPTION
            // =========================

            Label desc = new Label();

            desc.Text = issue.Description;

            desc.Location = new Point(10, y);

            desc.Width = 300;

            desc.AutoSize = true;

            this.Controls.Add(desc);

            y += 30;

            // =========================
            // TIMESTAMP
            // =========================

            Label time = new Label();

            time.Text = issue.Timestamp;

            time.ForeColor = Color.Gray;

            time.Location = new Point(10, y);

            time.AutoSize = true;

            this.Controls.Add(time);

            y += 30;

            // =========================
            // LINK
            // =========================

            LinkLabel link = new LinkLabel();

            link.Text = "Open Documentation";

            link.Location = new Point(10, y);

            link.AutoSize = true;

            link.Click += (s, e) =>
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = issue.Url,
                    UseShellExecute = true
                });
            };

            this.Controls.Add(link);

            y += 40;

            // =========================
            // COMMENTS
            // =========================

            Panel commentsPanel = new Panel();

            commentsPanel.Location = new Point(10, y);

            commentsPanel.Width = 300;

            commentsPanel.AutoSize = true;

            commentsPanel.Visible = false;

            int commentY = 0;

            foreach (var c in issue.Comments)
            {
                Label comment = new Label();

                comment.Text = "• " + c;

                comment.Location = new Point(0, commentY);

                comment.Width = 280;

                comment.AutoSize = true;

                commentsPanel.Controls.Add(comment);

                commentY += 25;
            }

            this.Controls.Add(commentsPanel);

            // =========================
            // COMMENT BUTTON
            // =========================

            Button commentsBtn = new Button();

            commentsBtn.Text =
                $"Comments ({issue.Comments.Count})";

            commentsBtn.Width = 120;

            commentsBtn.Height = 30;

            commentsBtn.Location = new Point(180, y - 35);

            this.Controls.Add(commentsBtn);

            if (issue.Comments.Count == 0)
            {
                commentsBtn.Enabled = false;
                commentsBtn.Text = "No Comments";
            }
            else
            {
                commentsBtn.Click += (s, e) =>
                {
                    commentsPanel.Visible =
                        !commentsPanel.Visible;

                    this.Height =
                        commentsPanel.Visible
                        ? 250
                        : 180;
                };
            }

            // =====================================
            // HOVER → SHOW MARKER IN SOLIDWORKS
            // =====================================

            this.MouseEnter += (s, e) =>
            {
                ShowMarker(issue.X, issue.Y);
            };
        }

        // =====================================
        // SHOW MARKER IN DRAWING
        // =====================================

        private void ShowMarker(double x, double y)
        {
            try
            {
                ModelDoc2 model =
                    swApp.ActiveDoc as ModelDoc2;

                if (model == null)
                    return;

                // Clear selection
                model.ClearSelection2(true);

                // Create temporary sketch point

                SketchManager sketchMgr =
                    model.SketchManager;

                sketchMgr.InsertSketch(true);

                sketchMgr.CreatePoint(x, y, 0);

                sketchMgr.InsertSketch(true);

                model.GraphicsRedraw2();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}