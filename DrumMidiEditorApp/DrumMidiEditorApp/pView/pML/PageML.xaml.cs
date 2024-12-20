using System;
using DrumMidiEditorApp.pLog;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using DrumMidiEditorApp.pUtil;
using DrumMidiEditorApp.pMachineLearning;
using DrumMidiEditorApp.pAudio;
using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pModel;
using Microsoft.Graphics.Canvas;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.DirectX;
using Windows.UI;
using OpenCvSharp;
using System.Linq;
using System.Threading;

namespace DrumMidiEditorApp.pView.pML;

public sealed partial class PageML : Page
{
    /// <summary>
    /// スコア設定
    /// </summary>
    private ConfigML DrawSet => Config.ML;

    /// <summary>
    /// Score情報
    /// </summary>
    private static Score Score => DMS.SCORE;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageML()
    {
        InitializeComponent();

        ControlAccess.PageML = this;
    }

    /// <summary>
    /// スコア更新フラグ設定
    /// </summary>
    private static void UpdateScore() => EventManage.EventScoreUpdateTab();

    /// <summary>
    /// 再生停止
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MachineLearning_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            UpdateScaleBgm();

            MLControl.Start();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #region BgmScale

    /// <summary>
    /// 音階別周波数表示用BGM
    /// </summary>
    public NAudioData? ScaleBgm { get; private set; } = null;

    /// <summary>
    /// BGM音階解析用のBGM更新
    /// </summary>
    private void UpdateScaleBgm()
    {
        try
        {
            // BGM削除
            ScaleBgm?.Dispose();
            ScaleBgm = null;

            // BGM読込
            if ( Score.BgmFilePath.IsExistFile )
            {
                ScaleBgm = new NAudioData( Score.BgmFilePath, true );
            }

            DrawSet.UpdateScoreBgmScaleFlag = true;
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion
}
