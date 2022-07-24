using DrumMidiEditorApp.pView.pPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrumMidiEditorApp.pView;

public static class WindowMange
{
	/// <summary>
	/// プレイヤーウィンドウ
	/// </summary>
	public static WindowPlayer? WindowPlayer { get; private set; } = null;


	public static void InitializeWindow()
    {
		WindowPlayer = new();
	}

	public static void DestroyWindow()
    {
		WindowPlayer?.Exit();
	}
}
