using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pDebug;

public sealed partial class PageDebugShell : Page
{
    ///// <summary>
    ///// コンストラクタ
    ///// </summary>
    //public PageDebugShell()
    //   {
    //	// 初期化
    //	InitializeComponent();
    //}

    //private void OpenButton_Click( object sender, RoutedEventArgs args )
    //{
    //       Process.Start( "EXPLORER.EXE", $"{AppDirectory.AppBaseDirectory}" );
    //   }

    //   private void ToastShowButton_Click( object sender, RoutedEventArgs args )
    //   {
    //       try
    //       {
    //           // トースト通知からのアプリアクティブ化
    //           //ToastNotificationManagerCompat.OnActivated += toastArgs =>
    //           //{
    //               //var args = ToastArguments.Parse( toastArgs.Argument );

    //               // Obtain any user input (text boxes, menu selections) from the notification
    //               //var userInput = toastArgs.UserInput;

    //               // 値をUIへ反映する場合、Dispatcherの処理が必要
    //           //};

    //           // トーストコンテンツを構成後、通知
    //           new ToastContentBuilder()
    //               // ---------------------------------------------------------
    //               // Launch:ユーザーがトーストをクリックしたときにアプリに渡される引数を定義
    //               // ---------------------------------------------------------
    //               .AddArgument("action", "viewConversation")
    //               .AddArgument("conversationId", 9813)

    //               // ---------------------------------------------------------
    //               // Visual:トーストの視覚的な部分
    //               // ---------------------------------------------------------

    //               // ヘッダー：通知ごとに同じ ID を指定すると通知内でグループ化される
    //               .AddHeader( "6289", "HeaderTitle", "action=openConversation&id=6289")

    //               // テキスト要素は３つまで追加可能。１行目がメインで、２，３行目がサブ情報
    //               .AddText( "トースト　テスト通知", hintMaxLines: 2 )
    //               .AddText( "説明要素：歩きスマホは" )
    //               .AddText( "説明要素：止めましょう"  )

    //               // インライン画像
    //               .AddInlineImage( new Uri( "https://avatars.githubusercontent.com/u/97685486?v=4" ) )

    //               // アプリ ロゴの上書き：Windows 11では無効?（アプリロゴは Attribute 領域に表示）
    //               //.AddAppLogoOverride(new Uri("ms-appdata:///local/Andrew.jpg"), ToastGenericAppLogoCrop.Circle)

    //               // ヒーロー イメージ：Attribute領域の上にイメージを表示
    //               .AddHeroImage( new Uri( "https://user-images.githubusercontent.com/97685486/182838106-50765a8a-814a-42c6-9714-23a23284b593.png" ) )

    //               /**
    //                * URI指定パターン
    //                *   - http://          http/https リモート Web画像 ファイルサイズの制限あり
    //                *   - ms-appx:///      
    //                *   - ms-appdata:///   
    //                */

    //               // カスタム タイムスタンプ：通知時刻をアプリ側から指定可能
    //               .AddCustomTimeStamp( new DateTime( 2017, 04, 15, 19, 45, 00, DateTimeKind.Utc ) )

    //               // 進捗状況バー
    //               .AddVisualChild
    //                   (
    //                       new AdaptiveProgressBar()
    //                       {
    //                           Title               = "進捗バー",
    //                           Value               = new BindableProgressBarValue( "progressValue" ),
    //                           ValueStringOverride = new BindableString( "progressValueString" ),
    //                           Status              = new BindableString( "progressStatus" )
    //                       }
    //                   )

    //               // ---------------------------------------------------------
    //               // Action:トーストの対話的な部分 (入力やアクションなど) 
    //               // ---------------------------------------------------------

    //               // テキスト入力
    //               .AddInputTextBox( "tbReply", placeHolderContent: "Type a response" )

    //               // 選択入力
    //               .AddToastInput
    //                   (
    //                       new ToastSelectionBox( "time" )
    //                       {
    //                           DefaultSelectionBoxItemId = "lunch",
    //                           Items =
    //                           {
    //                               new ToastSelectionBoxItem( "breakfast"  , "Breakfast"   ),
    //                               new ToastSelectionBoxItem( "lunch"      , "Lunch"       ),
    //                               new ToastSelectionBoxItem( "dinner"     , "Dinner"      ),
    //                           }
    //                       }
    //                   )

    //               // 再通知時間入力
    //               .AddToastInput
    //                   (
    //                       new ToastSelectionBox( "snoozeTime" )
    //                       {
    //                           DefaultSelectionBoxItemId = "15",
    //                           Items =
    //                           {
    //                               new( "5"      , "5 minutes"     ),
    //                               new( "15"     , "15 minutes"    ),
    //                               new( "60"     , "1 hour"        ),
    //                               new( "240"    , "4 hours"       ),
    //                               new( "1440"   , "1 day"         )
    //                           }
    //                       })

    //               // ボタン
    //               .AddButton
    //                   (
    //                       new ToastButton()
    //                           .SetContent("Reply")
    //                           .SetTextBoxId( "tbReply" )
    //                           .AddArgument("action", "reply")
    //                           .SetBackgroundActivation()
    //                   )
    //               .AddButton
    //                   (
    //                       new ToastButton()
    //                           .SetContent("Like")
    //                           .AddArgument("action", "like")
    //                           .SetImageUri( new Uri( "Assets/NotificationButtonIcons/Dismiss.png", UriKind.Relative ) )
    //                           .SetBackgroundActivation()
    //                   )
    //               //.AddButton
    //               //    ( 
    //               //        new ToastButton()
    //               //            .SetContent("View")
    //               //            .AddArgument("action", "viewImage")
    //               //            .AddArgument("imageUrl", image.ToString() ) 
    //               //    )

    //               // [一時停止(再通知)]ボタン
    //               .AddButton
    //                   (
    //                       new ToastButtonSnooze() 
    //                       { 
    //                           SelectionBoxId = "snoozeTime",
    //                       }
    //                   )

    //               // [無視]ボタン
    //               .AddButton( new ToastButtonDismiss() )

    //               // シナリオ
    //               .SetToastScenario( ToastScenario.Reminder )         // リマインダー
    //               //.SetToastScenario( ToastScenario.Alarm )          // アラーム：トースト通知に少なくとも 1 つのボタンを指定する必要あり
    //               //.SetToastScenario( ToastScenario.IncomingCall )   // 着信呼び出し
    //               //.SetToastScenario( ToastScenario.******** )       // 重要な通知：未実装

    //               // ---------------------------------------------------------
    //               // Audio:トーストがユーザーに表示されるときに再生されるオーディオ
    //               //
    //               // [既定のサウンド一覧]
    //               // https://docs.microsoft.com/ja-jp/uwp/schemas/tiles/toastschema/element-audio#attributes-and-elements
    //               // ---------------------------------------------------------
    //               .AddAudio( new Uri( "ms-appx:///Sound.mp3" ) )

    //               // ---------------------------------------------------------
    //               // Show
    //               // ---------------------------------------------------------
    //               .Show
    //               (
    //                   ( toast ) =>
    //                   {
    //                       toast.Tag               = "TagName";
    //                       toast.Group             = "GroupName";
    //                       toast.ExpirationTime    = DateTime.Now.AddMinutes( 3 );     // 有効期限

    //                       toast.Data = new NotificationData();
    //                       toast.Data.Values[ "progressValue" ]            = "0.6";
    //                       toast.Data.Values[ "progressValueString" ]      = "15/26 songs";
    //                       toast.Data.Values[ "progressStatus" ]           = "Downloading...";
    //                       toast.Data.SequenceNumber                       = 0;      // 0(常に更新する)、1-(シーケンス位置指定)
    //                   }
    //               ); 

    //           // ヘッダーテスト
    //           //new ToastContentBuilder()
    //           //    // ---------------------------------------------------------
    //           //    // Visual:トーストの視覚的な部分
    //           //    // ---------------------------------------------------------

    //           //    // ヘッダー
    //           //    .AddHeader( "6289", "HeaderTitle", "action=openConversation&id=6289")

    //           //    // テキスト要素は３つまで追加可能。１行目がメインで、２，３行目がサブ情報
    //           //    .AddText( "２２２" )

    //           //    // ---------------------------------------------------------
    //           //    // Show
    //           //    // ---------------------------------------------------------
    //           //    .Show
    //           //    (
    //           //        ( toast ) =>
    //           //        {
    //           //            toast.Tag               = "TagName2";
    //           //            toast.Group             = "GroupName";
    //           //            toast.ExpirationTime    = DateTime.Now.AddMinutes( 3 );     // 有効期限
    //           //        }
    //           //    ); 
    //       }
    //       catch ( Exception e )
    //       {
    //           Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
    //       }

    //       // スケジュール通知
    //       new ToastContentBuilder()
    //           .AddText( "5秒後に通知" )
    //           .Schedule
    //               ( 
    //                   DateTime.Now.AddSeconds( 5 ),
    //                   toast =>
    //                   {
    //                       toast.Tag   = "18365";
    //                       toast.Group = "ASTR 170B1";
    //                   }
    //               );

    //       // スケジュールの削除
    //       var notifier = ToastNotificationManagerCompat.CreateToastNotifier();

    //       var scheduledToasts = notifier.GetScheduledToastNotifications();

    //       var toRemove = scheduledToasts
    //           .FirstOrDefault( toast => toast.Tag == "18365" && toast.Group == "ASTR 170B1" );

    //       if ( toRemove != null )
    //       {
    //           notifier.RemoveFromSchedule( toRemove );
    //       }
    //   }

    //   private void ToastUpdateButton_Click( object sender, RoutedEventArgs args )
    //   {
    //       try
    //       {
    //           var data = new NotificationData
    //           {
    //               SequenceNumber = 0,      // 0(常に更新する)
    //           };

    //           data.Values[ "progressValue" ]          = "1.0";
    //           data.Values[ "progressValueString" ]    = "26/26 songs";
    //           data.Values[ "progressStatus" ]         = "Finished";

    //           ToastNotificationManager.CreateToastNotifier().Update( data, "TagName", "GroupName" );
    //       }
    //       catch ( Exception e )
    //       {
    //           Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
    //       }
    //   }

    //   private void ToastClearButton_Click( object sender, RoutedEventArgs args )
    //   {
    //       try
    //       {
    //           // 全てのトースト履歴をクリア
    //           ToastNotificationManagerCompat.History.Clear();

    //           // 対象グループのトースト履歴を削除
    //           ToastNotificationManagerCompat.History.RemoveGroup( "GroupName" );

    //           // 対象タグのトースト履歴を削除
    //           ToastNotificationManagerCompat.History.Remove( "TagName" );

    //           // 対象タグ・グループに一致するトースト履歴を削除
    //           ToastNotificationManagerCompat.History.Remove( "TagName", "GroupName" );
    //       }
    //       catch ( Exception e )
    //       {
    //           Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
    //       }
    //   }

    //   private void BadgeNumberButton_Click( object sender, RoutedEventArgs args )
    //   {
    //       try
    //       {
    //           var badgeXml = BadgeUpdateManager.GetTemplateContent( BadgeTemplateType.BadgeNumber );

    //           // 数値を設定
    //           // 　1 ～ 99 の数字。 値 0 はグリフ値 "none" と同じであり、バッジをクリア
    //           // 　99 を超える数字は、"99+" 表示
    //           var badgeElement = badgeXml.SelectSingleNode( "/badge" ) as XmlElement;
    //           badgeElement?.SetAttribute( "value", "99" );

    //           // バッジ更新
    //           BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update( new( badgeXml ) );
    //       }
    //       catch ( Exception e )
    //       {
    //           Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
    //       }
    //   }

    //   private void BadgeGlyphButton_Click( object sender, RoutedEventArgs args )
    //   {
    //       try
    //       {
    //           var badgeXml = BadgeUpdateManager.GetTemplateContent( BadgeTemplateType.BadgeGlyph );

    //           var badgeElement = badgeXml.SelectSingleNode( "/badge" ) as XmlElement;
    //           badgeElement?.SetAttribute( "value", "alert" );

    //           // -----------------------------------------------
    //           // [グリフ設定値]
    //           // -----------------------------------------------
    //           // none        : なし
    //           // activity    : activity
    //           // alarm       : alarm(アラーム)
    //           // alert       : アラート
    //           // attention   : attention(注意)
    //           // available   : 利用可能
    //           // away        : away(離席中)
    //           // busy        : busy(取り込み中)
    //           // error       : error
    //           // newMessage  : newMessage(新しいメッセージ)
    //           // paused      : paused(一時停止)
    //           // playing     : playing(再生)
    //           // unavailable : unavailable(利用不可)
    //           // -----------------------------------------------

    //           // バッジ更新
    //           BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update( new( badgeXml ) );
    //       }
    //       catch ( Exception e )
    //       {
    //           Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
    //       }
    //   }

    //   private void BadgeClearButton_Click( object sender, RoutedEventArgs args )
    //   {
    //       try
    //       {
    //           // バッジをクリア
    //           BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
    //       }
    //       catch ( Exception e )
    //       {
    //           Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
    //       }
    //   }

    //   private async void TaskBarRegistButton_Click( object sender, RoutedEventArgs args )
    //   {
    //       try
    //       {
    //           // APIサポートチェック
    //           if ( !ApiInformation.IsTypePresent( "Windows.UI.Shell.TaskbarManager" ) )
    //           {
    //               return;
    //           }

    //           // タスク バーが存在し、ピン留めを使用できるかどうかを確認
    //           if ( !TaskbarManager.GetDefault().IsPinningAllowed )
    //           {
    //               return;
    //           }

    //           // アプリが現在タスク バーにピン留めされているかどうかを確認
    //           if ( await TaskbarManager.GetDefault().IsCurrentAppPinnedAsync() )
    //           {
    //               return;
    //           }

    //           // ピン止め確認：エラーになる
    //           if ( await TaskbarManager.GetDefault().RequestPinCurrentAppAsync() )
    //           {

    //           }
    //       }
    //       catch ( Exception e )
    //       {
    //           Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
    //       }
    //   }

    //   private void RestartAppButton_Click( object sender, RoutedEventArgs args )
    //   {
    //       try
    //       {
    //           var restartArgsInput = string.Empty;

    //           // アプリ起動時のコマンドライン引数を取得
    //           var commandLineArguments = Environment.GetCommandLineArgs();

    //           if ( commandLineArguments.Length > 1 )
    //           {
    //               commandLineArguments = commandLineArguments.Skip( 1 ).ToArray();

    //               restartArgsInput = string.Join( ",", commandLineArguments );
    //           }

    //           // アクティブ化時の引数を取得
    //           var activatedArgs = AppInstance.GetCurrent().GetActivatedEventArgs();

    //           switch ( activatedArgs.Kind )
    //           {
    //               case ExtendedActivationKind.Launch:
    //                   {
    //                       if ( activatedArgs.Data is ILaunchActivatedEventArgs launchArgs )
    //                       {
    //                           var argStrings = launchArgs.Arguments.Split();

    //                           if ( argStrings.Length > 1 )
    //                           {
    //                               argStrings = argStrings.Skip( 1 ).ToArray();

    //                               restartArgsInput = string.Join( ",", argStrings.Where( s => !string.IsNullOrEmpty( s ) ) );
    //                           }
    //                       }
    //                   }
    //                   break;
    //           }

    //           // アプリ再起動
    //           var restartError = AppInstance.Restart( restartArgsInput );

    //           switch ( restartError )
    //           {
    //               case AppRestartFailureReason.RestartPending:
    //                   Log.Error( "Another restart is currently pending." );
    //                   break;
    //               case AppRestartFailureReason.InvalidUser:
    //                   Log.Error( "Current user is not signed in or not a valid user." );
    //                   break;
    //               case AppRestartFailureReason.Other:
    //                   Log.Error( "Failure restarting." );
    //                   break;
    //           }
    //       }
    //       catch ( Exception e )
    //       {
    //           Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
    //       }
    //   }
}
