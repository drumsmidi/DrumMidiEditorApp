using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pControl;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using DrumMidiPlayerApp.pIO;
using DrumMidiPlayerApp.pModel;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;

namespace DrumMidiPlayerApp.pView;

public sealed partial class WindowPlayer : Window
{
    /// <summary>
    /// �{�E�B���h�E�ւ̃A�N�Z�X
    /// </summary>
    private readonly AppWindow _AppWindow;

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public WindowPlayer()
    {
        // Midi�f�o�C�X������
        MidiNet.MidiOutDeviceWatcher();
        MidiNet.InitDeviceAsync( Config.Media.MidiOutDeviceName );

        // �E�B���h�E�\�z
        InitializeComponent();

        // ���g�̃E�B���h�E���擾
        _AppWindow = this.GetAppWindow();

        // �^�C�g�������ݒ�
        Title = $"{Config.Window.AppName}";

        // �Ǝ��̃^�C�g���o�[�ݒ�
        ExtendsContentIntoTitleBar = true;
        SetTitleBar( _AppTitleBar );
        SetSubTitle( $"[{DMS.OpenFilePath.AbsoulteFilePath}]" );

        // �E�B���h�E�����T�C�Y�ύX
        UpdateWindowsSize();

        // �ʏ�E�B���h�E�̃v���[���^�[�ݒ�
        HelperAppWindow.SetPresenterFixedWindow( _AppWindow );

        ControlAccess.MainWindow = this;

        // �Đ��R���g���[���J�n
        DmsControl.Start();
    }

    /// <summary>
    /// �A�N�e�B�u��ԍX�V
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void Window_Activated( object sender, WindowActivatedEventArgs args )
    {
        try
        {
            // �^�C�g���o�[����A�N�e�B�u��Ԃɂ��O�i�F�̕ύX
            var key =  args.WindowActivationState == WindowActivationState.Deactivated
                ? "WindowCaptionForegroundDisabled" : "WindowCaptionForeground";

            _AppTitleTextBlock.Foreground = (SolidColorBrush)Application.Current.Resources [ key ];
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// �E�B���h�E�I������
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void Window_Closed( object sender, WindowEventArgs args )
    {
        try
        {
            // �Đ��R���g���[����~
            DmsControl.StopPreSequence();
            DmsControl.End();

            // �v���C���[��~
            ControlAccess.UCPlayerPanel?.DrawTaskStop();

            // �ݒ�t�@�C���ۑ�
            _ = FileIO.SaveConfig();
        }
        catch ( Exception )
        {
            //Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// �^�C�g���o�[�̃T�u�^�C�g����ݒ�
    /// </summary>
    /// <param name="aSubTitle">�T�u�^�C�g��</param>
    public void SetSubTitle( string aSubTitle )
    {
        try
        {
            _AppTitleTextBlock.Text = $"{Config.Window.AppName} - {aSubTitle}";
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// �E�B���h�E�T�C�Y�X�V
    /// </summary>
    public void UpdateWindowsSize()
    {
        try
        {
            if ( Config.Window.WindowSizeWidth > 0 && Config.Window.WindowSizeHeight > 0 )
            {
                HelperAppWindow.ResizeWindow
                    (
                        _AppWindow,
                        Config.Window.WindowSizeWidth,
                        Config.Window.WindowSizeHeight
                    );
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
