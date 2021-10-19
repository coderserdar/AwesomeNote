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
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Primitives;
using Microsoft.Phone.Shell;
using AwesomeNote.Resources;

namespace AwesomeNote
{
    public partial class MainPage : PhoneApplicationPage
    {

        public Popup popup;
        public SolidColorBrush messageBackGround;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            SetBackgroundColor();

            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton button1 = new ApplicationBarIconButton();
            button1.IconUri = new Uri("/Assets/AddFolder.png", UriKind.Relative);
            button1.Text = AppResources.AddFolder;
            ApplicationBar.Buttons.Add(button1);
            button1.Click += new EventHandler(AddFolderButton_Click);

            ApplicationBarIconButton button2 = new ApplicationBarIconButton();
            button2.IconUri = new Uri("/Assets/Search.png", UriKind.Relative);
            button2.Text = AppResources.Search;
            ApplicationBar.Buttons.Add(button2);
            button2.Click += new EventHandler(SearchButton_Click);

            ApplicationBarIconButton button3 = new ApplicationBarIconButton();
            button3.IconUri = new Uri("/Assets/Settings.png", UriKind.Relative);
            button3.Text = AppResources.Settings;
            ApplicationBar.Buttons.Add(button3);
            button3.Click += new EventHandler(SettingsButton_Click);

            ApplicationBarIconButton button4 = new ApplicationBarIconButton();
            button4.IconUri = new Uri("/Assets/About.png", UriKind.Relative);
            button4.Text = AppResources.About;
            ApplicationBar.Buttons.Add(button4);
            button4.Click += new EventHandler(AboutButton_Click);

            popup = new Popup();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
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
            //txtMainApp.Text = AppResources.AppTitle;
            //txtMainFolder.Text = AppResources.AppFolder;
            
            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                var appSettings = context.AppSettings.First() as AppSettings;
                string orderStyle = appSettings.FolderOrderStyle;
                lstFolders.Items.Clear();
                switch (appSettings.FolderOrderBy)
                {
                    case "NAME":
                        if (orderStyle == "A")
                        {
                            lstFolders.ItemsSource = context.NoteFolders.OrderBy(j => j.NoteFolderName).ToList();
                        }
                        else
                        {
                            lstFolders.ItemsSource = context.NoteFolders.OrderByDescending(j => j.NoteFolderName).ToList();
                        }
                        break;
                    case "CDATE":
                        if (orderStyle == "A")
                        {
                            lstFolders.ItemsSource = context.NoteFolders.OrderBy(j => j.CreationDate).ToList();
                        }
                        else
                        {
                            lstFolders.ItemsSource = context.NoteFolders.OrderByDescending(j => j.CreationDate).ToList();
                        }
                        break;
                    case "MDATE":
                        if (orderStyle == "A")
                        {
                            lstFolders.ItemsSource = context.NoteFolders.OrderBy(j => j.ModificationDate).ToList();
                        }
                        else
                        {
                            lstFolders.ItemsSource = context.NoteFolders.OrderByDescending(j => j.ModificationDate).ToList();
                        }
                        break;
                    default:
                        if (orderStyle == "A")
                        {
                            lstFolders.ItemsSource = context.NoteFolders.OrderBy(j => j.NoteFolderName).ToList();
                        }
                        else
                        {
                            lstFolders.ItemsSource = context.NoteFolders.OrderByDescending(j => j.NoteFolderName).ToList();
                        }
                        break;
                }
                lstFolders.DisplayMemberPath = "NameCount";
            }
        }

        private void lstFolders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var noteFolder = (NoteFolder)lstFolders.SelectedItem;
            int folderId = noteFolder.NoteFolderId;

            // eğer açılmak istenen not klasörü şifreli ise
            if (noteFolder.NoteFolderPassword.Length > 0)
            {

                NavigationService.Navigate(new Uri("/EnterPasswordPage.xaml#" + folderId, UriKind.Relative));
                //Popup popup = new Popup();
                //popup.Height = 300;
                //popup.Width = 400;
                //popup.VerticalOffset = 20;
                //PopupEnterPassword control = new PopupEnterPassword();
                //control.txtLabel.Text = AppResources.PleaseEnterPassword + " (" + AppResources.FourDigits + ")" ;
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
                //        NavigationService.Navigate(new Uri("/NoteFolder.xaml#" + folderId, UriKind.Relative));
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
            // eğer açılmak istenen not klasörü şifresiz ise
            else
            {
                NavigationService.Navigate(new Uri("/NoteFolder.xaml#" + folderId, UriKind.Relative));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //while (NavigationService.CanGoBack)
            //NavigationService.RemoveBackEntry();

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            //while (NavigationService.CanGoBack)
            //NavigationService.RemoveBackEntry();

        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (popup.IsOpen)
            {
                popup.IsOpen = false;
            }
            if (MessageBox.Show(AppResources.ExitAppQuestion,
                AppResources.ExitApp, MessageBoxButton.OKCancel)
                != MessageBoxResult.OK)
            {
                e.Cancel = true;
            }
            else
            {
                Application.Current.Terminate();
            }
        }


        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void AddFolderButton_Click(object sender, EventArgs e)
        {
            int noteFolderId;

            popup = new Popup();
            popup.Height = 300;
            popup.Width = 400;
            popup.VerticalOffset = 20;
            PopupAddFolder control = new PopupAddFolder();
            control.txtLabel.Text = AppResources.EnterFolderName;
            control.btnCancel.Content = AppResources.Cancel;
            control.btnOK.Content = AppResources.OK;
            popup.Child = control;
            popup.IsOpen = true;
            control.txtFolderName.Focus();

            control.btnOK.Click += (s, args) =>
            {
                bool folder = false;
                string folderName;
                popup.IsOpen = false;
                folderName = control.txtFolderName.Text;

                int length = control.txtFolderName.Text.Length;
                string space = control.txtFolderName.Text.Substring(length - Math.Min(1, length));
                if (space == " ")
                {
                    folderName = control.txtFolderName.Text.Remove(length - 1, 1);
                }
                else
                {
                    folderName = control.txtFolderName.Text;
                }

                // aynı isimde bir klasörün daha önceden oluşturulup oluşturulmadığını
                // kontrol eden bir kod bölümü
                using (var contextFolder = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                {
                    folder =
                        contextFolder.NoteFolders.Any(j => j.NoteFolderName.Equals(folderName));
                }
                if (folder == true)
                {
                    MessageBox.Show(AppResources.FolderExists);
                }
                // eğer bu isimde bir klasör oluşturulmamışsa
                // oluşturulması için gerekli kodlar aşağıdadır
                else
                {
                    using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                    {
                        //noteFolderId = context.NoteFolders.Count() + 1;
                        NoteFolder noteFolder = new NoteFolder();
                        noteFolder.NoteFolderName = folderName;
                        //noteFolder.NoteFolderId = noteFolderId;
                        noteFolder.IsPasswordProtected = false;
                        noteFolder.NoteCount = 0;
                        noteFolder.NoteFolderPassword = "";
                        noteFolder.FontFamily = "Verdana";
                        noteFolder.FontSize = "26";
                        noteFolder.CreationDate = DateTime.Now;
                        noteFolder.ModificationDate = DateTime.Now;
                        noteFolder.NoteOrderBy = "NAME";
                        noteFolder.NoteOrderStyle = "A";
                        noteFolder.NameCount = noteFolder.NoteFolderName + " (" + noteFolder.NoteCount.ToString() + ")";
                        noteFolder.FolderBackground = null;

                        context.NoteFolders.InsertOnSubmit(noteFolder);
                        context.SubmitChanges();

                        lstFolders.ItemsSource = context.NoteFolders;
                        MessageBox.Show(AppResources.SuccessfulAddNoteFolder);
                        NavigationService.Navigate(new Uri("/NoteFolder.xaml#" + noteFolder.NoteFolderId, UriKind.Relative));
                    }

                }
            };
            control.btnCancel.Click += (s, args) =>
            {
                popup.IsOpen = false;
            };

            //PhoneApplicationPage_Loaded(this, new RoutedEventArgs());
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/GeneralSettings.xaml", UriKind.Relative));
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}