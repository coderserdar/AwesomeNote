using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using AwesomeNote.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace AwesomeNote
{
    public partial class FolderSettings : PhoneApplicationPage
    {

        public int noteFolderId;
        public SolidColorBrush messageBackGround;

        public FolderSettings()
        {
            InitializeComponent();
            //SetBackgroundColor();
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
                var noteFolder =
                    context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId)).Single() as NoteFolder;
                lblFontFamily.Text = AppResources.FontFamily + " (" + AppResources.Selected + ": " + noteFolder.FontFamily + ")";
                lblFontSize.Text = AppResources.FontSize + " (" + AppResources.Selected + ": " + noteFolder.FontSize + ")";
                if (noteFolder.NoteOrderBy == "NAME")
                {
                    lblNoteOrder.Text = AppResources.NoteOrder + " (" + AppResources.Selected + ": " + AppResources.NoteName + ")";
                }
                if (noteFolder.NoteOrderBy == "CDATE")
                {
                    lblNoteOrder.Text = AppResources.NoteOrder + " (" + AppResources.Selected + ": " + AppResources.CreationDate + ")";
                }
                if (noteFolder.NoteOrderBy == "MDATE")
                {
                    lblNoteOrder.Text = AppResources.NoteOrder + " (" + AppResources.Selected + ": " + AppResources.ModificationDate + ")";
                }
                if (noteFolder.NoteOrderStyle == "A")
                {
                    lblNoteOrderStyle.Text = AppResources.OrderStyle + " (" + AppResources.Selected + ": " + AppResources.Ascending + ")";
                }
                if (noteFolder.NoteOrderStyle == "D")
                {
                    lblNoteOrderStyle.Text = AppResources.OrderStyle + " (" + AppResources.Selected + ": " + AppResources.Descending + ")";
                }

            }
            SetBackgroundColor();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            piFont.Header = AppResources.Font;
            piOtherSettings.Header = AppResources.OtherSettings;
            piPassword.Header = AppResources.Password;
            piBackground.Header = AppResources.Background;

            btnFontFamily.Content = AppResources.Select;
            btnFontSize.Content = AppResources.Select;
            btnNoteOrder.Content = AppResources.Select;
            btnNoteOrderStyle.Content = AppResources.Select;


            // önce listbox içindeki değerlerin tamamı silinir
            lpPassword.Items.Clear();

            // şifre pivot sayfasında olması gereken düzenlemeler yapılıyor
            lblPassword.Text = AppResources.PasswordState;
            lpPassword.Items.Add(AppResources.SelectPasswordState);
            lpPassword.Items.Add(AppResources.Active);
            lpPassword.Items.Add(AppResources.Passive);

            lblBackgroundImage.Text = AppResources.BackgroundImage;
            btnBackgroundImage.Content = AppResources.Select;
            btnRemoveBackgroundImage.Content = AppResources.RemoveBackgroundImage;

            //chbBold.Content = AppResources.IsBold;
            //chbItalic.Content = AppResources.IsItalic;

        }

        private void lpPassword_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index;

            if (lpPassword.Items.Count > 2)
            {
                index = lpPassword.SelectedIndex;
                // eğer şifre özelliği aktif hale gelmişse
                if (index == 1)
                {

                    NavigationService.Navigate(new Uri("/SetPasswordPage.xaml#" + noteFolderId, UriKind.Relative));

                    //Popup popup = new Popup();
                    //popup.Height = 440;
                    //popup.Width = 480;
                    //popup.VerticalOffset = 20;
                    //PopupSetPassword control = new PopupSetPassword();
                    //control.txtPass1.Text = AppResources.PleaseEnterPassword + " (" + AppResources.FourDigits + ")";
                    //control.txtPass2.Text = AppResources.PleaseReEnterPassword + " (" + AppResources.FourDigits + ")";
                    //control.btnCancel.Content = AppResources.Cancel;
                    //control.btnOK.Content = AppResources.OK;
                    //popup.Child = control;
                    //popup.IsOpen = true;

                    //control.btnOK.Click += (s, args) =>
                    //{
                    // şifre dört karakterli ise 
                    // ve iki şifre alanı da aynı ise
                    //    if ((control.txtFirstPassword.Text == control.txtSecondPassword.Text) &&
                    //        (control.txtFirstPassword.Text.Length == 4))
                    //    {
                    //        using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                    //        {
                    //            var noteFolders =
                    //                context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId)).Select(j => j);
                    //            foreach (var noteFolder in noteFolders)
                    //            {
                    //                noteFolder.IsPasswordProtected = true;
                    //                noteFolder.NoteFolderPassword = control.txtFirstPassword.Text;
                    //                noteFolder.ModificationDate = DateTime.Now;
                    //            }
                    //            //context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId))
                    //            //    .Single()
                    //            //    .NoteFolderPassword = control.txtFirstPassword.Text;
                    //            //context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId))
                    //            //    .Single()
                    //            //    .IsPasswordProtected = true;
                    //            context.SubmitChanges();
                    //            MessageBox.Show(AppResources.SuccessfulSetPassword);
                    //            popup.IsOpen = false;
                    //        }
                    //    }
                    //    // şifreler aynı değilse veya 4 karakter değilse
                    //    else
                    //    {
                    //        MessageBox.Show(AppResources.TwoFieldsNotSame);
                    //        control.txtFirstPassword.Text = "";
                    //        control.txtSecondPassword.Text = "";
                    //        control.txtFirstPassword.Focus();
                    //    }

                    //};
                    //control.btnCancel.Click += (s, args) =>
                    //{
                    //    popup.IsOpen = false;
                    //};
                }
                // eğer şifre kaldırılacaksa
                else if (index == 2)
                {
                    using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                    {
                        var noteFolders =
                            context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId)).Select(j => j);
                        foreach (var noteFolder in noteFolders)
                        {
                            noteFolder.IsPasswordProtected = false;
                            noteFolder.NoteFolderPassword = "";
                            noteFolder.ModificationDate = DateTime.Now;
                        }
                        context.SubmitChanges();
                        MessageBox.Show(AppResources.SuccessfulRemovePassword);
                    }
                }
                else
                {

                }
            }
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/NoteFolder.xaml#" + noteFolderId, UriKind.Relative));
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

        private void btnFontFamily_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FontFamilySettings.xaml#" + noteFolderId, UriKind.Relative));
        }

        private void btnFontSize_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FontSizeSettings.xaml#" + noteFolderId, UriKind.Relative));
        }

        private void btnNoteOrder_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/NoteOrderSettings.xaml#" + noteFolderId, UriKind.Relative));
        }

        private void btnNoteOrderStyle_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/NoteOrderStyleSettings.xaml#" + noteFolderId, UriKind.Relative));
        }

        private void btnBackgroundImage_Click(object sender, RoutedEventArgs e)
        {
            PhotoChooserTask objPhotoChooser = new PhotoChooserTask();
            objPhotoChooser.Completed += new EventHandler<PhotoResult>(PhotoChooseCall);
            objPhotoChooser.Show();
        }

        private void PhotoChooseCall(object sender, PhotoResult e)
        {
            switch (e.TaskResult)
            {
                case TaskResult.OK:
                    using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                    {
                        var noteFolders =
                            context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId)).Select(j => j);
                        foreach (var noteFolder in noteFolders)
                        {
                            noteFolder.FolderBackground = new byte[e.ChosenPhoto.Length];
                            e.ChosenPhoto.Position = 0;
                            e.ChosenPhoto.Read(noteFolder.FolderBackground, 0, noteFolder.FolderBackground.Length);
                            //noteFolder.NoteFolderPassword = "";
                            noteFolder.ModificationDate = DateTime.Now;
                        }
                        context.SubmitChanges();
                        MessageBox.Show(AppResources.SuccessfulBackgroundImageChanged);
                    }
                    break;
                case TaskResult.Cancel:
                    //MessageBox.Show("Cancelled");
                    break;
                case TaskResult.None:
                    //MessageBox.Show("Nothing Entered");
                    break;
            }
            SetBackgroundColor();
        }

        private void btnRemoveBackgroundImage_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                    {
                        var noteFolders =
                            context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId)).Select(j => j);
                        foreach (var noteFolder in noteFolders)
                        {
                            noteFolder.FolderBackground = null;
                            //noteFolder.NoteFolderPassword = "";
                            noteFolder.ModificationDate = DateTime.Now;
                        }
                        context.SubmitChanges();
                        MessageBox.Show(AppResources.SuccessfulBackgroundImageRemoved);
                    }
            }
        }
}