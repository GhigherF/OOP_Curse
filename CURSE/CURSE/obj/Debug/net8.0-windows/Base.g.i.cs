﻿#pragma checksum "..\..\..\Base.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "74D3E0431E0793984E39B0691C93BAEB25CDC435"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using CURSE;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace CURSE {
    
    
    /// <summary>
    /// Base
    /// </summary>
    public partial class Base : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 30 "..\..\..\Base.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer MainScrollViewer;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\Base.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas Note;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\Base.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border MovableBorder;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\Base.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RichTextBox MovableTextBox;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.3.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CURSE;component/base.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Base.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.3.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 22 "..\..\..\Base.xaml"
            ((System.Windows.Controls.Border)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Drag);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 23 "..\..\..\Base.xaml"
            ((System.Windows.Controls.Border)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Drag);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 26 "..\..\..\Base.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this._);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 27 "..\..\..\Base.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.X);
            
            #line default
            #line hidden
            return;
            case 5:
            this.MainScrollViewer = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 6:
            this.Note = ((System.Windows.Controls.Canvas)(target));
            return;
            case 7:
            this.MovableBorder = ((System.Windows.Controls.Border)(target));
            
            #line 34 "..\..\..\Base.xaml"
            this.MovableBorder.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.LeftClick);
            
            #line default
            #line hidden
            
            #line 34 "..\..\..\Base.xaml"
            this.MovableBorder.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.LeftUnclick);
            
            #line default
            #line hidden
            
            #line 34 "..\..\..\Base.xaml"
            this.MovableBorder.MouseMove += new System.Windows.Input.MouseEventHandler(this.MouseMove);
            
            #line default
            #line hidden
            return;
            case 8:
            this.MovableTextBox = ((System.Windows.Controls.RichTextBox)(target));
            
            #line 36 "..\..\..\Base.xaml"
            this.MovableTextBox.KeyDown += new System.Windows.Input.KeyEventHandler(this.TextRedactor);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 39 "..\..\..\Base.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Bold);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

