#pragma checksum "D:\KodServer\VisualStudioProjeleri\AwesomeNote\DbDeneme\SetPasswordPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BCF4BAB56972D46460BF224CCA7034BE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace AwesomeNote {
    
    
    public partial class SetPasswordPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock lblFolderName;
        
        internal System.Windows.Controls.TextBlock lblSetPassword;
        
        internal System.Windows.Controls.ScrollViewer svPassword;
        
        internal System.Windows.Controls.StackPanel pnlDeneme;
        
        internal System.Windows.Controls.TextBox txtPass1;
        
        internal System.Windows.Controls.PasswordBox txtFirstPassword;
        
        internal System.Windows.Controls.TextBox txtPass2;
        
        internal System.Windows.Controls.PasswordBox txtSecondPassword;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Awesome%20Note;component/SetPasswordPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.lblFolderName = ((System.Windows.Controls.TextBlock)(this.FindName("lblFolderName")));
            this.lblSetPassword = ((System.Windows.Controls.TextBlock)(this.FindName("lblSetPassword")));
            this.svPassword = ((System.Windows.Controls.ScrollViewer)(this.FindName("svPassword")));
            this.pnlDeneme = ((System.Windows.Controls.StackPanel)(this.FindName("pnlDeneme")));
            this.txtPass1 = ((System.Windows.Controls.TextBox)(this.FindName("txtPass1")));
            this.txtFirstPassword = ((System.Windows.Controls.PasswordBox)(this.FindName("txtFirstPassword")));
            this.txtPass2 = ((System.Windows.Controls.TextBox)(this.FindName("txtPass2")));
            this.txtSecondPassword = ((System.Windows.Controls.PasswordBox)(this.FindName("txtSecondPassword")));
        }
    }
}

