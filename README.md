# 概要

ドラム耳コピ用に作成したアプリ  
- BGMの再生、および入力した譜面を元にMIDI-OUTイベントの発行を行います。
<img width="1196" alt="image" src="https://github.com/user-attachments/assets/5ac718a2-7ba9-4b30-abf3-01c8caffab91" />

- オーディオインタフェースやドラム音源は自前で用意する必要があります。
<img width="284" alt="image" src="https://user-images.githubusercontent.com/97685486/182602780-22433054-0a64-4e2d-a41b-3394893d5f53.png">

- DrumMidiPlayerApp試作中  
[![](https://i.ytimg.com/vi/v3fcRcVwsrM/hqdefault.jpg?sqp=-oaymwFBCNACELwBSFryq4qpAzMIARUAAIhCGAHYAQHiAQoIGBACGAY4AUAB8AEB-AH-CYAC0AWKAgwIABABGGUgZShlMA8=&rs=AOn4CLCV7OXxpnmJqPrwb1lnRD8pDyKFCQ)](https://www.youtube.com/watch?v=v3fcRcVwsrM)

## 技術要素

- C#
- .NET9
- WinUI3
- Win2D

## 使用パッケージ

| 使用パッケージ | メモ |
| --- | --- |
| Microsoft.Windows.SDK.BuildTools | |
| Microsoft.WindowsAppSDK | |
| Microsoft.Graphics.Win2D | Canvasの描画に使用 |
| Microsoft.ML | 機械学習に使用（活用できてません） |
| NAudio | BGM再生、周波数解析に使用 |
| OpenCvSharp4.Windows | MP4の動画出力に使用 |
| LiteDB | ローカルDB |

## ソリューション構成

| 使用パッケージ | メモ |
| --- | --- |
| NAudio | NAudioのソースを一部組込 |
| DrumMidiLibrary | エディター・プレイヤー共通ライブラリ |
| DrumMidiEditorApp | エディター |
| DrumMidiEditorApp.Package | エディター |
| DrumMidiPlayerApp | プレイヤー（試作中） |
| DrumMidiPlayerApp.Package | プレイヤー（試作中） |

-----------------------------------------------------------------------------
# Github設定メモ

## Actions によるビルド設定

### 署名の作成

[証明書の選択]より、証明書の作成を行う。

<img width="854" alt="image" src="https://github.com/drumsmidi/DrumMidieditorApp/assets/172901992/ccb93cb9-a3d2-4632-b317-011920e79260">

> 署名のパスワードは後ほど使用

### Githubへの署名登録

1. Visual Studioで［ツール］－［コマンドライン］－［開発者用 PowerShell］をクリックしてPowerShellを起動し、下記コマンドを実行

```BAT
cd '.\DrumMidiEditorApp\DrumMidiEditorApp (Package)\'

$pfx_cert = Get-Content '.\DrumMidiEditorApp (Package)_TemporaryKey.pfx' -Encoding Byte
[System.Convert]::ToBase64String($pfx_cert) | Out-File 'DrumMidiEditorApp (Package)_TemporaryKey.txt'
```

> DrumMidiEditorApp (Package)_TemporaryKey.txt に暗号化キーが出力される


2. Githubの [Settings] - [Secrets] - [Actions] の　[New repository secret]ボタンより下記２つの設定を登録

- １個目
  - Name："Base64_Encoded_Pfx"
  - Value：1で作成した「DrumMidiEditorApp (Package)_TemporaryKey.txt」の中身

- ２個目
  - Name："Pfx_Key"
  - Value：署名のパスワード

> 上記設定は workflow実行時に参照される


### Workflowの追加

<img width="275" alt="image" src="https://user-images.githubusercontent.com/97685486/182817247-828b0966-806b-43a8-bbe0-a1b1c8845b32.png">

[参考：WinUI 3 アプリの継続的インテグレーションをセットアップする](https://docs.microsoft.com/ja-jp/windows/apps/package-and-deploy/ci-for-winui3?pivots=winui3-packaged-csharp)  

設定メモ
```yaml
name: .NET Core Desktop

on:
  push:
    branches: [ "main" ]
  pull_request:
    branches: [ "main" ]

jobs:

  build:

    strategy:
      matrix:
        configuration: [Release]
        #configuration: [Debug, Release]
        platform: [x64]

    runs-on: windows-latest  # For a list of available runner types, refer to
                             # https://help.github.com/en/actions/reference/workflow-syntax-for-github-actions#jobsjob_idruns-on

    env:
      Solution_Name: DrumMidiEditorApp.sln
      Test_Project_Path: none
      Wap_Project_Directory: DrumMidiEditorApp\DrumMidiEditorApp (Package)
      Wap_Project_Path: DrumMidiEditorApp\DrumMidiEditorApp (Package)\DrumMidiEditorApp (Package).wapproj

    steps:
    - name: Checkout
      uses: actions/checkout@v4
      with:
        fetch-depth: 0

    # Install the .NET Core workload
    - name: Install .NET Core
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: 9.0.x

    # Add  MSBuild to the PATH: https://github.com/microsoft/setup-msbuild
    - name: Setup MSBuild.exe
      uses: microsoft/setup-msbuild@v2

    # Execute all unit tests in the solution
    # - name: Execute unit tests
    #   run: dotnet test

    # Restore the application to populate the obj folder with RuntimeIdentifiers
    - name: Restore the application
      run: msbuild $env:Solution_Name /t:Restore /p:Configuration=$env:Configuration
      env:
        Configuration: ${{ matrix.configuration }}

    # Decode the base 64 encoded pfx and save the Signing_Certificate
    - name: Decode the pfx
      run: |
        $pfx_cert_byte = [System.Convert]::FromBase64String("${{ secrets.Base64_Encoded_Pfx }}")
        $certificatePath = Join-Path -Path $env:Wap_Project_Directory -ChildPath GitHubActionsWorkflow.pfx
        [IO.File]::WriteAllBytes("$certificatePath", $pfx_cert_byte)

    # Create the app package by building and packaging the Windows Application Packaging project
    - name: Create the app package
      run: msbuild $env:Wap_Project_Path /p:Configuration=$env:Configuration /p:UapAppxPackageBuildMode=$env:Appx_Package_Build_Mode /p:AppxBundle=$env:Appx_Bundle /p:PackageCertificateKeyFile=GitHubActionsWorkflow.pfx /p:PackageCertificatePassword=${{ secrets.Pfx_Key }}
      env:
        Appx_Bundle: Never
        #Appx_Bundle: Always
        Appx_Bundle_Platforms: x64
        #Appx_Bundle_Platforms: x86|x64
        Appx_Package_Build_Mode: SideloadOnly
        #Appx_Package_Build_Mode: StoreUpload
        Configuration: ${{ matrix.configuration }}
        Platform: ${{ matrix.platform }}

    # Remove the pfx
    - name: Remove the pfx
      run: Remove-Item -path $env:Wap_Project_Directory\GitHubActionsWorkflow.pfx

    # Upload the MSIX package: https://github.com/marketplace/actions/upload-a-build-artifact
    - name: Upload build artifacts
      uses: actions/upload-artifact@v4
      with:
        name: DrumMidiEditorApp(MSIX Package)
        path: ${{ env.Wap_Project_Directory }}\AppPackages
```

-----------------------------------------------------------------------------
# インストール方法

1. [リリースフォルダ](https://github.com/drumsmidi/DrumMidiEditorApp/releases)から「DrumMidiEditorApp.MSIX.Package.zip」をダウンロード

2. ダウンロードしたファイルを解凍後、「DrumMidiEditorApp (Package)_0.3.0.0_x64.cer」を実行し、[証明書のインストール]を実行する。  

<img width="565" alt="image" src="https://github.com/user-attachments/assets/c88dd047-a4c8-4ef4-8b0d-931800310be2" />

> 「ローカルコンピューター」、「信頼されたルート証明機関」として登録が必要

3. 「DrumMidiEditorApp (Package)_0.3.0.0_x64.msix」を実行し、インストールを行う

> 正しく設定されていない場合は、下記エラーが発生する。  
> 
> このアプリ パッケージの発行元証明書を検証できませんでした。  
> 確認済みの証明書を含む新しいアプリ パッケージを取得するには、システム管理者またはアプリ開発者に問い合わせてください。  
> アプリ パッケージ内の署名のルート証明書とすべての即時証明書を確認する必要があります (0x800B010A)


-----------------------------------------------------------------------------
# 画面仕様

-----------------------------------------------------------------------------
## Config画面

### システム設定

現時点では設定なし

### デバイス設定

<img width="429" alt="image" src="https://github.com/user-attachments/assets/25654163-a2a8-4d12-97e1-7dfa0ea2c083" />

#### Midi-Out デバイス

MIDI 出力先のデバイスを選択します。

#### Midi-Out 遅延

Midi-Outイベント送信から実際に音が鳴るまでの遅延時間（秒）を設定します。

### ビデオ出力設定

<img width="380" alt="image" src="https://github.com/user-attachments/assets/11d39299-87fc-4e12-8b58-a102554a0638" />

MP4ファイル出力時のエンコードを設定します。  
MP4ファイルの作成は、[opencvsharp](https://github.com/shimat/opencvsharp)を使用しています。
  
> 未設定のままでも出力可能です（逆に設定するとエラーになることもあります）

### エディター設定

<img width="824" alt="image" src="https://github.com/user-attachments/assets/ac0a6d21-e2e2-4e05-a6de-c99c85c1ef02" />

### プレイヤー共通設定

<img width="392" alt="image" src="https://github.com/user-attachments/assets/03526f18-fe00-42d8-824c-a71a519419da" />

### プレイヤー個別設定：ScoreType2

<img width="936" alt="image" src="https://github.com/user-attachments/assets/6be021e8-4ace-43e7-a492-b432dc416f6d" />

#### DarkMode

プレイヤーのダークモードON/OFF切替（未実装）

#### 現在のBpm表示

再生位置のBPM値表示ON/OFFを切替えます。

#### 音量によるノートサイズ変更

ノート音量に合わせてノートサイズ変更を行うかON/OFFを切替えます。

> テキスト表示のノートには適用されません

#### 音量０のノート表示

音量０ノート表示ON/OFFを切替えます。

#### ノートテキスト表示

MidiMapタブで"ScaleText"に入力した先頭１文字のテキストで表示を行うかON/OFFを切替えます。

#### カーソル塗潰し

再生位置を表示するカーソルより前の表示塗潰しON/OFFを切替えます。

#### 背景色

プレイヤーの背景色を指定します。

#### 小節番号高さ

小節番号表示エリアの高さを指定します。

#### ノート間隔 高さ・横幅

1セル間の高さと横幅を指定します。

#### ノート 高さ・横幅

ノート音量最大値のノートサイズを指定します。

#### 小節線

縦線の横幅サイズと色を指定します。

#### 表示設定

表へ追加したレイアウトで、プレイヤーの譜面情報を表示します。  
表示するノート位置は、MidiMapタブページのScaleKeyとで紐づけて描画されます。  

Display項目は、横線（太線）の表示ON/OFFを切替えます。  

> システム共通の設定です

### プレイヤー個別設定：Sequence

未メンテ

### プレイヤー個別設定：Simuration

未メンテ

-----------------------------------------------------------------------------
## メニューバー

<img width="816" alt="image" src="https://user-images.githubusercontent.com/97685486/182868335-c3031a72-2f58-416e-bf58-9f5e7a0c6fc5.png">

### File

| メニュー | 概要 |
| --- | --- |
| New | 編集中の情報を破棄し、新しいデータを作成します |
| Open | DMSファイルを開きます |
| Save | 編集中のDMSファイルを上書きします |
| SaveAs | 編集中のDMSファイルを別名で保存します |
| Export > Midi | MIDIファイルを出力します |
| Export > Pdf | 譜面をPdfファイルとして保存します (ScoreType2のみ対応） |
| Export > Video | シーケンスを動画として保存します。<br>Playerウィンドウの設定と [Config]-[Video] の設定を元に動画を作成します。（対応形式：MP4）|
| Import | MIDIファイルを取込みます。 ※未サポート |

### Ch選択

編集対象のチャンネルを切替えます。  ＜Ch.9 固定＞

> 非推奨：試しに追加しましたがチャンネル切替に対する考慮ができていません。

### Bgm

BGM再生のON/OFFを切替えます。

### Midi-Out

ノート再生(MIDI-OUTイベント)の ON/OFF を切替えます。

### 再生

最初から再生します。

### 停止

再生を停止します。

### ループ再生

小節開始番号～終了番号までの区間をループ再生します。

### 小節開始番号

ループ再生の開始小節位置を指定します。 (0～999)

### 小節終了番号

ループ再生の終了小節位置を指定します。 (0～999)  
CONボタンを ON に設定している場合、変更できません。

### 小説連結

ループ再生の小節連結 ON/OFFを切替えます。  
ONに設定した場合、[小節終了番号] ＝ [小説開始番号] ＋ [小説連結数] の計算式で自動設定されます。

### 小説連結数

小節連携数を指定します。(0-100)

### プレイヤー表示

プレイヤーの表示 ON/OFF を切替えます。


-----------------------------------------------------------------------------
## MusicInfoタブ

耳コピ対象のBGM情報を設定する画面です。

<img width="551" alt="image" src="https://user-images.githubusercontent.com/97685486/182549973-4b3c5cc5-afb0-4e12-8569-0ccb7dae977b.png">

### BGMファイルパス

BGMのファイルパスを指定します。（対応形式：MP3、WAV）

### BPM

ベースとなるBPMを設定します。

### BGM再生開始位置

BGMの再生開始位置(秒)を設定します。

> 小節番号０の先頭位置を基準に、入力した秒数後にBGMを再生します。

### BGM音量

BGMの音量を設定します。(0～100)

### 自由入力欄

自由入力欄です。

### イコライザ設定
BGMのイコライザを設定します。  
（左クリックで入力、右クリックで削除、左ドラッグ＆ドロップで移動）

- X軸：Hz(20～20000Hz)  
- Y軸：音量(db)の増減値(-60db～10db)

> 譜面保存時／アプリ終了時にイコライザの設定は保存されません。

#### リセット

Equalizer の入力をリセットします。

#### イコライザ

Equalizer ON/OFFを切替えます。

#### 波形

Equalizer 波形の表示ON/OFFを切替えます。　

> 未サポート

-----------------------------------------------------------------------------
## MidiMapタブ

MidiMapSet を設定する画面です。
<img width="1227" alt="image" src="https://github.com/user-attachments/assets/472fa7d8-1e6f-41b3-8238-adcd50788ad8" />

### 適用ボタン

編集中の MidiMapSet の内容を正本に反映します。

### インポートボタン

取込対象のテンプレートファイル(.dms)を選択後  
『既存⇒インポート先へのMidiMap置換』と『インポートMidiMapSet』を表示します。

<img width="785" alt="image" src="https://github.com/drumsmidi/DrumMidiEditorApp/assets/172901992/bbb3b9c5-8b29-4e08-933e-c2c2df341a4a">

> テンプレートファイル保管  
> https://github.com/drumsmidi/Template-MidiMapSet

#### 『既存⇒インポート先へのMidiMap置換』

既存MidiMapSetでノートの入力があるMidiMapの一覧を表示します。

<img width="361" alt="image" src="https://user-images.githubusercontent.com/97685486/182596477-6faa5179-c058-4048-b947-2f2706e2848b.png">

- 上部

　既存MidiMapSetでノートの入力があるMidiMap

- 下部

　インポートMidiMapSetのMidiMap（ノート置換先）  
　初期値として、置換先のMidiMap で同一のMIDI番号が設定されているアイテムの情報を設定します。

> 『インポートMidiMapSet』 のアイテムをドラッグし  
> 『既存⇒インポート先へのMidiMap置換』へドロップすることで、置換先のMidiMapを設定できます。

### エクスポートボタン

出力先ファイルを指定し、MidiMapSetのテンプレートファイル(.dms)を保存します。  
（現在編集中の情報を保存）

出力ファイルの中身（サンプル）  
```XML
<DMS VERSION="1.0">
  <MIDIMAPSET CHANNEL="0">
    <MIDIMAPGROUP DISPLAY="1" KEY="2" NAME="[Hi-Hat]" VOLUME="0" SCALEKEY="HH">
      <MIDIMAP DISPLAY="1" KEY="60" NAME="Open Edge 4" MIDI="60" VOLUME="-17" COLOR="#FF008080" SCALE="" SCALEKEYTEXT="O" />
      <MIDIMAP DISPLAY="1" KEY="26" NAME="Open Edge 3" MIDI="26" VOLUME="-17" COLOR="#FF6FFFFF" SCALE="" SCALEKEYTEXT="O" />
    </MIDIMAPGROUP>
    <MIDIMAPGROUP DISPLAY="1" KEY="3" NAME="[Snare]" VOLUME="0" SCALEKEY="SD">
      <MIDIMAP DISPLAY="1" KEY="38" NAME="Center" MIDI="38" VOLUME="0" COLOR="#FFDABE0A" SCALE="SD" SCALEKEYTEXT="" />
      <MIDIMAP DISPLAY="1" KEY="33" NAME="Edge" MIDI="33" VOLUME="0" COLOR="#FFFFFF80" SCALE="" SCALEKEYTEXT="E" />
    </MIDIMAPGROUP>
    <MIDIMAPGROUP DISPLAY="1" KEY="5" NAME="[Kick]" VOLUME="0" SCALEKEY="BD">
      <MIDIMAP DISPLAY="1" KEY="0" NAME="Left" MIDI="35" VOLUME="0" COLOR="#FFFF6767" SCALE="" SCALEKEYTEXT="" />
    </MIDIMAPGROUP>
    <PLAYER MODE="Sequence">
      <PLAYERGROUP KEY="2" X="100" Y="100" MAGNIFICATION="1" />
      <PLAYERGROUP KEY="3" X="100" Y="100" MAGNIFICATION="1" />
      <PLAYERGROUP KEY="5" X="100" Y="100" MAGNIFICATION="1" />
    </PLAYER>
    <PLAYER MODE="Simuration">
      <PLAYERGROUP KEY="2" X="74.66669" Y="377.3333" MAGNIFICATION="1" />
      <PLAYERGROUP KEY="3" X="146.66669" Y="444.66663" MAGNIFICATION="1" />
      <PLAYERGROUP KEY="5" X="233.33331" Y="518" MAGNIFICATION="1" />
   </PLAYER>
    <PLAYER MODE="ScoreType2">
      <PLAYERGROUP KEY="2" X="100" Y="100" MAGNIFICATION="1" />
      <PLAYERGROUP KEY="3" X="100" Y="100" MAGNIFICATION="1" />
      <PLAYERGROUP KEY="5" X="100" Y="100" MAGNIFICATION="1" />
    </PLAYER>
  </MIDIMAPSET>
</DMS>
```

### キー変更

既存MidiMapSetでノートの入力があるMidiMapの一覧を表示します。

- キー変更対象の MidiMap を選択し、キー変更ボタン押下

　<img width="300" alt="image" src="https://user-images.githubusercontent.com/97685486/182600270-813977ac-0201-4f21-9145-0ae11525de35.png">

- 変更後の MidiMap を選択し、OKボタン押下

　<img width="390" alt="image" src="https://user-images.githubusercontent.com/97685486/182600883-5ce81016-9fb1-417f-ac56-29f56f702e59.png">

- 確認画面が表示されるので、Yesボタン押下でキー変換実行

　<img width="239" alt="image" src="https://user-images.githubusercontent.com/97685486/182601058-8d4e27de-847a-4d58-9c66-f51dc8e3a34e.png">

- キー変換完了

　<img width="300" alt="image" src="https://user-images.githubusercontent.com/97685486/182601513-3b1805e8-ea63-4650-b8b6-4d15d69ddffb.png">

-----------------------------------------------------------------------------
## Editerタブ

<img width="967" alt="image" src="https://user-images.githubusercontent.com/97685486/182816036-aaeca4ff-41f9-45dd-9d94-70032b06513d.png">

### シート操作

#### 小節移動

選択した小節番号の位置へ移動します。  
<img width="548" alt="image" src="https://user-images.githubusercontent.com/97685486/187019775-d91de86e-5f52-472f-a742-cccd49d6ac91.png">

#### シート移動

シート移動を左クリック  
<img width="76" alt="image" src="https://user-images.githubusercontent.com/97685486/187024222-b76f4d0d-ea09-4c42-8aa5-2a331e3102e6.png">

ポップアップされた領域をマウス左でスライドさせ、シート位置を移動  
（ドラッグ開始位置を基準に、スライドした位置までの差分を等間隔で移動させます）  
<img width="330" alt="image" src="https://user-images.githubusercontent.com/97685486/187024239-492ee9b6-cae9-4055-9350-e7061e56eadd.png">

#### 小節分割数

1小節内の分割数を指定することで、指定した間隔でノート入力を行えます  
<img width="61" alt="image" src="https://user-images.githubusercontent.com/97685486/187024375-3ffc16f2-4082-42fd-aed0-9537f2f164c4.png">

128分割で入力  
<img width="464" alt="image" src="https://user-images.githubusercontent.com/97685486/187024442-e1d5b23a-e593-4a36-94c3-19e8b92ffb86.png">

16分割で入力  
<img width="462" alt="image" src="https://user-images.githubusercontent.com/97685486/187024475-dd86f769-4386-4226-9b79-054f6ff62912.png">

#### サポート線

[Measure no] の赤枠の範囲内で、マウス左でスライドさせるとスライドした分の等間隔のサポート線を表示します  
（マウス右クリックで、サポート線を解除）  
<img width="464" alt="image" src="https://user-images.githubusercontent.com/97685486/187027940-24118982-2eb0-4f37-8603-c4e6443a7ec7.png">  
<img width="461" alt="image" src="https://user-images.githubusercontent.com/97685486/187027993-58af429f-cf64-46be-b90b-8c847cd24e56.png">

#### シートの行列の幅調整

１セルのサイズを指定します  
<img width="149" alt="image" src="https://user-images.githubusercontent.com/97685486/187028042-25a222d9-1e9e-4cbc-be53-6cf47ba8eb7a.png">

<img width="581" alt="image" src="https://user-images.githubusercontent.com/97685486/187028074-e9b496f0-3da4-4dba-83ee-2aa034bdb0f7.png">

<img width="582" alt="image" src="https://user-images.githubusercontent.com/97685486/187028096-52cc1004-9020-4b8a-8e96-6cf8d4a45a04.png">

### ノート編集

#### 入力音量

ノートON の音量を指定します。  
<img width="92" alt="image" src="https://user-images.githubusercontent.com/97685486/187024558-f650b73c-c2ce-4119-8bb5-6124a6731f6a.png">

指定後、ノートONを設定すると指定した音量でノートONが登録されます。  
<img width="400" alt="image" src="https://user-images.githubusercontent.com/97685486/187024633-16bccf9c-1f1e-4ca4-911b-541f32e4e25b.png">

#### ノート入力・削除

ノートON/OFF の入力切替を行います。  
<img width="85" alt="image" src="https://user-images.githubusercontent.com/97685486/187028446-562096a0-85f9-496f-9b70-9195c24e419d.png">

指定位置で  
- マウス左クリックでノート入力  
- マウス右クリックでノート削除  
<img width="321" alt="image" src="https://user-images.githubusercontent.com/97685486/187028520-7441c914-5deb-44ea-8400-633c6d14f212.png">

> ノートOFF は試しに作成したので、使用はお勧めしません

#### 範囲選択

<img width="332" alt="image" src="https://user-images.githubusercontent.com/97685486/187028793-47882537-d61d-4bd0-942c-5688783cb607.png">

マウス左押下後、マウスを移動させることで範囲選択モードとなります。  
<img width="388" alt="image" src="https://user-images.githubusercontent.com/97685486/187028774-7b0e6b03-1dca-4e37-a2da-110b8cd19196.png">

| タイプ | イメージ |
| --- | --- |
| Normal | 入力した範囲内を選択<br><img width="174" alt="image" src="https://user-images.githubusercontent.com/97685486/187028900-c2f0f4f8-cd0f-49c5-a393-c5c917802803.png"> |
| Split | 範囲内の左 または 右側の全てを選択します<br><img width="353" alt="image" src="https://user-images.githubusercontent.com/97685486/187028913-47baeaf6-b44d-41e1-ab53-6ed11c005e37.png"> |
| All | 範囲内の行全てを選択します<br><img width="296" alt="image" src="https://user-images.githubusercontent.com/97685486/187028926-e4aa3c47-47b3-48d5-9db8-ea8803c4f404.png"> |

##### 範囲選択 機能

範囲選択を  
- マウス左 ドラッグ＆ドロップで、ノート/BPMの移動  
- マウス右クリックで範囲選択内のノート/BPMを削除  
- Ctrl + マウス左クリックで、範囲選択内のノート/BPMをコピー  
  再度、Ctrl + マウス左クリックすることで、その位置を基準にコピーした情報を貼り付けます  

> BPMは、[範囲選択にBPMノートを含める] を ON にしている場合のみ対象となります

#### MidiMap選択

選択した MidiMapGroup or MidiMap の音量を Volume 領域に表示します。
<img width="354" alt="image" src="https://user-images.githubusercontent.com/97685486/187028668-98b0954a-8f32-462d-af21-2896726f74e9.png">

#### 音量編集モード

音量編集モードを指定します。  
<img width="98" alt="image" src="https://user-images.githubusercontent.com/97685486/187024740-1fe39813-e15b-4a9f-a271-0d4c269fcaba.png">

Volumeエリアで音量編集モード別の操作で対象ノートONの音量変更を行います。  
<img width="466" alt="image" src="https://user-images.githubusercontent.com/97685486/187024823-695e3440-b44d-4081-a909-f1ba476807da.png">

| モード | イメージ |
| --- | --- |
| FreeHand | フリーハンド<br><img width="334" alt="image" src="https://user-images.githubusercontent.com/97685486/187027126-c96d797c-196a-4cff-9398-2c432a2602e0.png"> |
| StraightLine | 直線<br><img width="311" alt="image" src="https://user-images.githubusercontent.com/97685486/187027136-6506b6b7-dbe4-4eb7-b6aa-aa610d8a245e.png"> |
| Curve1 | ベジエ曲線<br><img width="299" alt="image" src="https://user-images.githubusercontent.com/97685486/187027154-262f7aa0-3189-4550-b1b4-2fa86580f6a7.png"> |
| Curve2 | ベジエ曲線<br><img width="293" alt="image" src="https://user-images.githubusercontent.com/97685486/187027169-e21fa6a4-4302-4682-ab98-528656a1d671.png"> }
| Curve3 | ベジエ曲線<br><img width="292" alt="image" src="https://user-images.githubusercontent.com/97685486/187027189-866eeaff-fd2f-4309-baac-9b5977866b02.png"> |
| Curve4 | ベジエ曲線<br><img width="293" alt="image" src="https://user-images.githubusercontent.com/97685486/187027199-6fa33e8e-37aa-4807-8160-09e88e5917f1.png"> |
| UpDown | 始点と終点間の音量差を加算<br><img width="301" alt="image" src="https://user-images.githubusercontent.com/97685486/187027210-e38d0546-8ab9-414d-b5cd-2a7627e2855a.png"><br><img width="299" alt="image" src="https://user-images.githubusercontent.com/97685486/187027220-15da385b-0d34-46ed-9e19-95a1e004fe57.png"> |
| IntonationHL | 始点と終点間の音量差を加算<br>但し、始点より高い音量は＋、始点より低い音量はーで加算<br><img width="300" alt="image" src="https://user-images.githubusercontent.com/97685486/187027237-025d4d3c-5b81-40df-b9c3-d969d015b348.png"><br><img width="316" alt="image" src="https://user-images.githubusercontent.com/97685486/187027247-4f8c8ac5-2591-422b-8dc4-c1fa165a3089.png"> |
| IntonationH | 始点と終点間の音量差を加算<br>但し、始点より高い音量のみ加算<br><img width="298" alt="image" src="https://user-images.githubusercontent.com/97685486/187027261-00ef1348-7115-4ea6-8335-803bac7e6be5.png"><br><img width="299" alt="image" src="https://user-images.githubusercontent.com/97685486/187027274-35f144f8-5548-497a-bbe0-f951edb7c3e7.png"> |
| IntonationL | 始点と終点間の音量差を加算<br>但し、始点より低い音量のみ加算<br><img width="297" alt="image" src="https://user-images.githubusercontent.com/97685486/187027284-a254a2fe-a62b-4c23-af1e-89a67212e009.png"><br><img width="310" alt="image" src="https://user-images.githubusercontent.com/97685486/187027294-ab448b9a-a8aa-4bc3-8c3f-7bc362a3f5d8.png"> |

#### Undo/Redo

ノート/BPM 入力の Undo/Redo を実行します。  

> 検証不十分：不具合あり

#### WaveForm

周波数解析 試作  

> 自動採譜とか自作できないかなと思って、試作した機能。非推奨

<img width="960" alt="image" src="https://user-images.githubusercontent.com/97685486/187029266-9560fad2-42e9-4d0c-a0e0-94943a8fd037.png">


-----------------------------------------------------------------------------
## Scoreタブ

１画面内で全小節の情報を表示します。  
<img width="675" alt="image" src="https://user-images.githubusercontent.com/97685486/182563407-1b11fea2-b486-431a-9226-4c7681a75ed6.png">

### 音量によるノートサイズ変更  

音量による ノートサイズ縮小 ON/OFF 切替

### 音量０のノート表示  

音量=0 のノート表示 ON/OFF 切替

### ノート高さ  

ノートサイズの高さ

### ノート横幅  

ノートサイズの横幅

> 現時点では、機能拡張予定はありません

-----------------------------------------------------------------------------
## Player表示

Editで入力したノート情報のシーケンスを表示する機能です。

### プレイヤー表示

メニューバーの[プレイヤー表示スイッチ]押下で、プレイヤーの表示／非表示を切替えます。  
また、プレイヤーウィンドウを右クリックすることでも非表示に切替えます。

<img width="805" alt="image" src="https://user-images.githubusercontent.com/97685486/184022213-e63d7bf6-1c98-46b0-85dd-163465f5029c.png">

### プレイヤー表示位置移動

プレイヤーをマウス左でドラッグすることでプレイヤーの位置を移動できます。

> アプリ内の範囲でのみ移動可能です。（別ウィンドウの作成は未対応です）

### プレイヤー表示タイプ

現在、３種類作成していますが、ScoreType2 以外はメンテしてません。

| 種類 | 概要 |
| --- | --- |
| ScoreType2 | 譜面表示を目標とした作り<br><img width="473" alt="image" src="https://github.com/user-attachments/assets/10267db4-4be3-4d44-9c79-a506fb33932c" /> |
| Sequence | ノートを右から左へ または 上から下へ流す作り<br><img width="359" alt="image" src="https://user-images.githubusercontent.com/97685486/184024285-772c87ce-20ac-41f3-9b77-4c3312f86b1a.png"><img width="245" alt="image" src="https://user-images.githubusercontent.com/97685486/184024335-dd5aa54f-71f7-45d4-9743-955e42b9b0a1.png"> |
| Simuration | ドラムを上から見たような構図の作り<br><img width="479" alt="image" src="https://user-images.githubusercontent.com/97685486/184236317-336e53e9-2d6c-42ed-952f-1da6ab5ab350.png"> |

-----------------------------------------------------------------------------
