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
    public partial class BackgroundColorSettings : PhoneApplicationPage
    {
        public SolidColorBrush messageBackGround;
        public BackgroundColorSettings()
        {
            InitializeComponent();

            lstBackgroundColor.Items.Clear();
            lstBackgroundColor.Items.Add(AppResources.Black);
            lstBackgroundColor.Items.Add(AppResources.Blue);
            lstBackgroundColor.Items.Add(AppResources.Brown);
            lstBackgroundColor.Items.Add(AppResources.Gray);
            lstBackgroundColor.Items.Add(AppResources.Green);
            lstBackgroundColor.Items.Add(AppResources.Orange);
            lstBackgroundColor.Items.Add(AppResources.Purple);
            lstBackgroundColor.Items.Add(AppResources.Red);
            lstBackgroundColor.Items.Add(AppResources.Yellow);
            lstBackgroundColor.SelectedIndex = -1;

            lblBackgroundColor.Text = AppResources.SelectBackgroundColor;

        }

        private void lstBackgroundColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = lstBackgroundColor.SelectedIndex;
            string backgroundColor = "";
            if (index == 0)
            {
                backgroundColor = "BLA";
            }
            else if (index == 1)
            {
                backgroundColor = "BLU";
            }
            else if (index == 2)
            {
                backgroundColor = "BRO";
            }
            else if (index == 3)
            {
                backgroundColor = "GRA";
            }
            else if (index == 4)
            {
                backgroundColor = "GRE";
            }
            else if (index == 5)
            {
                backgroundColor = "ORA";
            }
            else if (index == 6)
            {
                backgroundColor = "PUR";
            }
            else if (index == 7)
            {
                backgroundColor = "RED";
            }
            else if (index == 8)
            {
                backgroundColor = "YEL";
            }
            else
            {
                backgroundColor = "BLA";
            }

            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                var appSettings = context.AppSettings;
                foreach (var appSetting in appSettings)
                {
                    appSetting.AppBackgroundColor = backgroundColor;
                }
                context.SubmitChanges();
                //CustomMessageBox messageBox = new CustomMessageBox()
                //{
                //    Caption = AppResources.BackgroundColor,
                //    Message = AppResources.SuccessfulBackgroundColorChanged,
                //    Background = messageBackGround
                //};
                //messageBox.Show();
                MessageBox.Show(AppResources.SuccessfulBackgroundColorChanged);
            }
            SetBackgroundColor();
            NavigationService.Navigate(new Uri("/GeneralSettings.xaml", UriKind.Relative));
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