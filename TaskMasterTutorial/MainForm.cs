using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskMasterTutorial.Model;

namespace TaskMasterTutorial
{
    public partial class MainForm : Form
    {

        private readonly TaskMasterDbContext _taskMasterDbContext;

        public MainForm()
        {
            InitializeComponent();

            try
            {
                _taskMasterDbContext = new TaskMasterDbContext();
            }
            catch (Exception e)
            {
                Load -= MainForm_Load;
                MessageBox.Show($"Something went wrong: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var statuses = _taskMasterDbContext.Statuses.ToList();

            cboStatus.DisplayMember = "Name";
            //cboStatus.Items.AddRange(statuses.ToArray()); // this can be used instead of the foreach loop.
            
            foreach (Status status in statuses)
            {
                // cboStatus.Items.Add(status.Name);
                /*
                 * The above Add() has a problem. 
                 * If we just pass the Name of our status, we wont be able to access other properties of the Status object. 
                 * The combo box list won't receive the object itself, just the string with its name.
                 * 
                 * Here we actually need to override the ToString() method of the Status Model Class, 
                 * since we are using it as a reference to a Status object and want to show the Name 
                 * field of the object within our list.
                 */

                cboStatus.Items.Add(status);
                /* Now we are correctly referencing the object.
                 * And the overriden ToString() method will take care of allowing the ComboBox
                 * to show the "Name" property of this object within the combo box list as we have set.
                 */

                /*
                 * I wasn't satisfied with being stuck to overriding the ToString() method of a Model Class,
                 * so I found out we can set the DisplayMember properties of the Combo Box itself to display the value 
                 * of a property of the object while keeping its reference.
                 * 
                 * I don't know if this is compatible with previous versions of .Net though...
                 */
            }

        }

        private void btnCreateTask_Click(object sender, EventArgs e)
        {
            // Validation
            List<string> invalidFieldNotifications = new List<string>();
            string notifications = "";

            if (cboStatus.SelectedItem == null)
            { invalidFieldNotifications.Add("Please select a status for your Task."); }

            if (string.IsNullOrWhiteSpace(txtTask.Text))
            { invalidFieldNotifications.Add("Please enter a name or description for the Task in the Task field."); }

            if (invalidFieldNotifications.Count > 0)
            { 
                foreach ( var notification in invalidFieldNotifications)
                {
                    notifications += $"• {notification}{(invalidFieldNotifications.Count > 1 ? "\n" : "")}";
                }

                MessageBox.Show(notifications, "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (invalidFieldNotifications.Count == 0)
            {
                // Validation passed - Creating Task.
                var newTask = new Model.Task()
                {
                    Name = txtTask.Text,
                    StatusId = (cboStatus.SelectedItem as Model.Status).Id,
                    DueDate = dateDueDate.Value
                };

                try { 
                    _taskMasterDbContext.Tasks.Add(newTask);
                    _taskMasterDbContext.SaveChanges();
                } 
                catch (Exception ex)
                {
                    MessageBox.Show($"Something went wrong: {ex.Message}", "Error");
                }

                MessageBox.Show("Your Task has been added to the list", "Success", MessageBoxButtons.OK);

                // Clearing the form.
                this.Controls.Clear();
                this.InitializeComponent();
                this.MainForm_Load(this, EventArgs.Empty);

            }

        }

    }
}
