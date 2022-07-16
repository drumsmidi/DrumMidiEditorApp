using System;
using System.Windows.Forms;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pGeneralFunction.pLog;

namespace DrumMidiEditor.pView.pEditer.pEdit;

/// <summary>
/// BPM入力フォーム
/// </summary>
internal partial class BpmInputForm : Form
{
	/// <summary>
	/// BPM入力フラグ
	/// </summary>
	private DialogResult _Result = DialogResult.None;

	/// <summary>
	/// BPM入力値
	/// </summary>
	public double Bpm
	{
		get => (double)BpmNumericUpDown.Value;
		set => BpmNumericUpDown.Value = (decimal)value;
	}

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public BpmInputForm()
    {
        InitializeComponent();
    }

	/// <summary>
	/// 入力フォーム表示
	/// </summary>
	/// <param name="aBpm">BPM初期値</param>
	/// <returns>入力実施フラグ</returns>
	public DialogResult ShowDialog( double aBpm )
	{
        try
        {
			if ( aBpm <= 0 )
			{
				aBpm = Config.System.DefaultBpm;
			}

			Bpm = aBpm;

			ShowDialog();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}

		return _Result;
	}

	/// <summary>
	/// 入力完了
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void OkButton_Click( object sender, EventArgs ev )
    {			
        try
        {
			_Result = DialogResult.Yes;
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
