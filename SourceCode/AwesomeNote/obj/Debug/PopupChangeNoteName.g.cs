﻿#pragma checksum "D:\KodServer\VisualStudioProjeleri\AwesomeNote\DbDeneme\PopupChangeNoteName.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "542E798F468DD8F8A892F7464CDB88D7"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
    
    
    public partial class PopupChangeNoteName : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.StackPanel LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtLabel;
        
        internal System.Windows.Controls.TextBox txtNoteName;
        
        internal System.Windows.Controls.Button btnOK;
        
        internal System.Windows.Controls.Button btnCancel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Awesome%20Note;component/PopupChangeNoteName.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.StackPanel)(this.FindName("LayoutRoot")));
            this.txtLabel = ((System.Windows.Controls.TextBox)(this.FindName("txtLabel")));
            this.txtNoteName = ((System.Windows.Controls.TextBox)(this.FindName("txtNoteName")));
            this.btnOK = ((System.Windows.Controls.Button)(this.FindName("btnOK")));
            this.btnCancel = ((System.Windows.Controls.Button)(this.FindName("btnCancel")));
        }
    }
}

