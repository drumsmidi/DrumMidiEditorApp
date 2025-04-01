using System;
using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pControl;
using DrumMidiLibrary.pInput;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using DrumMidiPlayerApp.pEvent;
using DrumMidiPlayerApp.pIO;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.Gaming.Input;

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
        SetSubTitle( string.Empty );
    //  SetSubTitle( $"[{DMS.OpenFilePath.AbsoulteFilePath}]" );

        // �E�B���h�E�����T�C�Y�ύX
        UpdateWindowsSize();

        // �ʏ�E�B���h�E�̃v���[���^�[�ݒ�
        HelperAppWindow.SetPresenterFixedWindow( _AppWindow );

        ControlAccess.MainWindow = this;

        // �����L����
        SystemSound.SoundOn();

        // �Đ��R���g���[���J�n
        DmsControl.Start();

        // �L�[�C�x���g�L���v�`��
        _MainGrid.KeyUp   += InputControl.KeyUp;
        _MainGrid.KeyDown += InputControl.KeyDown;

        // �Q�[���p�b�h�Ď�
        InputControl.SetGamePadWatcher();

        InputControl.StartTime();
    }

    /// <summary>
    /// �A�N�e�B�u��ԍX�V
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void Window_Activated( object aSender, WindowActivatedEventArgs aArgs )
    {
        try
        {
            // �^�C�g���o�[����A�N�e�B�u��Ԃɂ��O�i�F�̕ύX
            var key =  aArgs.WindowActivationState == WindowActivationState.Deactivated
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
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void Window_Closed( object aSender, WindowEventArgs aArgs )
    {
        try
        {
            // �L�[�C�x���g�L���v�`�����
            _MainGrid.KeyUp   -= InputControl.KeyUp;
            _MainGrid.KeyDown -= InputControl.KeyDown;

            // �Q�[���p�b�h�Ď����
            InputControl.ReleaseGamePadWatcher();

            // �Đ��R���g���[����~
            DmsControl.StopPreSequence();
            DmsControl.End();

            // �v���C���[��~
            ControlAccess.MainPanel?.DrawTaskStop();

            // �ݒ�t�@�C���ۑ�
            _ = FileIO.SaveConfig();
        }
        catch ( Exception )
        {
            //Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// �E�B���h�E�T�C�Y�ύX
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void Window_SizeChanged( object aSender, WindowSizeChangedEventArgs aArgs )
    {
        try
        {
            EventManage.Event_Window_ResizeWindow();
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
            // NOTE: DPI�X�P�[���擾�ł��Ȃ��HWindowsAPI�d�l���K�v�H
            // DPI�X�P�[���擾
            Config.Window.DpiScale = 1.5; // Content.RasterizationScale;

            // �p�l���̉𑜓x�ɂɍ��킹�ăE�B���h�E�T�C�Y��ύX����
            Config.Window.WindowSizeWidthDpiNoScale     = (int)Config.Panel.ResolutionScreenWidth;
            Config.Window.WindowSizeHeightDpiNoScale    = (int)Config.Panel.ResolutionScreenHeight;

            var width  = Config.Window.WindowSizeWidthDpiScale;
            var height = Config.Window.WindowSizeHeightDpiScale;

            if ( width > 0 && height > 0 )
            {
                HelperAppWindow.ResizeWindowClient( _AppWindow, width, height );
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
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
            _AppTitleTextBlock.Text = aSubTitle.Length != 0 
                ? $"{Config.Window.AppName} - {aSubTitle}"
                : $"{Config.Window.AppName}" ;
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
