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
    public partial class SignupForm : Form
    {
        public SignupForm()
        {
            InitializeComponent();
            button3.Click += button3_Click;
            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;

        }

        private void button3_Click(object sender, EventArgs e)
        {

            string fullName = txtFullname.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (fullName == "" || username == "" || password == "" || confirmPassword == "")


            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (OleDbConnection con = Database.GetConnection())
                {
                    con.Open();

                    string chcekUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = ?";
                    using (OleDbCommand checkCmd = new OleDbCommand(chcekUserQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@Username", username);
                        int userCount = (int)checkCmd.ExecuteScalar();
                        if (userCount > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose a different username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;

                        }
                    }

                    string query = "INSERT INTO Users (FullName, Username, PasswordHash, UserRole, IsActive) VALUES (?, ?, ?, ?, ?)";
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        string passwordHash = PasswordHelper.HashPassword(password);
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        cmd.Parameters.AddWithValue("@UserRole", "User");
                        cmd.Parameters.AddWithValue("@IsActive", true);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Signup successful! You can now log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            txtFullname.Clear();
                            txtUsername.Clear();
                            txtPassword.Clear();
                            txtConfirmPassword.Clear();

                            LoginForm loginForm = new LoginForm();
                            loginForm.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Signup failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while signing up: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();

        }
    }
}
