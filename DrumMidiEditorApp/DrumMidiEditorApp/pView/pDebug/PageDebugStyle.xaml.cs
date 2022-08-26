using System;
using System.Diagnostics;
using CommunityToolkit.WinUI.Notifications;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pGeneralFunction.pUtil;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pDebug;

public sealed partial class PageDebugStyle : Page
{
	/// <summary>
	/// コンストラクタ
	/// </summary>
	public PageDebugStyle()
    {
		// 初期化
		InitializeComponent();
	}
}
