﻿#pragma checksum "D:\KodServer\VisualStudioProjeleri\AwesomeNote\DbDeneme\EnterPasswordPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B6EB82DA1DFC5352A5ECF03590FFA250"
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
    
    
    public partial class EnterPasswordPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock lblFolderName;
        
        internal System.Windows.Controls.TextBlock lblEnterPassword;
        
        internal System.Windows.Controls.ScrollViewer svPassword;
        
        internal System.Windows.Controls.StackPanel pnlDeneme;
        
        internal System.Windows.Controls.Image imgPassword;
        
        internal System.Windows.Controls.TextBox txtLabel;
        
        internal System.Windows.Controls.PasswordBox txtPassword;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Awesome%20Note;component/EnterPasswordPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.lblFolderName = ((System.Windows.Controls.TextBlock)(this.FindName("lblFolderName")));
            this.lblEnterPassword = ((System.Windows.Controls.TextBlock)(this.FindName("lblEnterPassword")));
            this.svPassword = ((System.Windows.Controls.ScrollViewer)(this.FindName("svPassword")));
            this.pnlDeneme = ((System.Windows.Controls.StackPanel)(this.FindName("pnlDeneme")));
            this.imgPassword = ((System.Windows.Controls.Image)(this.FindName("imgPassword")));
            this.txtLabel = ((System.Windows.Controls.TextBox)(this.FindName("txtLabel")));
            this.txtPassword = ((System.Windows.Controls.PasswordBox)(this.FindName("txtPassword")));
        }
    }
}

