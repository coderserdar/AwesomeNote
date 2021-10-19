using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
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
    public partial class LanguageSettings : PhoneApplicationPage
    {
        public SolidColorBrush messageBackGround;
        public LanguageSettings()
        {
            InitializeComponent();
            SetBackgroundColor();

            lstLanguage.Items.Clear();
            lstLanguage.Items.Add(AppResources.English);
            lstLanguage.Items.Add(AppResources.Turkish);
            lstLanguage.Items.Add(AppResources.German);
            lstLanguage.Items.Add(AppResources.Spanish);
            lstLanguage.Items.Add(AppResources.Russian);
            lstLanguage.Items.Add(AppResources.Chinese);
            lstLanguage.Items.Add(AppResources.Arabic);
            lstLanguage.Items.Add(AppResources.Persian);
            lstLanguage.Items.Add(AppResources.Italian);
            lstLanguage.Items.Add(AppResources.French);
            lstLanguage.Items.Add(AppResources.Portuguese);
            lstLanguage.Items.Add(AppResources.Sanskrit);
            lstLanguage.Items.Add(AppResources.Thai);
            lstLanguage.SelectedIndex = -1;
            lblLanguage.Text = AppResources.SelectLanguage;
        }

        private void lstLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = lstLanguage.SelectedIndex;
            string culture = "";
            string lang = "";
            if (index == 0)
            {
                culture = "en";
                lang = "EN";
            }
            else if (index == 1)
            {
                culture = "tr";
                lang = "TR";
            }
            else if (index == 2)
            {
                culture = "de";
                lang = "DE";
            }
            else if (index == 3)
            {
                culture = "es";
                lang = "ES";
            }
            else if (index == 4)
            {
                culture = "ru";
                lang = "RU";
            }
            else if (index == 5)
            {
                culture = "zh";
                lang = "AR";
            }
            else if (index == 6)
            {
                culture = "ar";
                lang = "AR";
            }
            else if (index == 7)
            {
                culture = "fa-IR";
                lang = "FA";
            }
            else if (index == 8)
            {
                culture = "it";
                lang = "IT";
            }
            else if (index == 9)
            {
                culture = "fr";
                lang = "FR";
            }
            else if (index == 10)
            {
                culture = "pt";
                lang = "PT";
            }
            else if (index == 11)
            {
                culture = "sa";
                lang = "SA";
            }
            else if (index == 12)
            {
                culture = "th";
                lang = "TH";
            }
            else
            {
                culture = "en";
                lang = "EN";
            }

            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                var appSettings = context.AppSettings;
                foreach (var appSetting in appSettings)
                {
                    appSetting.AppLangName = lang;
                }
                context.SubmitChanges();
            }

            CultureInfo newCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;
            MessageBox.Show(AppResources.LanguageWarning);
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