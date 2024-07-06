using System.Collections.Generic;
using DrumMidiEditorApp.pModel;
using DrumMidiEditorApp.pConfig;

namespace DrumMidiEditorApp.pResume;

/// <summary>
/// レジューム管理
/// </summary>
public class ResumeManage
{
    /// <summary>
    /// Undoスタック
    /// </summary>
    private readonly Stack<IResume> _UndoResumeStack = new();

    /// <summary>
    /// Redoスタック
    /// </summary>
    private readonly Stack<IResume> _RedoResumeStack = new();

    /// <summary>
    /// レジューム実行後、Undoスタック
    /// </summary>
    /// <param name="aResume"></param>
    public void ExcuteAndResume( IResume aResume )
    {
        _RedoResumeStack.Clear();

        lock ( DMS.SCORE.LockObj )
        {
            aResume.Redo();
        }

        _UndoResumeStack.Push( aResume );

        // レジューム保持数を超えたデータを削除
        if ( _UndoResumeStack.Count > Config.Editer.ResumeStackCount )
        {
            while ( _UndoResumeStack.Count > 0 )
            {
                _RedoResumeStack.Push( _UndoResumeStack.Pop() );
            }
            while ( _RedoResumeStack.Count > Config.Editer.ResumeStackCount )
            {
                _ = _RedoResumeStack.Pop();
            }
            while ( _RedoResumeStack.Count > 0 )
            {
                _UndoResumeStack.Push( _RedoResumeStack.Pop() );
            }
        }
    }

    /// <summary>
    /// レジュームクリア
    /// </summary>
    public void Clear()
    {
        _UndoResumeStack.Clear();
        _RedoResumeStack.Clear();
    }

    /// <summary>
    /// Undo処理
    /// </summary>
    public void Undo()
    {
        if ( _UndoResumeStack.Count > 0 )
        {
            var rs = _UndoResumeStack.Pop();

            lock ( DMS.SCORE.LockObj )
            {
                rs.Undo();
            }

            _RedoResumeStack.Push( rs );
        }
    }

    /// <summary>
    /// Redo処理
    /// </summary>
    public void Redo()
    {
        if ( _RedoResumeStack.Count > 0 )
        {
            var rs = _RedoResumeStack.Pop();

            lock ( DMS.SCORE.LockObj )
            {
                rs.Redo();
            }

            _UndoResumeStack.Push( rs );
        }
    }
}
