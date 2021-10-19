using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using AwesomeNote.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AwesomeNote
{
    public partial class Page1 : PhoneApplicationPage
    {
        public SolidColorBrush messageBackGround;
        public Page1()
        {
            InitializeComponent();
            //SetBackgroundColor();
        }

        public int noteId;
        public int noteFolderId;
        protected override void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            // displays "Fragment: Detail"
            //MessageBox.Show("Note Id: " + e.Fragment);
            base.OnFragmentNavigation(e);
            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                var note = context.Notes.Where(j => j.NoteId.Equals(e.Fragment)).Single() as Note;
                var noteFolder =
                    context.NoteFolders.Where(j => j.NoteFolderId.Equals(note.NoteFolderId)).Single() as NoteFolder;
                lblNoteName.Text = note.NoteName;
                noteId = note.NoteId;
                noteFolderId = note.NoteFolderId;
                lblNoteInfo.Text = AppResources.About;
                StringBuilder sb = new StringBuilder();
                string[] wordNumbers;
                wordNumbers = note.NoteDescription.Split(' ');

                // burada not dosyası hakkında istatistiki bilgiler hazırlanıyor
                sb.AppendLine(AppResources.BelongFolderName + ": " + noteFolder.NoteFolderName);
                sb.AppendLine(AppResources.CharacterNumber + ": " + note.NoteDescription.Length);
                sb.AppendLine(AppResources.WordNumber + ": " + (wordNumbers.Length-1));
                sb.AppendLine(AppResources.CreationDate + ": " + note.CreationDate);
                sb.AppendLine(AppResources.ModificationDate + ": " + note.ModificationDate);

                //var paragraph = new Paragraph();
                //paragraph.Inlines.Add(sb.ToString());
                //txtNoteInfo.Blocks.Add(paragraph);
                txtNoteInfo.Text = sb.ToString();
                //lstNoteList.DisplayMemberPath = "NoteName";
                SetBackgroundColor();
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
                this.NavigationService.Navigate(new Uri("/NoteDetail.xaml#" + noteId, UriKind.Relative));
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