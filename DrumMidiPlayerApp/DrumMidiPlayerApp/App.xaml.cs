using DrumMidiLibrary.pLog;
using DrumMidiPlayerApp.pConfig;
using DrumMidiPlayerApp.pIO;
using DrumMidiPlayerApp.pView;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;

namespace DrumMidiPlayerApp;

/// <summary>
/// �f�t�H���g��Application�N���X��⑫����A�v���P�[�V�����ŗL�̓����񋟂��܂��B
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// �V���O���g���A�v���P�[�V�����I�u�W�F�N�g�����������܂��B 
    /// ����́A���s�����쐬�ς݃R�[�h�̍ŏ��̍s�ł���A
    /// main�i�j�܂���WinMain�i�j�Ƙ_���I�ɓ����ł��B
    /// </summary>
    public App()
    {
        InitializeComponent();

        Config.Log.SetTraceLog();

        #region �C���X�^���X�Ǘ�

        // ����ŁA�P��C���X�^���X �A�v�� �Ȃ̂ő��d�N���̐���͕s�v

        // GetInstances�Ɍ��݂̃C���X�^���X��ǉ��ƋL�ڂ���������
        // ���s���Ȃ��ł��o�^����Ă���H
        //AppInstance.GetCurrent();

        // AppInstance�ɓo�^����Ă���C���X�^���X�̈ꗗ
        foreach ( var instance in AppInstance.GetInstances() )
        {
            Log.Info( $"ProcessId={instance.ProcessId}, Key={instance.Key}, IsCurrent={instance.IsCurrent}" );
        }

        #endregion
    }

    #region member

    /// <summary>
    /// ���C���E�B���h�E
    /// </summary>
    private Window? _MainWindow;

    #endregion

    /// <summary>
    /// �A�v���P�[�V�������G���h���[�U�[�ɂ���Đ���ɋN�����ꂽ�Ƃ��ɌĂяo����܂��B 
    /// �A�v���P�[�V�������N�����ē���̃t�@�C�����J���Ƃ��ȂǁA���̃G���g���|�C���g���g�p����܂��B
    /// </summary>
    /// <param name="aArgs">�N�����N�G�X�g�ƃv���Z�X�Ɋւ���ڍ�</param>
    protected override void OnLaunched( LaunchActivatedEventArgs aArgs )
    {
        using var _ = new LogBlock( "App.OnLaunched" );

        // Config�t�@�C���Ǎ�
        FileIO.LoadConfig();

        // ���C���E�B���h�E�쐬
        _MainWindow = new WindowPlayer();
        _MainWindow.Activate();
    }
}
