﻿#pragma checksum "..\..\..\..\HUDS\PruebaHUD.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1D098A29E53851C0B33A2AE023CF91E422121F50"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using KrakenEasy.HUDS;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace KrakenEasy.HUDS {
    
    
    /// <summary>
    /// PruebaHUD
    /// </summary>
    public partial class PruebaHUD : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\..\HUDS\PruebaHUD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal KrakenEasy.HUDS.PruebaHUD Window;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\HUDS\PruebaHUD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Viewbox Cotenedor_Principal;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\HUDS\PruebaHUD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Name_Jugador;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\HUDS\PruebaHUD.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Contenedor;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.6.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/KrakenEasy;component/huds/pruebahud.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\HUDS\PruebaHUD.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.6.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Window = ((KrakenEasy.HUDS.PruebaHUD)(target));
            
            #line 8 "..\..\..\..\HUDS\PruebaHUD.xaml"
            this.Window.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseDown);
            
            #line default
            #line hidden
            
            #line 12 "..\..\..\..\HUDS\PruebaHUD.xaml"
            this.Window.Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 13 "..\..\..\..\HUDS\PruebaHUD.xaml"
            this.Window.MouseLeave += new System.Windows.Input.MouseEventHandler(this.Window_MouseLeave);
            
            #line default
            #line hidden
            
            #line 16 "..\..\..\..\HUDS\PruebaHUD.xaml"
            this.Window.Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Cotenedor_Principal = ((System.Windows.Controls.Viewbox)(target));
            return;
            case 3:
            this.Name_Jugador = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.Contenedor = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

