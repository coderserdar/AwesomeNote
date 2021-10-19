using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Phone.Tasks;
using AwesomeNote.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AwesomeNote
{
    public partial class NoteDetail : PhoneApplicationPage
    {

        double InputHeight = 0.0;
        Popup popup;
        public SolidColorBrush messageBackGround;
        public bool flag;

        public NoteDetail()
        {
            InitializeComponent();
            //SetBackgroundColor();

            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton button1 = new ApplicationBarIconButton();
            button1.IconUri = new Uri("/Assets/PreviousNote.png", UriKind.Relative);
            button1.Text = AppResources.PreviousNote;
            ApplicationBar.Buttons.Add(button1);
            button1.Click += new EventHandler(PreviousNoteButton_Click);

            ApplicationBarIconButton button2 = new ApplicationBarIconButton();
            button2.IconUri = new Uri("/Assets/SendWithMail.png", UriKind.Relative);
            button2.Text = AppResources.SendWithMail;
            ApplicationBar.Buttons.Add(button2);
            button2.Click += new EventHandler(SendMailButton_Click);

            ApplicationBarIconButton button3 = new ApplicationBarIconButton();
            button3.IconUri = new Uri("/Assets/SendWithSMS.png", UriKind.Relative);
            button3.Text = AppResources.SendWithSMS;
            ApplicationBar.Buttons.Add(button3);
            button3.Click += new EventHandler(SendSMSButton_Click);

            ApplicationBarIconButton button4 = new ApplicationBarIconButton();
            button4.IconUri = new Uri("/Assets/NextNote.png", UriKind.Relative);
            button4.Text = AppResources.NextNote;
            ApplicationBar.Buttons.Add(button4);
            button4.Click += new EventHandler(NextNoteButton_Click);

            ApplicationBarMenuItem menuItem1 = new ApplicationBarMenuItem();
            menuItem1.Text = AppResources.SelectAll;
            ApplicationBar.MenuItems.Add(menuItem1);
            menuItem1.Click += new EventHandler(SelectAllMenuItem_Click);

            ApplicationBarMenuItem menuItem2 = new ApplicationBarMenuItem();
            menuItem2.Text = AppResources.Share;
            ApplicationBar.MenuItems.Add(menuItem2);
            menuItem2.Click += new EventHandler(ShareMenuItem_Click);

            ApplicationBarMenuItem menuItem3 = new ApplicationBarMenuItem();
            menuItem3.Text = AppResources.ChangeFolder;
            ApplicationBar.MenuItems.Add(menuItem3);
            menuItem3.Click += new EventHandler(ChangeFolderMenuItem_Click);

            ApplicationBarMenuItem menuItem4 = new ApplicationBarMenuItem();
            menuItem4.Text = AppResources.DeleteNote;
            ApplicationBar.MenuItems.Add(menuItem4);
            menuItem4.Click += new EventHandler(DeleteMenuItem_Click);

            ApplicationBarMenuItem menuItem5 = new ApplicationBarMenuItem();
            menuItem5.Text = AppResources.NoteInfo;
            ApplicationBar.MenuItems.Add(menuItem5);
            menuItem5.Click += new EventHandler(NoteInfoMenuItem_Click);

            

            popup = new Popup();
        }

        public int noteFolderId;
        public int noteId;
        public string pageName;

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            ////txtNote.Focus();
            ////txtNote.Select(txtNote.Text.Length, 1);
            ////determine if HD device
            //var deviceWidth = this.ActualWidth;
            //var isHdDevice = (deviceWidth &gt; 500 ? true : false);
 
            ////the keyboard height differs between HD devices and regular ones
            //if (isHdDevice)
            //    keyboardHeight = 540;
            //else
            //    keyboardHeight = 336;
 
            ////make the keyboard placeholder's height as high as 
            ////the anticipted keyboard height
            ////this will be used to offset other controls on the page into the viewable area
            //pnlKeyboardPlaceHolder.Height = keyboardHeight;
        }
        protected override void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            // displays "Fragment: Detail"
            //MessageBox.Show("Note Id: " + noteFolder.NoteFolderId);
            base.OnFragmentNavigation(e);
            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                var note = context.Notes.Where(j => j.NoteId.Equals(e.Fragment)).Single() as Note;
                lblNoteDetail.Text = note.NoteName;
                lblCreationDate.Text = AppResources.CreationDate + ": " + note.CreationDate.ToString();
                noteId = note.NoteId;
                var noteFolder = context.NoteFolders.Where(j => j.NoteFolderId.Equals(note.NoteFolderId)).Single() as NoteFolder;

                if (pageName == "/SearchPage.xaml")
                {
                    lblNoteDetailApp.Text = AppResources.SearchResults;    
                }
                else
                {
                    lblNoteDetailApp.Text = noteFolder.NoteFolderName;    
                }
                noteFolderId = noteFolder.NoteFolderId;
                FontFamily temp = new FontFamily(noteFolder.FontFamily);
                txtNote.FontFamily = temp;
                txtNote.FontSize = double.Parse(noteFolder.FontSize);
                txtNote.Text = note.NoteDescription;
                //lstNoteList.DisplayMemberPath = "NoteName";

                SetBackgroundColor();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var lastPage = NavigationService.BackStack.FirstOrDefault();
            pageName = lastPage.Source.ToString();

            //txtNote.Focus();
            //txtNote.Select(txtNote.Text.Length, 0);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (popup.IsOpen)
            {
                popup.IsOpen = false;
            }

            if (this.NavigationService.CanGoBack)
            {
                SaveMenuItem_Click(this, new EventArgs());
                if (pageName == "/SearchPage.xaml")
                {
                    //this.NavigationService.Navigate(new Uri("/SearchPage.xaml" , UriKind.Relative));
                }
                else
                {
                    this.NavigationService.Navigate(new Uri("/NoteFolder.xaml#" + noteFolderId, UriKind.Relative));    
                }
            }
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void SendSMSButton_Click(object sender, EventArgs e)
        {
            SmsComposeTask smsComposeTask = new SmsComposeTask();

            smsComposeTask.To = "";
            smsComposeTask.Body = lblNoteDetail.Text + ": " + txtNote.Text;

            smsComposeTask.Show();
            //MessageBox.Show(AppResources.SuccessfulSendWithSMS);
        }

        private void NextNoteButton_Click(object sender, EventArgs e)
        {
            SaveMenuItem_Click(this, new EventArgs());

            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                int gidecekSayfa = 0;
                var note = context.Notes.Where(j => j.NoteId.Equals(noteId)).Single() as Note;
                var noteFolder = context.NoteFolders.Where(j => j.NoteFolderId.Equals(note.NoteFolderId)).Single() as NoteFolder;
                string orderStyle = noteFolder.NoteOrderStyle;
                List<Note> noteList = new List<Note>();
                switch (noteFolder.NoteOrderBy)
                {
                    case "NAME":
                        if (orderStyle == "A")
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(noteFolder.NoteFolderId)).OrderBy(j => j.NoteName).ToList() as List<Note>;
                        }
                        else
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(noteFolder.NoteFolderId)).OrderByDescending(j => j.NoteName).ToList() as List<Note>;
                        }
                        break;
                    case "CDATE":
                        if (orderStyle == "A")
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(noteFolder.NoteFolderId)).OrderBy(j => j.CreationDate).ToList() as List<Note>;
                        }
                        else
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(noteFolder.NoteFolderId)).OrderByDescending(j => j.CreationDate).ToList() as List<Note>;
                        }
                        break;
                    case "MDATE":
                        if (orderStyle == "A")
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(noteFolder.NoteFolderId)).OrderBy(j => j.ModificationDate).ToList() as List<Note>;
                        }
                        else
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(noteFolder.NoteFolderId)).OrderByDescending(j => j.ModificationDate).ToList() as List<Note>;
                        }
                        break;
                    default:
                        if (orderStyle == "A")
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(noteFolder.NoteFolderId)).OrderBy(j => j.NoteName).ToList() as List<Note>;
                        }
                        else
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(noteFolder.NoteFolderId)).OrderByDescending(j => j.NoteName).ToList() as List<Note>;
                        }
                        break;
                }

                if (note != noteList.Last())
                {
                    for (int i = 0; i < noteList.Count; i++)
                    {
                        if (note == noteList[i])
                        {
                            gidecekSayfa = i + 1;
                            break;
                        }
                    }
                    lblNoteDetail.Text = noteList[gidecekSayfa].NoteName;
                    txtNote.Text = noteList[gidecekSayfa].NoteDescription;
                    noteId = noteList[gidecekSayfa].NoteId;
                    //txtNote.Focus();
                    //txtNote.Select(txtNote.Text.Length,1);
                }
                else
                {

                }

            }
            //MessageBox.Show(AppResources.SuccessfulSendWithSMS);
        }

        private void PreviousNoteButton_Click(object sender, EventArgs e)
        {
            SaveMenuItem_Click(this, new EventArgs());

            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                var note = context.Notes.Where(j => j.NoteId.Equals(noteId)).Single() as Note;
                var noteFolder = context.NoteFolders.Where(j => j.NoteFolderId.Equals(note.NoteFolderId)).Single() as NoteFolder;
                string orderStyle = noteFolder.NoteOrderStyle;
                int gidecekSayfa = 0;
                List<Note> noteList = new List<Note>();
                switch (noteFolder.NoteOrderBy)
                {
                    case "NAME":
                        if (orderStyle == "A")
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(noteFolder.NoteFolderId)).OrderBy(j => j.NoteName).ToList() as List<Note>;
                        }
                        else
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(noteFolder.NoteFolderId)).OrderByDescending(j => j.NoteName).ToList() as List<Note>;
                        }
                        break;
                    case "CDATE":
                        if (orderStyle == "A")
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(noteFolder.NoteFolderId)).OrderBy(j => j.CreationDate).ToList() as List<Note>;
                        }
                        else
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(noteFolder.NoteFolderId)).OrderByDescending(j => j.CreationDate).ToList() as List<Note>;
                        }
                        break;
                    case "MDATE":
                        if (orderStyle == "A")
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(noteFolder.NoteFolderId)).OrderBy(j => j.ModificationDate).ToList() as List<Note>;
                        }
                        else
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(noteFolder.NoteFolderId)).OrderByDescending(j => j.ModificationDate).ToList() as List<Note>;
                        }
                        break;
                    default:
                        if (orderStyle == "A")
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(noteFolder.NoteFolderId)).OrderBy(j => j.NoteName).ToList() as List<Note>;
                        }
                        else
                        {
                            noteList = context.Notes.Where(j => j.NoteFolderId.Equals(noteFolder.NoteFolderId)).OrderByDescending(j => j.NoteName).ToList() as List<Note>;
                        }
                        break;
                }

                if (note != noteList.First())
                {
                    for (int i = 0; i < noteList.Count; i++)
                    {
                        if (note == noteList[i])
                        {
                            gidecekSayfa = i - 1;
                            break;
                        }
                    }
                    lblNoteDetail.Text = noteList[gidecekSayfa].NoteName;
                    txtNote.Text = noteList[gidecekSayfa].NoteDescription;
                    noteId = noteList[gidecekSayfa].NoteId;
                    //txtNote.Focus();
                    //txtNote.Select(txtNote.Text.Length, 1);
                }
                else
                {

                }

            }
            //MessageBox.Show(AppResources.SuccessfulSendWithSMS);
        }

        private void ShareMenuItem_Click(object sender, EventArgs e)
        {
            ShareStatusTask shareStatusTask = new ShareStatusTask();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(lblNoteDetail.Text);
            sb.AppendLine();
            sb.AppendLine(txtNote.Text);
            sb.AppendLine();
            sb.AppendLine(AppResources.SendWithApp);

            shareStatusTask.Status = sb.ToString();

            shareStatusTask.Show();
        }

        private void SendMailButton_Click(object sender, EventArgs e)
        {
            // burada birden fazla e-posta hesabı varsa birini seçmesi söyleniyor
            //EmailAddressChooserTask emailAddressChooserTask;
            //emailAddressChooserTask = new EmailAddressChooserTask();
            //emailAddressChooserTask.Completed += new EventHandler<EmailResult>(emailAddressChooserTask_Completed);
            //emailAddressChooserTask.Show();
            StringBuilder sb = new StringBuilder();
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            sb.AppendLine(txtNote.Text);
            sb.AppendLine();
            sb.AppendLine(AppResources.SendWithApp);

            emailComposeTask.Subject = lblNoteDetail.Text;
            emailComposeTask.Body = sb.ToString();
            emailComposeTask.To = "";
            emailComposeTask.Cc = "";
            emailComposeTask.Bcc = "";

            emailComposeTask.Show();
            //MessageBox.Show(AppResources.SuccessfulSendWithMail);
        }

        // burada da seçilen e_posta adresi üzerinden gönderim yapılacak
        void emailAddressChooserTask_Completed(object sender, EmailResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                //MessageBox.Show("The email for " + e.DisplayName + " is " + e.Email);

                //Code to send a new email message using the retrieved email address.
                EmailComposeTask emailComposeTask = new EmailComposeTask();
                emailComposeTask.To = e.Email;
                emailComposeTask.Subject = e.DisplayName + ", here is an email from my app!";
                emailComposeTask.Show();
            }
        }

        private void SelectAllMenuItem_Click(object sender, EventArgs e)
        {
            flag = true;
            txtNote.Focus();
            txtNote.SelectAll();
        }

        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                var notes = context.Notes.Where(j => j.NoteId.Equals(noteId)).Select(j => j);
                foreach (var note in notes)
                {
                    note.NoteDescription = txtNote.Text;
                    note.ModificationDate = DateTime.Now;
                    note.NameDescription = note.NoteName + " " + note.NoteDescription;
                    //note.NameDescriptionWithoutNewline = note.NameDescription.Replace(Environment.NewLine," ");
                }
                context.SubmitChanges();
            }
            //MessageBox.Show(AppResources.NoteSaved);
        }


        // notu silmek için kullanılan bir metod bu
        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppResources.DeleteNoteQuestion,
                AppResources.DeleteNote, MessageBoxButton.OKCancel)
                != MessageBoxResult.OK)
            {

            }
            else
            {
                using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                {
                    var note = context.Notes.Where(j => j.NoteId.Equals(noteId)).Single() as Note;
                    context.Notes.DeleteOnSubmit(note);
                    context.SubmitChanges();
                }
                MessageBox.Show(AppResources.SuccessfulDeleteNote);
                NavigationService.Navigate(new Uri("/NoteFolder.xaml#" + noteFolderId, UriKind.Relative));
            }
            //MessageBox.Show(AppResources.NoteSaved);
        }

        private void NoteInfoMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/NoteInfo.xaml#" + noteId, UriKind.Relative));
            //MessageBox.Show(AppResources.NoteSaved);
        }

        private void ChangeFolderMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ChangeNoteFolder.xaml#" + noteId, UriKind.Relative));
        }

        // notun adını değiştirmek için kullanılan bir metod
        private void lblNoteDetail_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            popup = new Popup();
            popup.Height = 300;
            popup.Width = 400;
            popup.VerticalOffset = 20;
            PopupChangeNoteName control = new PopupChangeNoteName();
            control.txtLabel.Text = AppResources.EnterNoteName;
            control.btnCancel.Content = AppResources.Cancel;
            control.btnOK.Content = AppResources.OK;
            popup.Child = control;
            popup.IsOpen = true;
            control.txtNoteName.Text = lblNoteDetail.Text;
            control.txtNoteName.Focus();
            control.txtNoteName.Select(0,control.txtNoteName.Text.Length);

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

                if (noteName != lblNoteDetail.Text)
                {
                    // aynı isimde bir notun daha önceden oluşturulup oluşturulmadığını
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
                    // eğer bu isimde bir not oluşturulmamışsa
                    // oluşturulması için gerekli kodlar aşağıdadır
                    else
                    {
                        using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
                        {
                            var notes = context.Notes.Where(j => j.NoteId.Equals(noteId)).Select(j => j);
                            foreach (var note in notes)
                            {
                                note.NoteName = noteName;
                                note.ModificationDate = DateTime.Now;
                                note.NameCreation = note.NoteName + " (" + note.CreationDate.ToString() + ")";
                                note.NameDescription = note.NoteName + " " + note.NoteDescription;
                                //note.NameDescriptionWithoutNewline = note.NameDescription.Replace(Environment.NewLine," ");
                            }
                            context.SubmitChanges();
                            //lstFolders.ItemsSource = context.NoteFolders;
                            MessageBox.Show(AppResources.SuccessfulChangeNoteName);
                            Note note2 = context.Notes.Where(j => j.NoteName.Equals(noteName)).Single() as Note;
                            NavigationService.Navigate(new Uri("/NoteDetail.xaml#" + note2.NoteId, UriKind.Relative));
                        }
                    }
                }
            };
            control.btnCancel.Click += (s, args) =>
            {
                popup.IsOpen = false;
            };

        }

        private void txtNote_TextInputStart(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            this.svNote.UpdateLayout();
            this.svNote.ScrollToVerticalOffset(this.txtNote.ActualHeight);
            this.svNote.UpdateLayout();
        }

        private void svNote_GotFocus(object sender, RoutedEventArgs e)
        {
            //this.svNote.Height = 590;
            this.svNote.ScrollToVerticalOffset(this.txtNote.ActualHeight);
            this.svNote.UpdateLayout();
            //App.RootFrame.RenderTransform = new CompositeTransform();
        }

        
        private void svNote_LostFocus(object sender, RoutedEventArgs e)
        {
            //this.svNote.Height = 600;
        }

        private void txtNote_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            txtNote.Focus();
            //txtNote.Select(txtNote.Text.Length, 1);
            svNote.ScrollToVerticalOffset(e.GetPosition(txtNote).Y - 80);
        }

        private void txtNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                double CurrentInputHeight = txtNote.ActualHeight;

                if (CurrentInputHeight > InputHeight)
                {
                    svNote.ScrollToVerticalOffset(svNote.VerticalOffset + CurrentInputHeight - InputHeight);
                }

                InputHeight = CurrentInputHeight;
            });
        }

        private void txtNote_GotFocus(object sender, RoutedEventArgs e)
        {
            
            //reset any page movement cause by keyboard opening
            App.RootFrame.RenderTransform = new CompositeTransform();
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

        private void txtNote_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                svNote.ScrollToVerticalOffset(txtNote.ActualHeight);
            }
        }

        private void txtNote_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!flag) return;
            txtNote.Focus();
            flag = false;
            this.pnlKeyboardPlaceHolder.Visibility = Visibility.Collapsed;
        }

    }
}