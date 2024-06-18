namespace DrumMidiEditorApp.pModel;

/// <summary>
/// プレイヤー描画モード
/// </summary>
public enum PlayerSurfaceMode : int
{
    Sequence = 0,
    Score,
    Simuration,
}

/// <summary>
/// プレイヤー再生リクエスト一覧
/// </summary>
public enum PlayRequest : int
{
    None = 0,
    PreStop,
    PrePlay,
    PreLoopPlay,
    PreRecord,
}
