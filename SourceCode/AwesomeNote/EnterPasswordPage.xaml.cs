using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using AwesomeNote.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AwesomeNote
{
    public partial class EnterPasswordPage : PhoneApplicationPage
    {

        public int noteFolderId;
        public int noteId;
        public NoteFolder noteFolder;
        public string pageName;
        public Note note;
        public SolidColorBrush messageBackGround;

        public EnterPasswordPage()
        {
            InitializeComponent();
            //SetBackgroundColor();
            //txtPassword.Focus();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // hangi sayfadan buraya yönlendirme yapılmışsa onun adını almaya yarıyor bu bölüm
            var lastPage = NavigationService.BackStack.FirstOrDefault();
            pageName = lastPage.Source.ToString();
            txtPassword.Focus();
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
            if (pageName == "/MainPage.xaml")
            {
                noteFolderId = int.Parse(e.Fragment);
                using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                {
                    var noteFolders =
                        context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId)).Single() as NoteFolder;
                    noteFolder = noteFolders;
                    lblFolderName.Text = noteFolders.NoteFolderName;
                }
            }
            else
            {
                noteId = int.Parse(e.Fragment);
                using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                {
                    var note = context.Notes.Where(j => j.NoteId.Equals(noteId)).Single() as Note;
                    noteFolderId = note.NoteFolderId;
                    var noteFolders =
                        context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId)).Single() as NoteFolder;
                    noteFolder = noteFolders;
                    lblFolderName.TextWrapping = TextWrapping.Wrap;
                    //lblFolderName.Text = AppResources.SearchResults + " (" + noteFolders.NoteFolderName + ")";
                    lblFolderName.Text = noteFolders.NoteFolderName;
                }
            }
            lblEnterPassword.Text = AppResources.EnterPassword;
            txtLabel.Text = AppResources.PleaseEnterPassword + " (" + AppResources.AtLeastFourDigits + ")";
            //btnCancel.Content = AppResources.Cancel;
            //btnOK.Content = AppResources.OK;
            SetBackgroundColor();

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            string password;
            password = txtPassword.Password;
            // girilen şifre doğruysa
            if (password.Length >= 4 && password == noteFolder.NoteFolderPassword)
            {
                MessageBox.Show(AppResources.PasswordTrue);
                if (pageName == "/MainPage.xaml")
                {
                    NavigationService.Navigate(new Uri("/NoteFolder.xaml#" + noteFolderId, UriKind.Relative));
                }
                else
                {
                    NavigationService.Navigate(new Uri("/NoteDetail.xaml#" + noteId, UriKind.Relative));
                }
            }
            else
            {
                MessageBox.Show(AppResources.PasswordFalse);
                txtPassword.Password = "";
                txtPassword.Focus();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (pageName == "/MainPage.xaml")
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            else
            {
                NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
            }
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            this.svPassword.ScrollToVerticalOffset(this.txtPassword.ActualHeight);
            this.svPassword.UpdateLayout();
        }

        private void txtPassword_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            svPassword.ScrollToVerticalOffset(e.GetPosition(txtPassword).Y - 120);
        }

        private void txtPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string password;
                password = txtPassword.Password;
                // girilen şifre doğruysa
                if (password.Length >= 4 && password == noteFolder.NoteFolderPassword)
                {
                    MessageBox.Show(AppResources.PasswordTrue);
                    if (pageName == "/MainPage.xaml")
                    {
                        NavigationService.Navigate(new Uri("/NoteFolder.xaml#" + noteFolderId, UriKind.Relative));
                    }
                    else
                    {
                        NavigationService.Navigate(new Uri("/NoteDetail.xaml#" + noteId, UriKind.Relative));
                    }
                }
                else
                {
                    MessageBox.Show(AppResources.PasswordFalse);
                    txtPassword.Password = "";
                    txtPassword.Focus();
                }
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