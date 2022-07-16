using System;
using System.Windows.Forms;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pGeneralFunction.pLog;

namespace DrumMidiEditor.pView.pEditer.pEdit;

/// <summary>
/// インポートMIDIフォーム
/// </summary>
internal partial class ImportMidiForm : Form
{
    /// <summary>
    /// 取込実施フラグ
    /// </summary>
	private DialogResult _Result = DialogResult.None;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ImportMidiForm()
    {
        InitializeComponent();
    }

    /// <summary>
    /// フォーム表示
    /// </summary>
    /// <param name="aZoom">BPMズーム初期値</param>
    /// <returns>取込実施フラグ</returns>
	public DialogResult ShowDialog( int aZoom )
	{
        try
        {
            ZoomNumericUpDown.Value = aZoom;

    		ShowDialog();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}

		return _Result;
	}

    /// <summary>
    /// 取込実施
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
	private void OkButton_Click( object sender, EventArgs ev )
    {
        try
        { 
            Config.Media.MidiImportZoom = (int)ZoomNumericUpDown.Value;

			_Result = DialogResult.OK;
            Hide();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

    /// <summary>
    /// キャンセル
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
    private void CxlButton_Click( object sender, EventArgs ev )
    {
        try
        { 
			_Result = DialogResult.Cancel;
            Hide();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }
}
