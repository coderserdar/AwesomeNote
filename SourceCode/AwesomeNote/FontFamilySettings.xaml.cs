using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using AwesomeNote.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AwesomeNote
{
    public partial class FontFamilySettings : PhoneApplicationPage
    {
        public int noteFolderId;
        public SolidColorBrush messageBackGround;
        public FontFamilySettings()
        {
            InitializeComponent();
            //SetBackgroundColor();

            lstFontFamily.Items.Clear();
            lstFontFamily.Items.Add("Arial");
            lstFontFamily.Items.Add("Arial Black");
            lstFontFamily.Items.Add("Baskerville Old Face");
            lstFontFamily.Items.Add("Berlin Sans FB");
            lstFontFamily.Items.Add("Bookman Old Style");
            lstFontFamily.Items.Add("Calibri");
            lstFontFamily.Items.Add("Cambria");
            lstFontFamily.Items.Add("Candara");
            lstFontFamily.Items.Add("Comic Sans MS");
            lstFontFamily.Items.Add("Consolas");
            lstFontFamily.Items.Add("Constantia");
            lstFontFamily.Items.Add("Courier New");
            lstFontFamily.Items.Add("DokChampa");
            lstFontFamily.Items.Add("Ebrima");
            lstFontFamily.Items.Add("Georgia");
            lstFontFamily.Items.Add("Lucida Sans Unicode");
            lstFontFamily.Items.Add("Meiryo UI");
            lstFontFamily.Items.Add("Microsoft YaHei");
            lstFontFamily.Items.Add("Malgun Gothic");
            lstFontFamily.Items.Add("Segoe UI");
            lstFontFamily.Items.Add("Segoe WP");
            lstFontFamily.Items.Add("Tahoma");
            lstFontFamily.Items.Add("Trebuchet MS");
            lstFontFamily.Items.Add("Times New Roman");
            lstFontFamily.Items.Add("Verdana");
            lstFontFamily.SelectedIndex = -1;
        }

        private void lstFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstFontFamily.SelectedIndex != -1)
            {
                using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                {
                    var noteFolders = context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId)).Select(j => j);
                    foreach (var noteFolder in noteFolders)
                    {
                        noteFolder.FontFamily = lstFontFamily.SelectedItem.ToString();
                    }
                    context.SubmitChanges();
                    MessageBox.Show(AppResources.SuccessfulFontFamilyChanged);
                }
            }
            NavigationService.Navigate(new Uri("/FolderSettings.xaml#" + noteFolderId, UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        protected override void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            // displays "Fragment: Detail"
            //MessageBox.Show("Folder Id: " + e.Fragment);
            base.OnFragmentNavigation(e);
            noteFolderId = int.Parse(e.Fragment);
            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                var noteFolders = context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId)).Single() as NoteFolder;
                lblFolderName.Text = noteFolders.NoteFolderName;
                lblFontFamily.Text = AppResources.SelectFontFamily;
            }
            SetBackgroundColor();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/FolderSettings.xaml#" + noteFolderId, UriKind.Relative));
            }
        }

        private void SetBackgroundColor()
        {
            AppSettings appSettings = new AppSettings();
            NoteFolder noteFolder;
            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                appSettings = context.AppSettings.First() as AppSettings;
                noteFolder = context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId)).Single() as NoteFolder;
            }

            if (noteFolder.FolderBackground != null)
            {
                MemoryStream stream = new MemoryStream(noteFolder.FolderBackground);
                BitmapImage image = new BitmapImage();
                image.SetSource(stream);
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = image;
                this.LayoutRoot.Background = ib;
            }
            else
            {
                switch (appSettings.AppBackgroundColor)
                {
                    case "BLA":
                        messageBackGround = new SolidColorBrush(Colors.Black);
                        //this.LayoutRoot.Background = new ImageBrush(new BitmapImage());
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Black);
                        break;
                    case "BLU":
                        messageBackGround = new SolidColorBrush(Colors.Blue);
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Blue);
                        break;
                    case "BRO":
                        messageBackGround = new SolidColorBrush(Colors.Brown);
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Brown);
                        break;
                    case "RED":
                        messageBackGround = new SolidColorBrush(Colors.Red);
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Red);
                        break;
                    case "GRE":
                        messageBackGround = new SolidColorBrush(Colors.Green);
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Green);
                        break;
                    case "GRA":
                        messageBackGround = new SolidColorBrush(Colors.Gray);
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Gray);
                        break;
                    case "YEL":
                        messageBackGround = new SolidColorBrush(Colors.Yellow);
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Yellow);
                        break;
                    case "ORA":
                        messageBackGround = new SolidColorBrush(Colors.Orange);
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Orange);
                        break;
                    case "PUR":
                        messageBackGround = new SolidColorBrush(Colors.Purple);
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Purple);
                        break;
                    default:
                        messageBackGround = new SolidColorBrush(Colors.Black);
                        this.LayoutRoot.Background = new SolidColorBrush(Colors.Black);
                        break;
                }
            }
        }
    }
}