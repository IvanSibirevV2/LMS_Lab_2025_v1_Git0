using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using Практика_сожрет_тебя.Не_лезь_сюда.images;

namespace Практика_сожрет_тебя.Не_лезь_сюда
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
            this.ActiveControl = label1;
        }


        private void textBoxFirstName_Leave(object sender, EventArgs e)
        {

        }

        private void textBoxLastName_Enter(object sender, EventArgs e)
        {
            if (textBoxLastName.Text.ToLower().Trim().Equals("last name"))
            {
                textBoxLastName.Text = "";
                textBoxLastName.ForeColor = Color.Black;
            }
        }

        private void textBoxLastName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxLastName.Text))
            {
                textBoxLastName.Text = "last name";
                textBoxLastName.ForeColor = Color.Gray;
            }
        }

        private void textBoxEmail_Enter(object sender, EventArgs e)
        {
            if (textBoxEmail.Text.ToLower().Trim().Equals("email address"))
            {
                textBoxEmail.Text = "";
                textBoxEmail.ForeColor = Color.Black;
            }
        }

        private void textBoxEmail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxEmail.Text))
            {
                textBoxEmail.Text = "email address";
                textBoxEmail.ForeColor = Color.Gray;
            }
        }

        private void textBoxUserName_Enter(object sender, EventArgs e)
        {
            if (textBoxUserName.Text.ToLower().Trim().Equals("username"))
            {
                textBoxUserName.Text = "";
                textBoxUserName.ForeColor = Color.Black;
            }
        }

        private void textBoxUserName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxUserName.Text))
            {
                textBoxUserName.Text = "username";
                textBoxUserName.ForeColor = Color.Gray;
            }
        }

        private void textBoxPassword_Enter(object sender, EventArgs e)
        {
            if (textBoxPassword.Text.ToLower().Trim().Equals("password"))
            {
                textBoxPassword.Text = "";
                textBoxPassword.UseSystemPasswordChar = true;
                textBoxPassword.ForeColor = Color.Black;
            }
        }

        private void textBoxPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                textBoxPassword.Text = "password";
                textBoxPassword.UseSystemPasswordChar = false;
                textBoxPassword.ForeColor = Color.Gray;
            }
        }

        private void textBoxPasswordConfirm_Enter(object sender, EventArgs e)
        {
            if (textBoxPasswordConfirm.Text.ToLower().Trim().Equals("confirm password"))
            {
                textBoxPasswordConfirm.Text = "";
                textBoxPasswordConfirm.UseSystemPasswordChar = true;
                textBoxPasswordConfirm.ForeColor = Color.Black;
            }
        }

        private void textBoxPasswordConfirm_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxPasswordConfirm.Text))
            {
                textBoxPasswordConfirm.Text = "confirm password";
                textBoxPasswordConfirm.UseSystemPasswordChar = false;
                textBoxPasswordConfirm.ForeColor = Color.Gray;
            }
        }


        private bool checkTextBoxesValues()
        {
            return
                textBoxFirstName.Text == "first name" ||
                textBoxLastName.Text == "last name" ||
                textBoxEmail.Text == "email address" ||
                textBoxUserName.Text == "username" ||
                textBoxPassword.Text == "password";
        }


        private bool checkUsername()
        {
            DB db = new DB();
            string username = textBoxUserName.Text;

            MySqlCommand command = new MySqlCommand(
                "SELECT * FROM `users` WHERE `username` = @usn", db.getConnection());
            command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = username;

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();

            db.openConnection();
            adapter.Fill(table);
            db.closeConnection();

            return table.Rows.Count > 0;
        }


        private void buttonCreateAccountx_Click(object sender, EventArgs e)
        {
            DB db = new DB();

            MySqlCommand command = new MySqlCommand(
                "INSERT INTO users (firstname, lastname, emailaddress, username, password) " +
                "VALUES (@fn, @ln, @em, @usn, @pass)", db.getConnection());

            command.Parameters.Add("@fn", MySqlDbType.VarChar).Value = textBoxFirstName.Text;
            command.Parameters.Add("@ln", MySqlDbType.VarChar).Value = textBoxLastName.Text;
            command.Parameters.Add("@em", MySqlDbType.VarChar).Value = textBoxEmail.Text;
            command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = textBoxUserName.Text;
            command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = textBoxPassword.Text;

            db.openConnection();

            if (!checkTextBoxesValues())
            {
                if (textBoxPassword.Text.Equals(textBoxPasswordConfirm.Text))
                {
                    if (checkUsername())
                    {
                        MessageBox.Show(
                            "This Username already exists, Select a different One",
                            "Duplicate Username",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Error
                        );
                    }
                    else
                    {
                        if (command.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show(
                                "Your Account Has Been Created",
                                "Account",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                        }
                        else
                        {
                            MessageBox.Show("ERROR", "Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(
                        "Wrong Confirm Password",
                        "Password Error",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Error
                    );
                }
            }
            else
            {
                MessageBox.Show(
                    "Enter Your Informations First",
                    "Empty Data",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Error
                );
            }

            db.closeConnection();
        }



        private void textBoxFirstName_TextChanged(object sender, EventArgs e) { }
        private void textBoxLastName_TextChanged(object sender, EventArgs e) { }
        private void textBoxEmail_TextChanged(object sender, EventArgs e) { }
        private void textBoxUserName_TextChanged(object sender, EventArgs e) { }
        private void textBoxPassword_TextChanged(object sender, EventArgs e) { }
        private void textBoxPasswordConfirm_TextChanged(object sender, EventArgs e) { }
        private void RegisterForm_Load(object sender, EventArgs e) { }


        private void labelClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void labelClose_MouseEnter(object sender, EventArgs e)
        {
            labelClose.ForeColor = Color.Black;
        }

        private void labelClose_MouseLeave(object sender, EventArgs e)
        {
            labelClose.ForeColor = Color.White;
        }

        private void labelGoToLogin_MouseEnter(object sender, EventArgs e)
        {
            labelGoToLogin.ForeColor = Color.Yellow;
        }

        private void labelGoToLogin_MouseLeave(object sender, EventArgs e)
        {
            labelGoToLogin.ForeColor = Color.White;
        }

        private void labelGoToLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginform = new LoginForm();
            loginform.ShowDialog();
        }
    }
}
