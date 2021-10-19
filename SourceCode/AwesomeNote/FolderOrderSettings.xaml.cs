using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using AwesomeNote.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AwesomeNote
{
    public partial class FolderOrderSettings : PhoneApplicationPage
    {
        public SolidColorBrush messageBackGround;
        public FolderOrderSettings()
        {
            InitializeComponent();
            SetBackgroundColor();

            lstFolderOrder.Items.Clear();
            lstFolderOrder.Items.Add(AppResources.FolderName);
            lstFolderOrder.Items.Add(AppResources.CreationDate);
            lstFolderOrder.Items.Add(AppResources.ModificationDate);
            lstFolderOrder.SelectedIndex = -1;

            lblFolderOrder.Text = AppResources.SelectOrderType;
        }

        private void lstFolderOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = lstFolderOrder.SelectedIndex;
            string orderType = "";
            if (index == 0)
            {
                orderType = "NAME";
            }
            else if (index == 1)
            {
                orderType = "CDATE";
            }
            else if (index == 2)
            {
                orderType = "MDATE";
            }
            else
            {
                orderType = "NAME";
            }

            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                var appSettings = context.AppSettings;
                foreach (var appSetting in appSettings)
                {
                    appSetting.FolderOrderBy = orderType;
                }
                context.SubmitChanges();
                //CustomMessageBox messageBox = new CustomMessageBox()
                //{
                //    Caption = AppResources.NoteOrder,
                //    Message = AppResources.SuccessfulOrderTypeChanged,
                //    Background = messageBackGround,
                //    LeftButtonContent = AppResources.OK
                //};
                //messageBox.Dismissed += (s1, e1) =>
                //{
                //    switch (e1.Result)
                //    {
                //        case CustomMessageBoxResult.LeftButton:
                //            // Do something.
                //            break;
                //        case CustomMessageBoxResult.RightButton:
                //            // Do something.
                //            break;
                //        case CustomMessageBoxResult.None:
                //            // Do something.
                //            break;
                //        default:
                //            break;
                //    }
                //};
                //messageBox.Show();
                MessageBox.Show(AppResources.SuccessfulOrderTypeChanged);
            }
            NavigationService.Navigate(new Uri("/GeneralSettings.xaml", UriKind.Relative));
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
                this.NavigationService.Navigate(new Uri("/GeneralSettings.xaml", UriKind.Relative));
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
    }
}