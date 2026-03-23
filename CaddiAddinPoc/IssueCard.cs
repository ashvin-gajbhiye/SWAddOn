using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace CaddiAddinPoc
{
    public class IssueCard : UserControl
    {
        public IssueCard(IssueModel issue)
        {
            this.Width = 320;
            this.Height = 110;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Margin = new Padding(5);

            Label title = new Label();
            title.Text = issue.Code;
            title.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            title.Location = new Point(10, 10);
            title.AutoSize = true;

            Label desc = new Label();
            desc.Text = issue.Description;
            desc.Location = new Point(10, 35);
            desc.Width = 200;

            LinkLabel link = new LinkLabel();
            link.Text = "Open Documentation";
            link.Location = new Point(10, 70);
            link.AutoSize = true;

            link.LinkClicked += (s, e) =>
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = issue.Url,
                    UseShellExecute = true
                });
            };

            ComboBox status = new ComboBox();
            status.Items.AddRange(new string[]
            {
                "Pending",
                "Approved",
                "Rejected"
            });

            status.SelectedIndex = 0;
            status.Location = new Point(220, 10);
            status.Width = 85;

            this.Controls.Add(title);
            this.Controls.Add(desc);
            this.Controls.Add(link);
            this.Controls.Add(status);
        }
    }
}

