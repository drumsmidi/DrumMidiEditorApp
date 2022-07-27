using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.System;
using Windows.UI;

using DrumMidiEditorApp.pAudio;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pAudio;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;
using DrumMidiEditorApp.pResume;
using Microsoft.UI.Windowing;
using DrumMidiEditorApp.pView.pPlayer.pSurface;
using DrumMidiEditorApp.pGeneralFunction.pUtil;

namespace DrumMidiEditorApp.pView.pMidiMap;

public sealed partial class UserControlMidiMapPanel : UserControl
{
    #region Member

    /// <summary>
    /// Player設定
    /// </summary>
    private ConfigPlayer DrawSet => Config.Player;

    /// <summary>
    /// System設定
    /// </summary>
    private ConfigSystem ConfigSystem => Config.System;

    /// <summary>
    /// Scale設定
    /// </summary>
    private ConfigScale ConfigScale => Config.Scale;

    /// <summary>
    /// Media設定
    /// </summary>
    private ConfigMedia ConfigMedia => Config.Media;

    /// <summary>
    /// Score情報
    /// </summary>
    private Score Score => DMS.SCORE;

    private PeriodicTimer? _Timer = null;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public UserControlMidiMapPanel()
    {
        InitializeComponent();
    }
}
