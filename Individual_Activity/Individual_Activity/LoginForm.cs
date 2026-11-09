using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Individual_Activity
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            txtPass.UseSystemPasswordChar = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                string username = txtUser.Text.Trim();
                string password = txtPass.Text;

                if (username == "" || password == "")
                {
                    MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            try
            {
                using(OleDbConnection con = Database.GetConnection())
                {
                    con.Open();

                    string query = "SELECT * FROM Users WHERE Username = ?";

                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    { 
                    cmd.Parameters.AddWithValue("@Username", username);

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if(!reader.Read())
                            {
                                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            string stroredHash = reader["PasswordHash"].ToString();
                            string fullname = reader["FullName"].ToString();
                            string role = reader["UserRole"].ToString();
                            bool isActive = Convert.ToBoolean(reader["IsActive"]); 
                            int userId = Convert.ToInt32(reader["UserID"]);


                            if(!PasswordHelper.VerifyPassword(password, stroredHash))
                            {
                                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            if (!isActive)
                            {
                                MessageBox.Show("Your account is inactive. Please contact the administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                               MessageBox.Show($"Welcome {fullname}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        reader.Close();

                            InventoryPage inventoryPage = new InventoryPage();
                            inventoryPage.Show();
                            this.Hide();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while logging in:  {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SignupForm signupForm = new SignupForm();
            signupForm.Show();
            this.Hide();
        }
    }
}
