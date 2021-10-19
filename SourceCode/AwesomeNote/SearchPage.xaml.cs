using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using AwesomeNote.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AwesomeNote
{
    public partial class SearchPage : PhoneApplicationPage
    {
        public SolidColorBrush messageBackGround;
        public SearchPage()
        {
            InitializeComponent();
            SetBackgroundColor();

            txtSearchResult.Text = AppResources.SearchResults;
            lblSearch.Text = AppResources.Search;
            //btnSearch.Content = AppResources.Search;
            //lstSearch.SelectedIndex = -1;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text.TrimStart().Length < 1)
            {
                MessageBox.Show(AppResources.SearchTrimFault);
            }
            else
            {
                lstSearch.Items.Clear();
                using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                {
                    var noteList =
                        context.Notes.Where(j => j.NameDescription.ToLower().Contains(txtSearch.Text.ToLower())).ToList() as List<Note>;
                    //var noteList = context.Notes.ToList() as List<Note>;

                    if (noteList != null)
                    {
                        txtSearchResult.Text = AppResources.SearchResults + " (" + noteList.Count() + ")";
                    }

                    //lstSearch.ItemsSource = noteList;
                    for (int i = 0; i < noteList.Count; i++)
                    {
                        //if (noteList[i].NameDescriptionWithoutNewline.ToLower(Thread.CurrentThread.CurrentCulture).IndexOf(txtSearch.Text.ToLower(Thread.CurrentThread.CurrentCulture)) != -1)
                        //{
                        lstSearch.Items.Add(noteList[i] as Note);
                        //}
                    }
                    //lstSearch.ItemTemplate.
                    //lstSearch.DisplayMemberPath = "NoteName" + " (" + "CreationDate" + ")";
                    lstSearch.DisplayMemberPath = "NameCreation";
                    MessageBox.Show(AppResources.SearchCompleted);
                }
            }
        }

        private void lstSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstSearch.SelectedIndex != -1)
                {
                    Note selectedNote = lstSearch.SelectedItem as Note;
                    using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                    {
                        var noteFolder =
                            context.NoteFolders.Where(j => j.NoteFolderId.Equals(selectedNote.NoteFolderId)).Single() as
                                NoteFolder;
                        if (noteFolder.IsPasswordProtected == true)
                        {

                            // burada parametre olarak notun kendi id bilgisi gönderiliyor ki
                            // aynı sayfadan döndüğünde sorunsuz bir şekilde sorgu sonucu not dosyasına yönlendirme yapılsın
                            NavigationService.Navigate(new Uri("/EnterPasswordPage.xaml#" + selectedNote.NoteId, UriKind.Relative));

                            //Popup popup = new Popup();
                            //popup.Height = 300;
                            //popup.Width = 400;
                            //popup.VerticalOffset = 20;
                            //PopupEnterPassword control = new PopupEnterPassword();
                            //control.txtLabel.Text = AppResources.PleaseEnterPassword + " (" + AppResources.FourDigits + ")";
                            //control.btnCancel.Content = AppResources.Cancel;
                            //control.btnOK.Content = AppResources.OK;
                            //popup.Child = control;
                            //popup.IsOpen = true;
                            //control.txtPassword.Focus();

                            //control.btnOK.Click += (s, args) =>
                            //{
                            //    string password;
                            //    popup.IsOpen = false;
                            //    password = control.txtPassword.Text;
                            //    // girilen şifre doğruysa
                            //    if (password.Length == 4 && password == noteFolder.NoteFolderPassword)
                            //    {
                            //        MessageBox.Show(AppResources.PasswordTrue);
                            //        NavigationService.Navigate(new Uri("/NoteDetail.xaml#" + selectedNote.NoteId,
                            //            UriKind.Relative));
                            //    }
                            //    else
                            //    {
                            //        MessageBox.Show(AppResources.PasswordFalse);
                            //        control.txtPassword.Text = "";
                            //        control.txtPassword.Focus();
                            //    }
                            //};
                            //control.btnCancel.Click += (s, args) =>
                            //{
                            //    popup.IsOpen = false;
                            //};
                        }
                        else
                        {
                            NavigationService.Navigate(new Uri("/NoteDetail.xaml#" + selectedNote.NoteId, UriKind.Relative));
                        }
                    }
                    lstSearch.SelectedIndex = -1;
                }

            }
            catch (Exception)
            {
                MessageBox.Show(AppResources.SystemFault);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private void txtSearch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtSearch.Text.TrimStart().Length < 1)
                {
                    MessageBox.Show(AppResources.SearchTrimFault);
                }
                else
                {
                    lstSearch.Items.Clear();
                    using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                    {
                        var noteList =
                            context.Notes.Where(j => j.NameDescription.ToLower().Contains(txtSearch.Text.ToLower())).ToList() as List<Note>;
                        //var noteList = context.Notes.ToList() as List<Note>;

                        if (noteList != null)
                        {
                            txtSearchResult.Text = AppResources.SearchResults + " (" + noteList.Count() + ")";
                        }

                        //lstSearch.ItemsSource = noteList;
                        for (int i = 0; i < noteList.Count; i++)
                        {
                            //if (noteList[i].NameDescriptionWithoutNewline.ToLower(Thread.CurrentThread.CurrentCulture).IndexOf(txtSearch.Text.ToLower(Thread.CurrentThread.CurrentCulture)) != -1)
                            //{
                            lstSearch.Items.Add(noteList[i] as Note);
                            //}
                        }
                        //lstSearch.ItemTemplate.
                        //lstSearch.DisplayMemberPath = "NoteName" + " (" + "CreationDate" + ")";
                        lstSearch.DisplayMemberPath = "NameCreation";
                        MessageBox.Show(AppResources.SearchCompleted);
                        lstSearch.SelectedIndex = -1;
                    }
                }
            }
        }

        private void SetBackgroundColor()
        {
            AppSettings appSettings = new AppSettings();
            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                appSettings = context.AppSettings.First() as AppSettings;
            }

            if (appSettings.AppBackgroundImage != null)
            {
                MemoryStream stream = new MemoryStream(appSettings.AppBackgroundImage);
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

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            txtSearch.Focus();
        }
    }
}