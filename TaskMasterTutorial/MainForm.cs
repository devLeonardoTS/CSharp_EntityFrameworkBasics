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

            // Handling DataGrid Item Selection event -- Event Handler(Delegates).
            _cellSelectionHandler += DeleteBtnActivationHandler;
            dataGridView1.SelectionChanged += _cellSelectionHandler;
        }

        private EventHandler _cellSelectionHandler;
        private void DeleteBtnActivationHandler(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                btnDeleteTask.Enabled = true;
            } 
            else
            {
                btnDeleteTask.Enabled = false;
            }
            
        }

        private void RefreshMainData()
        {
            BindingSource bindingSource = new BindingSource();
            /*
             * MS recommends the binding of data grids to BindingSources which are intermediary objects that provide some useful features. 
             * Another aproach would be binding the grid's data source to the query you ran against your DB.
             */

            // LINQ query example + usage of our Navigation Property we setup in the Task model. Notice how we se aliases at the select for equally named attributes.
            var query = from task in _taskMasterDbContext.Tasks
                        orderby task.DueDate descending
                        select new { task.Id, TaskName = task.Name, StatusName = task.Status.Name, task.DueDate };

            // Notice how we are setting up an Anonymous Class with our LINQ Query at the select statement through the usage of an object initializer syntax.
            // We are querying our Tasks Table through our DbContext, the Task model will be accessible through the "task" range variable, and finally we will get the desired data using the query operators.
            // We can now use the Queryable object we got from the LINQ Query to turn it into a list, which can be binded to the bindingSource's Data Source.

            bindingSource.DataSource = query.ToList();

            dataGridView1.DataSource = bindingSource;
            // dataGridView1.DataSource = query.ToList(); // This is the "another aproach". The binding source one is recommended tho.

            dataGridView1.Refresh();
            dataGridView1.ClearSelection();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            List<Status> statuses;

            try
            {
                statuses = _taskMasterDbContext.Statuses.ToList();


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
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong: {ex.Message}", "Error");
            }

            try
            {
                RefreshMainData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong while fetching the data: {ex.Message}", "Error");
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

                // Refreshing the main data gridView.
                try
                {
                    RefreshMainData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Something went wrong while fetching the data: {ex.Message}", "Error");
                }

            }

        }

        private void btnDeleteTask_Click(object sender, EventArgs e)
        {
            string notification = "";

            if (dataGridView1.SelectedCells.Count == 0)
            {
                notification = "Please select a Task.";
            }

            if (!string.IsNullOrWhiteSpace(notification))
            {
                MessageBox.Show(notification, "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dataGridView1.SelectedCells.Count > 0)
            {
                //selectedTaskId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

                DialogResult dialogResult = MessageBox.Show(
                    "Do you really want to remove this Task from your Tasks list?",
                    "Attention",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (dialogResult.Equals(DialogResult.No | DialogResult.Cancel | DialogResult.Abort))
                {
                    return;
                }

                try
                {
                    var selectedRowItemId = dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value;
                    var task = _taskMasterDbContext.Tasks.Find((int)selectedRowItemId); // Search an item by its primary key by default.
                    // SelectedRows[0].Cells[0] = first column in the row, which is the Task Id in this case. Notice that the dataGrid has the MultiSelect property as false.

                    _taskMasterDbContext.Remove(task);
                    _taskMasterDbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Something went wrong while removing the selected Task: {ex.Message}", "Error");
                }

                // Refreshing the main data gridView.
                try
                {
                    RefreshMainData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Something went wrong while fetching the data: {ex.Message}", "Error");
                }

            }

        }
    }
}
