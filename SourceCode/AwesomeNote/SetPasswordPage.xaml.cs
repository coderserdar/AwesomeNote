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
    public partial class SetPasswordPage : PhoneApplicationPage
    {
        public int noteFolderId;
        public SolidColorBrush messageBackGround;

        public SetPasswordPage()
        {
            InitializeComponent();
            //SetBackgroundColor();

            //txtFirstPassword.Focus();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            txtFirstPassword.Focus();
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
                var noteFolders =
                    context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId)).Single() as NoteFolder;
                lblFolderName.Text = noteFolders.NoteFolderName;
                lblSetPassword.Text = AppResources.SetPassword;
                txtPass1.Text = AppResources.PleaseEnterPassword + " (" + AppResources.AtLeastFourDigits + ")";
                txtPass2.Text = AppResources.PleaseReEnterPassword + " (" + AppResources.AtLeastFourDigits + ")";
                //btnCancel.Content = AppResources.Cancel;
                //btnOK.Content = AppResources.OK;
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FolderSettings.xaml#" + noteFolderId, UriKind.Relative));
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if ((txtFirstPassword.Password == txtSecondPassword.Password) &&
                (txtFirstPassword.Password.Length >= 4))
            {
                using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                {
                    var noteFolders =
                        context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId)).Select(j => j);
                    foreach (var noteFolder in noteFolders)
                    {
                        noteFolder.IsPasswordProtected = true;
                        noteFolder.NoteFolderPassword = txtFirstPassword.Password;
                        noteFolder.ModificationDate = DateTime.Now;
                    }
                    //context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId))
                    //    .Single()
                    //    .NoteFolderPassword = txtFirstPassword.Text;
                    //context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId))
                    //    .Single()
                    //    .IsPasswordProtected = true;
                    context.SubmitChanges();
                    MessageBox.Show(AppResources.SuccessfulSetPassword);
                    NavigationService.Navigate(new Uri("/FolderSettings.xaml#" + noteFolderId, UriKind.Relative));
                }
            }
            // şifreler aynı değilse veya 4 karakter değilse
            else
            {
                MessageBox.Show(AppResources.TwoFieldsNotSame);
                txtFirstPassword.Password = "";
                txtSecondPassword.Password = "";
                txtFirstPassword.Focus();
            }
        }

        private void txtFirstPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtSecondPassword.Focus();
        }

        private void txtFirstPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            this.svPassword.ScrollToVerticalOffset(this.txtFirstPassword.ActualHeight);
            this.svPassword.UpdateLayout();
        }

        private void txtFirstPassword_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            svPassword.ScrollToVerticalOffset(e.GetPosition(txtFirstPassword).Y - 120);
        }

        private void txtSecondPassword_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            svPassword.ScrollToVerticalOffset(e.GetPosition(txtSecondPassword).Y - 120);
        }

        private void txtSecondPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            this.svPassword.ScrollToVerticalOffset(this.txtSecondPassword.ActualHeight);
            this.svPassword.UpdateLayout();
        }

        private void txtSecondPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if ((txtFirstPassword.Password == txtSecondPassword.Password) &&
                (txtFirstPassword.Password.Length >= 4))
                {
                    using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                    {
                        var noteFolders =
                            context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId)).Select(j => j);
                        foreach (var noteFolder in noteFolders)
                        {
                            noteFolder.IsPasswordProtected = true;
                            noteFolder.NoteFolderPassword = txtFirstPassword.Password;
                            noteFolder.ModificationDate = DateTime.Now;
                        }
                        //context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId))
                        //    .Single()
                        //    .NoteFolderPassword = txtFirstPassword.Text;
                        //context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId))
                        //    .Single()
                        //    .IsPasswordProtected = true;
                        context.SubmitChanges();
                        MessageBox.Show(AppResources.SuccessfulSetPassword);
                        NavigationService.Navigate(new Uri("/FolderSettings.xaml#" + noteFolderId, UriKind.Relative));
                    }
                }
                // şifreler aynı değilse veya 4 karakter değilse
                else
                {
                    MessageBox.Show(AppResources.TwoFieldsNotSame);
                    txtFirstPassword.Password = "";
                    txtSecondPassword.Password = "";
                    txtFirstPassword.Focus();
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