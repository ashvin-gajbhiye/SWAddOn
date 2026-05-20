using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CaddiAddinPoc
{
    public class ChecklistDialog : Form
    {
        private CheckedListBox checklistBox;
        private Button runButton;
        private Button cancelButton;

        public List<string> SelectedItems { get; private set; }

        public ChecklistDialog()
        {
            this.Text = "Checklist Selection";
            this.Width = 500;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label title = new Label();
            title.Text = "Checklist Selection";
            title.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            title.Location = new Point(20, 20);
            title.AutoSize = true;

            checklistBox = new CheckedListBox();
            checklistBox.Location = new Point(20, 60);
            checklistBox.Width = 420;
            checklistBox.Height = 200;

            checklistBox.Items.Add("チェックリスト");
            checklistBox.Items.Add("15 rows token check");
            checklistBox.Items.Add("DebugRegex");
            checklistBox.Items.Add("Checklist Attributes");

            runButton = new Button();
            runButton.Text = "Run Review";
            runButton.Width = 120;
            runButton.Height = 40;
            runButton.Location = new Point(320, 300);

            runButton.Click += RunButton_Click;

            cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.Width = 100;
            cancelButton.Height = 40;
            cancelButton.Location = new Point(200, 300);

            cancelButton.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            this.Controls.Add(title);
            this.Controls.Add(checklistBox);
            this.Controls.Add(runButton);
            this.Controls.Add(cancelButton);
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            SelectedItems = new List<string>();

            foreach (var item in checklistBox.CheckedItems)
            {
                SelectedItems.Add(item.ToString());
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}