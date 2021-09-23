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
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var taskMasterDbContext = new TaskMasterDbContext();

            var statuses = taskMasterDbContext.Statuses.ToList();

            foreach(Status status in statuses)
            {
                cboStatus.Items.Add(status.Name);
            }

        }
    }
}
