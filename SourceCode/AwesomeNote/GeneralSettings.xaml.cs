using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using AwesomeNote.Resources;
using Microsoft.Live;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace AwesomeNote
{
    public partial class GeneralSettings : PhoneApplicationPage
    {
        /// <summary>
        ///     Defines the scopes the application needs.
        /// </summary>
        private static readonly string[] scopes = new string[] { "wl.signin", "wl.basic", "wl.offline_access", "wl.skydrive", "wl.skydrive_update" };

        /// <summary>
        ///     Stores the LiveAuthClient instance.
        /// </summary>
        private LiveAuthClient authClient;

        /// <summary>
        ///     Stores the LiveConnectClient instance.
        /// </summary>
        private LiveConnectClient liveClient;

        public int signIn;

        private PhoneApplicationPage currentPage;

        public SolidColorBrush messageBackGround;

        public GeneralSettings()
        {
            InitializeComponent();
            SetBackgroundColor();
            InitializePage();
            signIn = 0;

            //btnOneDrive.Content = AppResources.SignIn;

            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                var appSettings = context.AppSettings.First() as AppSettings;
                if (appSettings.AppLangName == "EN")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.English + ")";
                }
                if (appSettings.AppLangName == "TR")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Turkish + ")";
                }
                if (appSettings.AppLangName == "DE")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.German + ")";
                }
                if (appSettings.AppLangName == "ES")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Spanish + ")";
                }

                if (appSettings.AppLangName == "PT")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Portuguese + ")";
                }
                if (appSettings.AppLangName == "AR")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Arabic + ")";
                }
                if (appSettings.AppLangName == "FA")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Persian + ")";
                }
                if (appSettings.AppLangName == "IT")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Italian + ")";
                }
                if (appSettings.AppLangName == "FR")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.French + ")";
                }
                if (appSettings.AppLangName == "RU")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Russian + ")";
                }
                if (appSettings.AppLangName == "ZH")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Chinese + ")";
                }
                if (appSettings.AppLangName == "SA")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Sanskrit + ")";
                }
                if (appSettings.AppLangName == "TH")
                {
                    lblLanguage.Text = AppResources.Language + " (" + AppResources.Selected + ": " + AppResources.Thai + ")";
                }

                if (appSettings.FolderOrderBy == "NAME")
                {
                    lblFolderOrder.Text = AppResources.FolderOrder + " (" + AppResources.Selected + ": " + AppResources.FolderName + ")";
                }
                if (appSettings.FolderOrderBy == "CDATE")
                {
                    lblFolderOrder.Text = AppResources.FolderOrder + " (" + AppResources.Selected + ": " + AppResources.CreationDate + ")";
                }
                if (appSettings.FolderOrderBy == "MDATE")
                {
                    lblFolderOrder.Text = AppResources.FolderOrder + " (" + AppResources.Selected + ": " + AppResources.ModificationDate + ")";
                }
                if (appSettings.FolderOrderStyle == "A")
                {
                    lblFolderOrderStyle.Text = AppResources.OrderStyle + " (" + AppResources.Selected + ": " + AppResources.Ascending + ")";
                }
                if (appSettings.FolderOrderStyle == "D")
                {
                    lblFolderOrderStyle.Text = AppResources.OrderStyle + " (" + AppResources.Selected + ": " + AppResources.Descending + ")";
                }
                if (appSettings.AppBackgroundColor == "BLA")
                {
                    lblBackgroundColor.Text = AppResources.BackgroundColor + " (" + AppResources.Selected + ": " + AppResources.Black + ")";
                }
                if (appSettings.AppBackgroundColor == "BLU")
                {
                    lblBackgroundColor.Text = AppResources.BackgroundColor + " (" + AppResources.Selected + ": " + AppResources.Blue + ")";
                }
                if (appSettings.AppBackgroundColor == "BRO")
                {
                    lblBackgroundColor.Text = AppResources.BackgroundColor + " (" + AppResources.Selected + ": " + AppResources.Brown + ")";
                }
                if (appSettings.AppBackgroundColor == "RED")
                {
                    lblBackgroundColor.Text = AppResources.BackgroundColor + " (" + AppResources.Selected + ": " + AppResources.Red + ")";
                }
                if (appSettings.AppBackgroundColor == "GRE")
                {
                    lblBackgroundColor.Text = AppResources.BackgroundColor + " (" + AppResources.Selected + ": " + AppResources.Green + ")";
                }
                if (appSettings.AppBackgroundColor == "YEL")
                {
                    lblBackgroundColor.Text = AppResources.BackgroundColor + " (" + AppResources.Selected + ": " + AppResources.Yellow + ")";
                }
                if (appSettings.AppBackgroundColor == "GRA")
                {
                    lblBackgroundColor.Text = AppResources.BackgroundColor + " (" + AppResources.Selected + ": " + AppResources.Gray + ")";
                }
                if (appSettings.AppBackgroundColor == "ORA")
                {
                    lblBackgroundColor.Text = AppResources.BackgroundColor + " (" + AppResources.Selected + ": " + AppResources.Orange + ")";
                }
                if (appSettings.AppBackgroundColor == "PUR")
                {
                    lblBackgroundColor.Text = AppResources.BackgroundColor + " (" + AppResources.Selected + ": " + AppResources.Purple + ")";
                }
            }
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        public string DesignFileName(string fileName)
        {
            fileName = fileName.Replace(":", ".");
            fileName = fileName.Replace("?", ".");
            fileName = fileName.Replace("\"", ".");
            fileName = fileName.Replace("/", ".");
            fileName = fileName.Replace("<", ".");
            fileName = fileName.Replace(">", ".");
            fileName = fileName.Replace("|", ".");
            fileName = fileName.Replace("*", ".");
            return fileName;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //currentPage = ((PhoneApplicationFrame)Application.Current.RootVisual).Content as PhoneApplicationPage;
            //currentPage.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            piLanguage.Header = AppResources.Language;
            piSync.Header = AppResources.Sync;
            piOtherSettings.Header = AppResources.OtherSettings;
            piBackground.Header = AppResources.Background;

            //lblOneDrive.Text = AppResources.OneDrive;

            btnFolderOrder.Content = AppResources.Select;
            btnFolderOrderStyle.Content = AppResources.Select;
            btnLanguage.Content = AppResources.Select;
            btnBackgroundColor.Content = AppResources.Select;
            //btnOneDrive.Content = AppResources.Login;
            //btnOneDrive.SignInText = AppResources.SignIn;
            //btnOneDrive.SignOutText = AppResources.SignOut;
            btnOneDriveSync.Content = AppResources.Sync;
            lblOneDrive.Text = AppResources.OneDrive;
            txtSyncronizing.Text = AppResources.Syncronizing;

            pbSync.Visibility = Visibility.Collapsed;
            txtSyncronizing.Visibility = Visibility.Collapsed;
            txtSyncronizing.BorderBrush = this.LayoutRoot.Background;

            btnRemoveBackgroundImage.Content = AppResources.RemoveBackgroundImage;
            lblBackgroundImage.Text = AppResources.BackgroundImage;
            btnBackgroundImage.Content = AppResources.Select;
            btnResetSettings.Content = AppResources.ResetSettings;

            btnOneDriveSync.IsEnabled = false;
            cbSync.Content = AppResources.SyncOnOneFile;
            cbSync.IsEnabled = false;
            btnOneDrive.Content = "Sign In";

        }

        private void btnFolderOrder_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FolderOrderSettings.xaml", UriKind.Relative));
        }

        private void btnFolderOrderStyle_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FolderOrderStyleSettings.xaml", UriKind.Relative));
        }

        private void btnLanguage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LanguageSettings.xaml", UriKind.Relative));
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
                        var appSettings = context.AppSettings;
                foreach (var appSetting in appSettings)
                {
appSetting.AppBackgroundImage = new byte[e.ChosenPhoto.Length];
                            e.ChosenPhoto.Position = 0;
                            e.ChosenPhoto.Read(appSetting.AppBackgroundImage, 0, appSetting.AppBackgroundImage.Length);
                            //noteFolder.NoteFolderPassword = "";
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
                var appSettings = context.AppSettings;
                foreach (var appSetting in appSettings)
                {
                    appSetting.AppBackgroundImage = null;
                }
                context.SubmitChanges();
                MessageBox.Show(AppResources.SuccessfulBackgroundImageRemoved);
            }
        }

        private void btnBackgroundColor_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/BackgroundColorSettings.xaml", UriKind.Relative));
        }

        private async void btnOneDrive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.btnOneDrive.Content.ToString() == "Sign In" || this.btnOneDrive.Content.ToString() == "Sign in")
                {
                    LiveLoginResult loginResult = await this.authClient.LoginAsync(scopes);
                    if (loginResult.Status == LiveConnectSessionStatus.Connected)
                    {
                        //this.btnOneDrive.Content = AppResources.SignOut;
                        this.btnOneDrive.Content = "Sign Out";

                        this.liveClient = new LiveConnectClient(loginResult.Session);
                        this.GetMe();
                        btnOneDriveSync.IsEnabled = true;
                        cbSync.IsEnabled = true;

                    }
                }
                else
                {
                    this.authClient.Logout();
                    //this.btnOneDrive.Content = AppResources.SignIn;
                    this.btnOneDrive.Content = "Sign Out";
                    btnOneDriveSync.IsEnabled = true;
                    cbSync.IsEnabled = true;
                    //this.tbResponse.Text = "";
                }
            }
            catch (LiveAuthException authExp)
            {
                //this.tbResponse.Text = authExp.ToString();
            }
        }

        private async void InitializePage()
        {
            try
            {
                // bu benim uygulamama ait bir client id
                this.authClient = new LiveAuthClient("000000004C120D50");
                LiveLoginResult loginResult = await this.authClient.InitializeAsync(scopes);
                btnOneDrive.Content = "Sign In";
                if (loginResult.Status == LiveConnectSessionStatus.Connected)
                {
                    //this.btnOneDrive.Content = AppResources.SignOut;
                    this.btnOneDrive.Content = "Sign Out";

                    this.liveClient = new LiveConnectClient(loginResult.Session);
                    //this.GetMe();
                }
            }
            catch (LiveAuthException authExp)
            {
                //this.tbResponse.Text = authExp.ToString();
            }
        }

        private async void GetMe()
        {
            try
            {
                LiveOperationResult operationResult = await this.liveClient.GetAsync("me");

                dynamic properties = operationResult.Result;
                //this.tbResponse.Text = properties.first_name + " " + properties.last_name;
            }
            catch (LiveConnectException e)
            {
                //this.tbResponse.Text = e.ToString();
            }
        }

        private void btnOneDrive_SessionChanged(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e)
        {
            if (e != null && e.Status == LiveConnectSessionStatus.Connected)
            {
                //the session status is connected so we need to set this session status to client
                this.liveClient = new LiveConnectClient(e.Session);
            }
            else
            {
                this.liveClient = null;
            }
        }

        private async void btnOneDriveSync_Click(object sender, RoutedEventArgs e)
        {
            IsolatedStorageFile myIsolatedStorage = null;
            StringBuilder sb = null;
            
            string folderName;
            try
            {
                //var folderData = new Dictionary<string, object>();
                folderName = "Awesome Note (" + DateTime.Now + ")";
                //folderName = folderName.Replace(":", ".");
                //folderName = folderName.Replace("/", ".");

                folderName = DesignFileName(folderName);

                //folderData.Add("name",folderName);
                //LiveConnectClient liveClient = new LiveConnectClient(this.session);
                //LiveOperationResult operationResult =
                    //await this.liveClient.PostAsync("me/skydrive", folderData);
                //dynamic result = operationResult.Result;

                string skyDriveFolder = await CreateDirectoryAsync(liveClient, folderName, "me/skydrive");
                if (cbSync.IsChecked == false)
                {
                    btnOneDrive.IsEnabled = false;
                    pbSync.Visibility = Visibility.Visible;
                    txtSyncronizing.Visibility = Visibility.Visible;

                    using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                    {
                        //var noteFolders = context.NoteFolders.ToList() as List<NoteFolder>;
                        var notes = context.Notes.ToList() as List<Note>;

                        for (int i = 0; i < notes.Count; i++)
                        {
                            var noteFolder =
                                context.NoteFolders.Where(j => j.NoteFolderId.Equals(notes[i].NoteFolderId)).Single() as
                                    NoteFolder;


                            string fileName = Guid.NewGuid() + ". " + notes[i].NoteName + " (" + noteFolder.NoteFolderName +
                                              ").txt";

                            fileName = DesignFileName(fileName);

                            //StringBuilder sb = new StringBuilder();
                            //sb.AppendLine(AppResources.NoteName + ": " + notes[i].NoteName);
                            //sb.AppendLine(AppResources.FolderName + ": " + noteFolder.NoteFolderName);
                            //sb.AppendLine(AppResources.Password + ": " + noteFolder.IsPasswordProtected);
                            //sb.AppendLine(AppResources.CreationDate + ": " + notes[i].CreationDate);
                            //sb.AppendLine(AppResources.ModificationDate + ": " + notes[i].ModificationDate);
                            //sb.AppendLine(AppResources.Note + ": " + notes[i].NoteDescription);


                            myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();//deletes the file if it already exists
                            //if (myIsolatedStorage.FileExists(fileName))
                            //{
                            //myIsolatedStorage.DeleteFile(fileName);
                            //}//now we use a StreamWriter to write inputBox.Text to the file and save it to IsolatedStorage
                            using (StreamWriter writeFile = new StreamWriter
                            (new IsolatedStorageFileStream(fileName, FileMode.Create, FileAccess.Write, myIsolatedStorage)))
                            {
                                writeFile.WriteLine(AppResources.NoteName + ": " + notes[i].NoteName);
                                writeFile.WriteLine(AppResources.FolderName + ": " + noteFolder.NoteFolderName);
                                writeFile.WriteLine(AppResources.PasswordProtection + ": " + (noteFolder.IsPasswordProtected == true ? AppResources.Yes : AppResources.No));
                                writeFile.WriteLine(AppResources.CreationDate + ": " + notes[i].CreationDate);
                                writeFile.WriteLine(AppResources.ModificationDate + ": " + notes[i].ModificationDate);
                                writeFile.WriteLine(AppResources.Note + ": " + notes[i].NoteDescription);
                                writeFile.Close();
                            }
                            IsolatedStorageFileStream isfs = myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read);
                            var res = await liveClient.UploadAsync(skyDriveFolder, fileName, isfs, OverwriteOption.Overwrite);
                            pbSync.Value = (i + 1) * (100) / notes.Count;
                            //var res = await liveClient.UploadAsync("me/skydrive/" + folderName, fileName, isfs, OverwriteOption.Overwrite);
                        }
                    }
                    //this.infoTextBlock.Text = string.Join(" ", "Created folder:", result.name, "ID:", result.id);
                    MessageBox.Show(AppResources.OneDriveSyncCompleted);    
                }
                else
                {
                    using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                    {
                        //var noteFolders = context.NoteFolders.ToList() as List<NoteFolder>;
                        var notes = context.Notes.OrderBy(j => j.CreationDate).ToList() as List<Note>;

                        var noteFirst = notes.First();
                        var noteLast = notes.Last();

                        string fileName = Guid.NewGuid() + ". Awesome Note (" + noteFirst.CreationDate.ToShortDateString() + " - " + noteLast.CreationDate.ToShortDateString() +").txt";
                        fileName = DesignFileName(fileName);

                        myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();//deletes the file if it already exists
                        sb = new StringBuilder();
                        for (int i = 0; i < notes.Count; i++)
                        {
                            var noteFolder =
                                context.NoteFolders.Where(j => j.NoteFolderId.Equals(notes[i].NoteFolderId)).Single() as
                                    NoteFolder;

                            sb.AppendLine();
                            sb.AppendLine(AppResources.NoteName + ": " + notes[i].NoteName);
                            sb.AppendLine(AppResources.FolderName + ": " + noteFolder.NoteFolderName);
                            sb.AppendLine(AppResources.Password + ": " + noteFolder.IsPasswordProtected);
                            sb.AppendLine(AppResources.CreationDate + ": " + notes[i].CreationDate);
                            sb.AppendLine(AppResources.ModificationDate + ": " + notes[i].ModificationDate);
                            sb.AppendLine(AppResources.Note + ": " + notes[i].NoteDescription);
                            sb.AppendLine();
                            
                            //if (myIsolatedStorage.FileExists(fileName))
                            //{
                            //myIsolatedStorage.DeleteFile(fileName);
                            //}//now we use a StreamWriter to write inputBox.Text to the file and save it to IsolatedStorage
                            
                            //pbSync.Value = (i + 1) * (100) / notes.Count;
                            //var res = await liveClient.UploadAsync("me/skydrive/" + folderName, fileName, isfs, OverwriteOption.Overwrite);
                        }

                        using (StreamWriter writeFile = new StreamWriter
                            (new IsolatedStorageFileStream(fileName, FileMode.Create, FileAccess.Write, myIsolatedStorage)))
                        {
                            writeFile.Write(sb.ToString());
                            writeFile.Close();
                        }

                        IsolatedStorageFileStream isfs = myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read);
                        var res = await liveClient.UploadAsync(skyDriveFolder, fileName, isfs, OverwriteOption.Overwrite);
                    }
                    //this.infoTextBlock.Text = string.Join(" ", "Created folder:", result.name, "ID:", result.id);
                    MessageBox.Show(AppResources.OneDriveSyncCompleted);
                }

                pbSync.Visibility = Visibility.Collapsed;
                txtSyncronizing.Visibility = Visibility.Collapsed;
                pbSync.Value = 0;
                btnOneDrive.IsEnabled = true;
            }
            catch (Exception exception)
            {
                //this.infoTextBlock.Text = "Error creating folder: " + exception.Message;
                MessageBox.Show(AppResources.SystemFault);
            }
        }

        public async static Task<string> CreateDirectoryAsync(LiveConnectClient client,
string folderName, string parentFolder)
        {
            string folderId = null;

            // Retrieves all the directories.
            var queryFolder = parentFolder + "/files?filter=folders,albums";
            var opResult = await client.GetAsync(queryFolder);
            dynamic result = opResult.Result;

            foreach (dynamic folder in result.data)
            {
                // Checks if current folder has the passed name.
                if (folder.name.ToLowerInvariant() == folderName.ToLowerInvariant())
                {
                    folderId = folder.id;
                    break;
                }
            }

            if (folderId == null)
            {
                // Directory hasn't been found, so creates it using the PostAsync method.
                var folderData = new Dictionary<string, object>();
                folderData.Add("name", folderName);
                opResult = await client.PostAsync(parentFolder, folderData);
                result = opResult.Result;

                // Retrieves the id of the created folder.
                folderId = result.id;
            }

            return folderId;
        }

        private void btnResetSettings_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                var appSettings = context.AppSettings;
                foreach (var appSetting in appSettings)
                {
                    appSetting.AppBackgroundImage = null;
                    appSetting.AppBackgroundColor = "BLA";
                }
                context.SubmitChanges();
                this.LayoutRoot.Background = new SolidColorBrush(Colors.Black);
                MessageBox.Show(AppResources.SuccessfulResetSettings);
            }
        }

        
    }
}