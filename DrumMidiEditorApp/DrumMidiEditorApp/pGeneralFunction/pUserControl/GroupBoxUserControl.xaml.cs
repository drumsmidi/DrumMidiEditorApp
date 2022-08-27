using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DrumMidiEditorApp.pGeneralFunction.pUserControl;

public sealed partial class GroupBoxUserControl : UserControl
{
    public GroupBoxUserControl()
    {
        InitializeComponent();
    }

    #region Property:Header

    public string Header
    {
        get => (string)GetValue( HeaderProperty );
        set => SetValue( HeaderProperty, value );
    }

    // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register
            (
                "Header", 
                typeof( string ), 
                typeof( HGroupUserControl ), 
                new PropertyMetadata( "Your Header", HeaderPropertyChangedCallback )
            );

    public static void HeaderPropertyChangedCallback( DependencyObject sender, DependencyPropertyChangedEventArgs ev )
    {
        var obj = sender as GroupBoxUserControl;

        if ( ev.NewValue != ev.OldValue && obj != null )
        {
            obj._HeaderTitle.Text = ev.NewValue?.ToString() ?? string.Empty ;
        }
    }

    #endregion

    #region Property:CustomContent

    public object CustomContent
    {
        get => (object)GetValue( CustomContentProperty );
        set => SetValue( CustomContentProperty, value );
    }

    public static readonly DependencyProperty CustomContentProperty =
        DependencyProperty.Register
            (
                "CustomContent", 
                typeof(object), 
                typeof(HGroupUserControl), 
                new PropertyMetadata( null, PropertyChangedCallback ) 
            );

    public static void PropertyChangedCallback( DependencyObject sender, DependencyPropertyChangedEventArgs ev )
    {
        var obj = sender as GroupBoxUserControl;

        if ( ev.NewValue != ev.OldValue && obj != null )
        {
            obj._Content.Content = ev.NewValue;
        }
    }

    #endregion

    private void HeaderTitle_LayoutUpdated( object sender, object ev )
    {
        _Border.Margin = new( _HeaderTitle.ActualWidth + 10, 10, 3, 3 );
    }
}
