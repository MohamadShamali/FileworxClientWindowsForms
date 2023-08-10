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
using static Fileworx_Client.FileWorx;

namespace Fileworx_Client
{
    public partial class FileWorx : Form
    {
        // Properties
        private static List<clsFile> allFiles { get; set; }
        private static List<clsNews> allNews { get; set; }
        private static List<clsPhoto> allPhotos { get; set; }
        private TabPage hiddenTabPage;
        public enum SortBy { RecentDate, OldestDate, Alphabetically };

        public FileWorx()
        {
            InitializeComponent();

            // UI 
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.FixedPanel = FixedPanel.Panel2;
            label7.Text = Global.LoggedInUser.Name;

            // Hide and save hidden Tab
            hiddenTabPage = tabControl1.TabPages[1];
            tabControl1.TabPages.RemoveAt(1);

            //Admin access
            if (Global.LoggedInUser.IsAdmin) usersListToolStripMenuItem.Enabled = true;
            else usersListToolStripMenuItem.Enabled = false;

            // Add files to listView
            addDBFilesToFilesList();
            sortFilesList(SortBy.RecentDate);
            addFilesListItemsToListView();
        }

        private void addDBFilesToFilesList()
        {
            clsNewsQuery allNewsQuery = new clsNewsQuery();
            allNews = allNewsQuery.Run();

            clsPhotoQuery allPhotosQuery = new clsPhotoQuery();
            allPhotos = allPhotosQuery.Run();


            allFiles = new List<clsFile>();
            allFiles.AddRange(allPhotos);
            allFiles.AddRange(allNews);
        }

        private void addFilesListItemsToListView()
        {
            foreach (clsFile file in allFiles)
            {
                var listViewNews = new ListViewItem($"{file.Name}");
                listViewNews.SubItems.Add($"{file.CreationDate}");
                listViewNews.SubItems.Add($"{file.Description}");
                newsListView.Items.Add(listViewNews);
            }
        }

        private void refreshFilesList()
        {
            allFiles.Clear();
            allNews.Clear();
            allPhotos.Clear(); 
            addDBFilesToFilesList();
        }

        private void sortFilesList(SortBy sortby)
        {
            if (sortby == SortBy.RecentDate)
            {
                var sortedList = (from file in allFiles
                                  orderby file.CreationDate descending
                                  select file).ToList();

                allFiles = sortedList;
            }

            if (sortby == SortBy.OldestDate)
            {
                var sortedList = (from file in allFiles
                                  orderby file.CreationDate ascending
                                  select file).ToList();

                allFiles = sortedList;
            }

            if (sortby == SortBy.Alphabetically)
            {
                var sortedList = (from file in allFiles
                                  orderby file.Name ascending
                                  select file).ToList();

                allFiles = sortedList;
            }
        }
        
        private void clearEverylabels()
        {
            titleLabel.Text = String.Empty;
            dateLabel.Text = String.Empty;
            categoryLabel.Text = String.Empty;
            bodyRichTextBox.Text = String.Empty;
        }

        private clsFile findSelectedFile()
        {
            clsFile selectedFile =
                (from file in allFiles
                 where ((file.Name == (newsListView.SelectedItems[0].Text)) && (file.CreationDate == DateTime.Parse(newsListView.SelectedItems[0].SubItems[1].Text)))
                 select file).FirstOrDefault();

            return selectedFile;
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newsListView_MouseClick(object sender, MouseEventArgs e)
        {


            //if (e.Button == MouseButtons.Right)
            //{
            //    News foundNews = findSelectedNews();


            //    DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //    if (result == DialogResult.Yes)
            //    {
            //        if (foundNews is Image)
            //        {
            //            Image foundImage = (Image)foundNews;
            //            if (File.Exists(foundImage.ImagePath))
            //            {
            //                previewImagePictureBox.Image.Dispose();
            //                previewImagePictureBox.Image = null;
            //            }

            //            clearEverylabels();

            //            if (File.Exists(foundImage.ImagePath))
            //            {
            //                File.Delete(foundImage.ImagePath);
            //            }
            //        }

            //        File.Delete(foundNews.textPath);

            //        newsListView.SelectedItems[0].Remove();
            //        clearEverylabels();
            //    }

            //}

            clearEverylabels();

            if (e.Button == MouseButtons.Left)
            {
                clsFile selectedFile = findSelectedFile();

                label3.Text = "Category:";
                titleLabel.Text = selectedFile.Name;
                dateLabel.Text = selectedFile.CreationDate.ToString();
                categoryLabel.Text = selectedFile.Description;
                bodyRichTextBox.Text = selectedFile.Body;
                previewImagePictureBox.SizeMode = PictureBoxSizeMode.Zoom;

                if (selectedFile is clsPhoto)
                {
                    label3.Text = "";
                    clsPhoto selectedPhoto = (clsPhoto) selectedFile;

                    if (File.Exists(selectedPhoto.Location))
                    {
                        if (tabControl1.TabPages.Count == 1)
                        {
                            tabControl1.TabPages.Add(hiddenTabPage);
                        }
                        previewImagePictureBox.Image= new Bitmap(selectedPhoto.Location);
                    }
                }

                //else
                //{
                //    try
                //    {
                //        hiddenTabPage = tabControl1.TabPages[1];
                //        tabControl1.TabPages.RemoveAt(1);
                //        previewImagePictureBox.Image.Dispose();
                //        previewImagePictureBox.Image = null;
                //    }
                //    catch { }
                //}




                if (newsListView.SelectedItems.Count == 0)
                {
                    clearEverylabels();
                }
            }

        }

        //private void addThisNewsToListView(News news)
        //{
        //    var listViewNews = new ListViewItem($"{news.Title}");
        //    listViewNews.SubItems.Add($"{news.Date}");
        //    listViewNews.SubItems.Add($"{news.Body}");
        //    newsListView.Items.Add(listViewNews);
        //}

        //private void addImageToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Add_Images add_Image = new Add_Images();
        //    DialogResult result = add_Image.ShowDialog();

        //    if (result == DialogResult.OK)
        //    {
        //        RefreshNews();
        //    }
        //}

        //private void recentToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    SortNews(SortBy.RecentDate);
        //    uncheckAllSortByItems();
        //    recentToolStripMenuItem.Checked = true;
        //}

        //private void oldestToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    SortNews(SortBy.OldestDate);
        //    uncheckAllSortByItems();
        //    oldestToolStripMenuItem.Checked = true;
        //}

        //private void alphabeticallyToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    SortNews(SortBy.Alphabetically);
        //    uncheckAllSortByItems();
        //    alphabeticallyToolStripMenuItem.Checked = true;
        //}

        //private void uncheckAllSortByItems()
        //{
        //    recentToolStripMenuItem.Checked = false;
        //    oldestToolStripMenuItem.Checked = false;
        //    alphabeticallyToolStripMenuItem.Checked = false;
        //}



        //private void newsListView_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    News foundNews = findSelectedNews();
        //    if (foundNews is Image)
        //    {
        //        Image foundImage = (Image)foundNews;
        //        Edit_Image editImage = new Edit_Image(foundImage);

        //        DialogResult result = editImage.ShowDialog();
        //        if (result == DialogResult.OK)
        //        {
        //            newsListView.Items.Clear();
        //            addAllSavedNews();
        //            clearEverylabels();
        //        }
        //        try
        //        {
        //            hiddenTabPage = tabControl1.TabPages[1];
        //            tabControl1.TabPages.RemoveAt(1);
        //        }
        //        catch { }
        //    }
        //    else
        //    {
        //        Edit_News editNews = new Edit_News(foundNews);

        //        DialogResult result = editNews.ShowDialog();
        //        if (result == DialogResult.OK)
        //        {
        //            newsListView.Items.Clear();
        //            addAllSavedNews();
        //            clearEverylabels();
        //        }
        //    }
        //}

        //private void usersListToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    UsersList usersList = new UsersList(LoggedInUser);
        //    usersList.ShowDialog();
        //}

        //private void FileWorx_Resize(object sender, EventArgs e)
        //{
        //    tabControl1.Height = this.Height /3 ;
        //    newsListView.Height = splitContainer2.Panel1.Height - 10 ;
        //}

        //private void addNewsToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Add_News add_News = new Add_News();
        //    DialogResult result = add_News.ShowDialog();

        //    if (result == DialogResult.OK)
        //    {
        //        RefreshNews();
        //    }
        //}
        //private void showImage(string ImagePath)
        //{
        //    if(previewImagePictureBox.Image!=null)
        //    {
        //        previewImagePictureBox.Image.Dispose();
        //    }


        //    string showedImagesPath = EditBeforeRun.ImagesDirectory + @"\ShowedImage\FileWorx";
        //    foreach (string file in Directory.GetFiles(showedImagesPath))
        //    {
        //        File.Delete(file);
        //    }
        //    string s = ImagePath.Substring(0, ImagePath.Length - 2); 
        //    string name = Path.GetFileName(s);
        //    string newPath = Path.Combine(showedImagesPath,name );
        //    File.Copy(ImagePath, newPath);

        //    string showedImagePath = Directory.GetFiles(showedImagesPath).FirstOrDefault();
        //    previewImagePictureBox.Image = new Bitmap(showedImagePath);
        //}

    }
}
