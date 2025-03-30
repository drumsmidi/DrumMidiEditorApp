﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pUserControl;

public sealed partial class HGroup : UserControl
{
    public HGroup()
    {
        InitializeComponent();
    }

    #region Property:Header

    /// <summary>
    /// 
    /// </summary>
    public string Header
    {
        get => (string)GetValue( HeaderProperty );
        set => SetValue( HeaderProperty, value );
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register
            (
                "Header",
                typeof( string ),
                typeof( HGroup ),
                new PropertyMetadata( "Your Header", HeaderPropertyChangedCallback )
            );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    public static void HeaderPropertyChangedCallback( DependencyObject aSender, DependencyPropertyChangedEventArgs aArgs )
    {
        var obj = aSender as HGroup;

        if ( aArgs.NewValue != aArgs.OldValue && obj != null )
        {
            obj._HeaderTitle.Text = aArgs.NewValue?.ToString() ?? string.Empty ;
        }
    }

    #endregion

    #region Property:CustomContent

    /// <summary>
    /// 
    /// </summary>
    public object CustomContent
    {
        get => GetValue( CustomContentProperty );
        set => SetValue( CustomContentProperty, value );
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty CustomContentProperty =
        DependencyProperty.Register
            (
                "CustomContent",
                typeof( object ),
                typeof( HGroup ),
                new PropertyMetadata( null, PropertyChangedCallback )
            );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    public static void PropertyChangedCallback( DependencyObject aSender, DependencyPropertyChangedEventArgs aArgs )
    {
        var obj = aSender as HGroup;

        if ( aArgs.NewValue != aArgs.OldValue && obj != null )
        {
            obj._Content.Content = aArgs.NewValue;
        }
    }

    #endregion
}
