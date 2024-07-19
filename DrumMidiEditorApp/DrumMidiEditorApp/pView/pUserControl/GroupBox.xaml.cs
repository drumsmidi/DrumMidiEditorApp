using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pUserControl;

public sealed partial class GroupBox : UserControl
{
    public GroupBox()
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
                typeof( HGroup ),
                new PropertyMetadata( "Your Header", HeaderPropertyChangedCallback )
            );

    public static void HeaderPropertyChangedCallback( DependencyObject sender, DependencyPropertyChangedEventArgs ev )
    {
        var obj = sender as GroupBox;

        if ( ev.NewValue != ev.OldValue && obj != null )
        {
            obj._HeaderTitle.Text = ev.NewValue?.ToString() ?? string.Empty ;
        }
    }

    #endregion

    #region Property:CustomContent

    public object CustomContent
    {
        get => GetValue( CustomContentProperty );
        set => SetValue( CustomContentProperty, value );
    }

    public static readonly DependencyProperty CustomContentProperty =
        DependencyProperty.Register
            (
                "CustomContent",
                typeof( object ),
                typeof( HGroup ),
                new PropertyMetadata( null, PropertyChangedCallback )
            );

    public static void PropertyChangedCallback( DependencyObject sender, DependencyPropertyChangedEventArgs ev )
    {
        var obj = sender as GroupBox;

        if ( ev.NewValue != ev.OldValue && obj != null )
        {
            obj._Content.Content = ev.NewValue;
        }
    }

    #endregion

    private void HeaderTitle_LayoutUpdated( object sender, object ev ) 
        => _Border.Margin = new( _HeaderTitle.ActualWidth + 10, 10, 3, 3 );
}
