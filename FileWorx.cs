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
        private TabPage hiddenTabPage;
        public enum SortBy { RecentDate, OldestDate, Alphabetically };

        public FileWorx()
        {
            InitializeComponent();

            // UI 
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer2.FixedPanel = FixedPanel.Panel2;
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
            clsFileQuery allFilesQuery = new clsFileQuery();

            allFiles = allFilesQuery.Run();
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

        //private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}


        //private void clearEverylabels()
        //{
        //    titleLabel.Text = String.Empty;
        //    dateLabel.Text = String.Empty;
        //    categoryLabel.Text = String.Empty;

        //    bodyRichTextBox.Text = String.Empty;


        //}

        //private News findSelectedNews()
        //{
        //    News foundNews =
        //        (from News in allNews
        //         where News.Title == (newsListView.SelectedItems[0].Text)
        //         select News).FirstOrDefault();
        //    return foundNews;
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

        //private void newsListView_MouseClick(object sender, MouseEventArgs e)
        //{


        //    if (e.Button == MouseButtons.Right)
        //    {
        //        News foundNews = findSelectedNews();


        //        DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        //        if (result == DialogResult.Yes)
        //        {
        //            if (foundNews is Image)
        //            {
        //                Image foundImage = (Image)foundNews;
        //                if (File.Exists(foundImage.ImagePath))
        //                {
        //                    previewImagePictureBox.Image.Dispose();
        //                    previewImagePictureBox.Image = null;
        //                }

        //                clearEverylabels();

        //                if (File.Exists(foundImage.ImagePath))
        //                {
        //                    File.Delete(foundImage.ImagePath);
        //                }
        //            }

        //            File.Delete(foundNews.textPath);

        //            newsListView.SelectedItems[0].Remove();
        //            clearEverylabels();
        //        }

        //    } 

        //        clearEverylabels();

        //    if(e.Button == MouseButtons.Left) 
        //    {
        //        News foundNews = findSelectedNews();

        //        label3.Text = "Category:";
        //        titleLabel.Text = foundNews.Title;
        //        dateLabel.Text = foundNews.Date.ToString();
        //        categoryLabel.Text = foundNews.Category;
        //        bodyRichTextBox.Text = foundNews.Body;
        //        previewImagePictureBox.SizeMode = PictureBoxSizeMode.Zoom;

        //        if (foundNews is Image)
        //        {
        //            label3.Text ="";
        //            Image foundImage = (Image)foundNews;

        //            if (File.Exists(foundImage.ImagePath))
        //            {
        //                if (tabControl1.TabPages.Count == 1)
        //                {
        //                    tabControl1.TabPages.Add(hiddenTabPage);
        //                }
        //                showImage(foundImage.ImagePath);
        //            }
        //        }

        //        else
        //        {
        //            try
        //            {
        //                hiddenTabPage = tabControl1.TabPages[1];
        //                tabControl1.TabPages.RemoveAt(1);
        //                previewImagePictureBox.Image.Dispose();
        //                previewImagePictureBox.Image = null;
        //            }
        //            catch { }
        //        }




        //        if (newsListView.SelectedItems.Count == 0)
        //        {
        //            clearEverylabels();
        //        }
        //    }
        
        //    }
        

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
