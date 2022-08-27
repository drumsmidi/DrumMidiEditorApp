using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace DrumMidiEditorApp.pGeneralFunction.pUserControl;

public sealed partial class HGroupUserControl : UserControl
{
    public HGroupUserControl()
    {
        InitializeComponent();
    }

    #region Property:Header

    public string Header
    {
        get => (string)GetValue( HeaderProperty );
        set => SetValue( HeaderProperty, value );
    }

    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register
            (
                "Header", 
                typeof( string ), 
                typeof( HGroupUserControl ), 
                new PropertyMetadata( "Your Header", HeaderPropertyChangedCallback )
            );

    public static void HeaderPropertyChangedCallback( DependencyObject sender, DependencyPropertyChangedEventArgs args )
    {
        var obj = sender as HGroupUserControl;

        if ( args.NewValue != args.OldValue && obj != null )
        {
            obj._HeaderTitle.Text = args.NewValue?.ToString() ?? string.Empty ;
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
                typeof( object ), 
                typeof( HGroupUserControl ), 
                new PropertyMetadata( null, PropertyChangedCallback ) 
            );

    public static void PropertyChangedCallback( DependencyObject sender, DependencyPropertyChangedEventArgs args )
    {
        var obj = sender as HGroupUserControl;

        if ( args.NewValue != args.OldValue && obj != null )
        {
            obj._Content.Content = args.NewValue;
        }
    }

    #endregion
}
