using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AwesomeNote
{
    public partial class PopupAddNote : UserControl
    {
        public PopupAddNote()
        {
            InitializeComponent();
            SetPopupBackgroundColor();
        }

        private void SetPopupBackgroundColor()
        {
            AppSettings appSettings = new AppSettings();
            using (var context = new NoteFolderDataContext(NoteFolderDataContext.ConnectionString))
            {
                appSettings = context.AppSettings.First() as AppSettings;
            }

            switch (appSettings.AppBackgroundColor)
            {
                case "BLA":
                    this.LayoutRoot.Background = new SolidColorBrush(Colors.Black);
                    break;
                case "BLU":
                    this.LayoutRoot.Background = new SolidColorBrush(Colors.Blue);
                    break;
                case "BRO":
                    this.LayoutRoot.Background = new SolidColorBrush(Colors.Brown);
                    break;
                case "RED":
                    this.LayoutRoot.Background = new SolidColorBrush(Colors.Red);
                    break;
                case "GRE":
                    this.LayoutRoot.Background = new SolidColorBrush(Colors.Green);
                    break;
                case "GRA":
                    this.LayoutRoot.Background = new SolidColorBrush(Colors.Gray);
                    break;
                case "YEL":
                    this.LayoutRoot.Background = new SolidColorBrush(Colors.Yellow);
                    break;
                case "ORA":
                    this.LayoutRoot.Background = new SolidColorBrush(Colors.Orange);
                    break;
                case "PUR":
                    this.LayoutRoot.Background = new SolidColorBrush(Colors.Purple);
                    break;
                default:
                    this.LayoutRoot.Background = new SolidColorBrush(Colors.Black);
                    break;
            }
        }
    }
}
