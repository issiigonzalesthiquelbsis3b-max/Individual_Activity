using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Individual_Activity
{
    public partial class InventoryPage : Form
    {
        public InventoryPage()
        {
            InitializeComponent();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you sure you want to logout?.", "Logout", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        private void label33_Click(object sender, EventArgs e)
        {

        }
    }
}
