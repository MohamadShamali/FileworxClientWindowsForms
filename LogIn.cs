using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileworxObjectClassLibrary;

namespace Fileworx_Client
{
    public partial class LogIn : Form
    {
        // Properties
        private clsUser loggedInUser = new clsUser();

        public LogIn()
        {
            InitializeComponent();
        }

        private void logInClearTextboxes()
        {
            logInLogInNameTextBox.Text = String.Empty;
            logInPasswordTextBox.Text = String.Empty;
        }

        private void logInButton_Click(object sender, EventArgs e)
        {

            clsUser tryingToLogIn= new clsUser() { Username = logInLogInNameTextBox.Text, Password = logInPasswordTextBox.Text };
            clsUser.LogInValidationResult validateResult = tryingToLogIn.ValidateLogin();

            if(validateResult == clsUser.LogInValidationResult.Valid)
            {
                loggedInUser = tryingToLogIn;
                this.Hide();

                FileWorx fileWorx = new FileWorx(loggedInUser);
                DialogResult result = fileWorx.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    this.Show();
                }

                logInClearTextboxes();
            }

            else if (validateResult == clsUser.LogInValidationResult.WrongPassword)
            {
                logInPasswordTextBox.Text = String.Empty;

                MessageBox.Show($"Invalid password for {tryingToLogIn.Username}, please try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (validateResult == clsUser.LogInValidationResult.WrongUser)
            {
                logInPasswordTextBox.Text = String.Empty;

                MessageBox.Show($"There is no username with the name {tryingToLogIn.Username}, please try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
        
    }
}
