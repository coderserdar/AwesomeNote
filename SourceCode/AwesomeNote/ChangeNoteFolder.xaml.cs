using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using AwesomeNote.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AwesomeNote
{
    public partial class ChangeNoteFolder : PhoneApplicationPage
    {
        public int noteId;
        public int noteFolderId;
        public SolidColorBrush messageBackGround;

        public ChangeNoteFolder()
        {
            InitializeComponent();
            SetBackgroundColor();
        }

        private void lstNoteFolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NoteFolder temp = lstNoteFolders.SelectedItem as NoteFolder;

            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {

                var noteFolders1 = context.NoteFolders.Where(j => j.NoteFolderId.Equals(temp.NoteFolderId)).Select(j => j);
                var noteFolders2 = context.NoteFolders.Where(j => j.NoteFolderId.Equals(noteFolderId)).Select(j => j);
                var notes = context.Notes.Where(j => j.NoteId.Equals(noteId)).Select(j => j);
                foreach (var note in notes)
                {
                    note.NoteFolderId = temp.NoteFolderId;
                }

                foreach (var noteFolder1 in noteFolders1)
                {
                    noteFolder1.NoteCount = noteFolder1.NoteCount + 1;
                    noteFolder1.NameCount = noteFolder1.NoteFolderName + " (" + noteFolder1.NoteCount.ToString() + ")";
                }

                foreach (var noteFolder2 in noteFolders2)
                {
                    noteFolder2.NoteCount = noteFolder2.NoteCount - 1;
                    noteFolder2.NameCount = noteFolder2.NoteFolderName + " (" + noteFolder2.NoteCount.ToString() + ")";
                }
                context.SubmitChanges();
                //lstFolders.ItemsSource = context.NoteFolders;
                MessageBox.Show(AppResources.SuccessfulChangeNoteFolder);
            }
            this.NavigationService.Navigate(new Uri("/NoteDetail.xaml#" + noteId, UriKind.Relative));
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
            noteId = int.Parse(e.Fragment);
            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                lstNoteFolders.Items.Clear();
                var note = context.Notes.Where(j => j.NoteId.Equals(noteId)).Single() as Note;
                noteFolderId = note.NoteFolderId;
                lblNoteName.Text = note.NoteName;
                lblChangeFolder.Text = AppResources.SelectNoteFolder;
                var noteFolders = context.NoteFolders;
                lstNoteFolders.ItemsSource = noteFolders;
                lstNoteFolders.DisplayMemberPath = "NoteFolderName";
            }
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new Uri("/NoteDetail.xaml#" + noteId, UriKind.Relative));
            }
        }

        private void SetBackgroundColor()
        {
            AppSettings appSettings = new AppSettings();
            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                appSettings = context.AppSettings.First() as AppSettings;
            }

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
}