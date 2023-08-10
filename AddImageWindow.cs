using FileworxObjectClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fileworx_Client
{
    public partial class AddImageWindow : CommonNews
    {
        public AddImageWindow()
        {
            InitializeComponent();
        }

        private void cancelAddNewsbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveAddNewsButton_Click(object sender, EventArgs e)
        {
            if ((tiltleTextBox.Text != String.Empty) && (descriptionTextBox.Text != String.Empty) 
                && (bodyTextBox.Text != String.Empty) && (File.Exists(imagePathTextBox.Text)))
            {

                clsPhoto newPhoto = new clsPhoto() 
                { Id = Guid.NewGuid(),
                  Description = descriptionTextBox.Text,
                  CreatorId = Global.LoggedInUser.Id,
                  CreatorName = Global.LoggedInUser.Name,
                  Name = tiltleTextBox.Text,
                  Body = bodyTextBox.Text,
                  Location = imagePathTextBox.Text 
                };
                
                newPhoto.Insert();
            }
            else
            {
                MessageBox.Show("Empty fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void browseImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog browseImageDialog = new OpenFileDialog();
            browseImageDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (browseImageDialog.ShowDialog() == DialogResult.OK)
            {
                imagePathTextBox.Text = browseImageDialog.FileName;
                previewBrowsedPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                previewBrowsedPictureBox.Image = new Bitmap(browseImageDialog.FileName);
            }
        }
    }
}
