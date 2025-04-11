using DrumMidiLibrary.pUtil;

namespace DrumMidiPlayerApp.pModel.pOption;

/// <summary>
/// 曲リストアイテム
/// </summary>
public partial class OptionListItem : DisposeBaseClass
{
    /// <summary>
    /// アイテム名
    /// </summary>
    public string ItemName { get; private set; } = string.Empty;

    /// <summary>
    /// ファイルパス
    /// </summary>
    public GeneralPath? FilePath { get; private set; } = null;

    /// <summary>
    /// 曲リスト
    /// </summary>
    public OptionList OptionList { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aFilePath"></param>
    public OptionListItem( string aItemName )
    {
        ItemName    = aItemName;
        FilePath    = new( aItemName );
        OptionList  = new( this );
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aFilePath"></param>
    public OptionListItem( GeneralPath aFilePath )
    {
        ItemName    = aFilePath.FileNameWithoutExtension;
        FilePath    = new( aFilePath );
        OptionList  = new( this );
    }

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                OptionList?.Dispose();
            }

            // Dispose unmanaged resources.

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;
}
