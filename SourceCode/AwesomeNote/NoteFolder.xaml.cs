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

namespace AwesomeNote
{
    public partial class NoteList : PhoneApplicationPage
    {

        //List<Note> noteListTemplate { set; get; } 
        public NoteList()
        {
            InitializeComponent();
            //SetBackgroundColor();

            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton button1 = new ApplicationBarIconButton();
            button1.IconUri = new Uri("/Assets/AddNote.png", UriKind.Relative);
            button1.Text = AppResources.AddNote;
            ApplicationBar.Buttons.Add(button1);
            button1.Click += new EventHandler(AddNoteButton_Click);

            ApplicationBarIconButton button2 = new ApplicationBarIconButton();
            button2.IconUri = new Uri("/Assets/DeleteFolder.png", UriKind.Relative);
            button2.Text = AppResources.DeleteFolder;
            ApplicationBar.Buttons.Add(button2);
            button2.Click += new EventHandler(DeleteFolderButton_Click);

            ApplicationBarIconButton button3 = new ApplicationBarIconButton();
            button3.IconUri = new Uri("/Assets/FolderSettings.png", UriKind.Relative);
            button3.Text = AppResources.FolderSettings;
            ApplicationBar.Buttons.Add(button3);
            button3.Click += new EventHandler(FolderSettingsButton_Click);
            
            popup = new Popup();
            //lstNoteList.DataContext = noteListTemplate;
        }

        public int noteFolderId;
        public int noteId;
        public Popup popup;
        public SolidColorBrush messageBackGround;

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // not klasörünün altındaki notlar listelenirken
        // ada göre- oluşturma zamanına göre vs. vs.
        // artan azalan bir şekilde ayarlanıp öyle listeleniyor
        protected override void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            List<Note> noteList = new List<Note>();

            // displays "Fragment: Detail"
            //MessageBox.Show("Folder Id: " + e.Fragment);
            base.OnFragmentNavigation(e);
            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                var noteFolder = context.NoteFolders.Where( j => j.NoteFolderId.Equals(e.Fragment)).Single() as NoteFolder;
                string orderStyle = noteFolder.NoteOrderStyle;
                switch (noteFolder.NoteOrderBy)
                {
                    case "NAME":
                        if (orderStyle == "A")
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(e.Fragment)).OrderBy(j => j.NoteName).ToList() as List<Note>; 
                        }
                        else
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(e.Fragment)).OrderByDescending(j => j.NoteName).ToList() as List<Note>; 
                        }
                        break;
                    case "CDATE":
                        if (orderStyle == "A")
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(e.Fragment)).OrderBy(j => j.CreationDate).ToList() as List<Note>; 
                        }
                        else
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(e.Fragment)).OrderByDescending(j => j.CreationDate).ToList() as List<Note>; 
                        }
                        break;
                    case "MDATE":
                        if (orderStyle == "A")
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(e.Fragment)).OrderBy(j => j.ModificationDate).ToList() as List<Note>; 
                        }
                        else
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(e.Fragment)).OrderByDescending(j => j.ModificationDate).ToList() as List<Note>; 
                        }
                        break;
                    default:
                        if (orderStyle == "A")
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(e.Fragment)).OrderBy(j => j.NoteName).ToList() as List<Note>; 
                        }
                        else
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(e.Fragment)).OrderByDescending(j => j.NoteName).ToList() as List<Note>; 
                        }
                        break;
                }
                
                lstNoteList.Items.Clear();
                noteFolderId = noteFolder.NoteFolderId;
                lblNoteFolder.Text = noteFolder.NoteFolderName;
                lblNoteList.Text = AppResources.NoteListOf + " (" + noteFolder.NoteFolderName + ")";
                lstNoteList.ItemsSource = noteList;
                lstNoteList.DisplayMemberPath = "NoteName";
                SetBackgroundColor();
                //lstNoteList.DisplayMemberPath = "NameCreation";
            }
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (popup.IsOpen)
            {
                popup.IsOpen = false;
            }
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private void lstNoteList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var note = (Note)lstNoteList.SelectedItem;
            noteId = note.NoteId;
            noteFolderId = note.NoteFolderId;
            NavigationService.Navigate(new Uri("/NoteDetail.xaml#" + noteId, UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private void AddNoteButton_Click(object sender, EventArgs e)
        {
            popup = new Popup();
            popup.Height = 300;
            popup.Width = 400;
            popup.VerticalOffset = 20;
            PopupAddNote control = new PopupAddNote();
            control.txtLabel.Text = AppResources.EnterNoteName;
            control.btnCancel.Content = AppResources.Cancel;
            control.btnOK.Content = AppResources.OK;
            popup.Child = control;
            popup.IsOpen = true;
            control.txtNoteName.Focus();

            control.btnOK.Click += (s, args) =>
            {
                bool isCreated;
                string noteName;
                popup.IsOpen = false;
                noteName = control.txtNoteName.Text;

                int length = control.txtNoteName.Text.Length;
                string space = control.txtNoteName.Text.Substring(length - Math.Min(1, length));
                if (space == " ")
                {
                    noteName = control.txtNoteName.Text.Remove(length - 1, 1);
                }
                else
                {
                    noteName = control.txtNoteName.Text;
                }

                // aynı isimde bir klasörün daha önceden oluşturulup oluşturulmadığını
                // kontrol eden bir kod bölümü
                using (var contextFolder = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                {
                    isCreated =
                        contextFolder.Notes.Any(j => j.NoteName.Equals(noteName));
                }
                if (isCreated == true)
                {
                    MessageBox.Show(AppResources.NoteExists);
                }
                // eğer bu isimde bir klasör oluşturulmamışsa
                // oluşturulması için gerekli kodlar aşağıdadır
                else
                {
                    using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                    {
                        Note note = new Note();
                        note.NoteName = noteName;
                        note.NoteFolderId = noteFolderId;
                        note.NoteDescription = "";
                        note.CreationDate = DateTime.Now;
                        note.ModificationDate = DateTime.Now;
                        note.NameCreation = note.NoteName + " (" + note.CreationDate.ToString() + ")";
                        note.NameDescription = note.NoteName + " " + note.NoteDescription;
                        //note.NameDescriptionWithoutNewline = note.NameDescription.Replace(Environment.NewLine," ");
                        //note.IsPasswordProtected = false;

                        context.Notes.InsertOnSubmit(note);
                        context.SubmitChanges();

                        var noteFolders = context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId)).Select(j => j);
                        foreach (var noteFolder in noteFolders)
                        {
                            noteFolder.ModificationDate = DateTime.Now;
                            noteFolder.NoteCount = noteFolder.NoteCount + 1;
                            noteFolder.NameCount = noteFolder.NoteFolderName + " (" + noteFolder.NoteCount + ")";
                        }
                        context.SubmitChanges();

                        lstNoteList.ItemsSource = context.Notes.Where(j => j.NoteFolderId.Equals(noteFolderId));
                        MessageBox.Show(AppResources.SuccessfulAddNote);
                        Note note2 = context.Notes.Where(j => j.NoteName.Equals(noteName)).Single() as Note;
                        NavigationService.Navigate(new Uri("/NoteDetail.xaml#" + note2.NoteId, UriKind.Relative));
                    }
                }
            };
            control.btnCancel.Click += (s, args) =>
            {
                popup.IsOpen = false;
            };

            //PhoneApplicationPage_Loaded(this, new RoutedEventArgs());
        }

        private void FolderSettingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/FolderSettings.xaml#" + noteFolderId, UriKind.Relative));
        }

        private void lblNoteFolder_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            popup = new Popup();
            popup.Height = 300;
            popup.Width = 400;
            popup.VerticalOffset = 20;
            PopupChangeFolderName control = new PopupChangeFolderName();
            control.txtLabel.Text = AppResources.EnterNoteName;
            control.btnCancel.Content = AppResources.Cancel;
            control.btnOK.Content = AppResources.OK;
            popup.Child = control;
            popup.IsOpen = true;
            control.txtFolderName.Text = lblNoteFolder.Text;
            control.txtFolderName.Focus();
            control.txtFolderName.Select(0, control.txtFolderName.Text.Length);

            control.btnOK.Click += (s, args) =>
            {
                bool isCreated;
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


                if (folderName != lblNoteFolder.Text)
                {
                    // aynı isimde bir klasörün daha önceden oluşturulup oluşturulmadığını
                    // kontrol eden bir kod bölümü
                    using (var contextFolder = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                    {
                        isCreated =
                            contextFolder.NoteFolders.Any(j => j.NoteFolderName.Equals(folderName));
                    }
                    if (isCreated == true)
                    {
                        MessageBox.Show(AppResources.FolderExists);
                    }
                    // eğer bu isimde bir klasör oluşturulmamışsa
                    // oluşturulması için gerekli kodlar aşağıdadır
                    else
                    {
                        using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                        {
                            var noteFolder = context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId)).Select(j => j);
                            foreach (var folder in noteFolder)
                            {
                                folder.NoteFolderName = folderName;
                                folder.ModificationDate = DateTime.Now;
                                folder.NameCount = folder.NoteFolderName + " (" + folder.NoteCount.ToString() + ")";
                            }
                            context.SubmitChanges();
                            //lstFolders.ItemsSource = context.NoteFolders;
                            MessageBox.Show(AppResources.SuccessfulChangeFolderName);
                            NoteFolder note2 = context.NoteFolders.Where(j => j.NoteFolderName.Equals(folderName)).Single() as NoteFolder;
                            NavigationService.Navigate(new Uri("/NoteFolder.xaml#" + note2.NoteFolderId, UriKind.Relative));
                        }
                    }
                }
            };
            control.btnCancel.Click += (s, args) =>
            {
                popup.IsOpen = false;
            };
        }

        private void DeleteFolderButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppResources.DeleteFolderQuestion,
                AppResources.DeleteFolder, MessageBoxButton.OKCancel)
                != MessageBoxResult.OK)
            {

            }
            else
            {
                using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                {
                    var note = context.Notes.Where(j => j.NoteFolderId.Equals(noteId)).ToList() as List<Note>;
                    context.Notes.DeleteAllOnSubmit(note);

                    var noteFolder = context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId)).Single() as NoteFolder;
                    context.NoteFolders.DeleteOnSubmit(noteFolder);

                    context.SubmitChanges();
                }
                MessageBox.Show(AppResources.SuccessfulDeleteFolder);
                NavigationService.Navigate(new Uri("/MainPage.xaml" , UriKind.Relative));
            }
            //MessageBox.Show(AppResources.NoteSaved);
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