﻿#pragma checksum "D:\KodServer\VisualStudioProjeleri\AwesomeNote\DbDeneme\SearchPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2692BB7AFDDFF9555545725F00BA8FCE"
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
    
    
    public partial class SearchPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock lblSearch;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBox txtSearch;
        
        internal System.Windows.Controls.TextBox txtSearchResult;
        
        internal System.Windows.Controls.ListBox lstSearch;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Awesome%20Note;component/SearchPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.lblSearch = ((System.Windows.Controls.TextBlock)(this.FindName("lblSearch")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.txtSearch = ((System.Windows.Controls.TextBox)(this.FindName("txtSearch")));
            this.txtSearchResult = ((System.Windows.Controls.TextBox)(this.FindName("txtSearchResult")));
            this.lstSearch = ((System.Windows.Controls.ListBox)(this.FindName("lstSearch")));
        }
    }
}

