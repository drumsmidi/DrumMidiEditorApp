# 実行環境  
<img width="284" alt="image" src="https://user-images.githubusercontent.com/97685486/182602780-22433054-0a64-4e2d-a41b-3394893d5f53.png">  

# 使用ライブラリ（WinUI3対応）  
| 使用パッケージ | メモ |
| --- | --- |
| Microsoft.Windows.SDK.BuildTools | |
| Microsoft.WindowsAppSDK | |
| Microsoft.Graphics.Win2D | Canvasの描画に使用 |
| Microsoft.ML | 機械学習に使用（活用できてません） |
| NAudio | BGM再生、周波数解析に使用 |
| OpenCvSharp4.Windows | MP4の動画出力に使用 |

# Actions によるビルド設定

## 署名の作成
[証明書の選択]より、証明書の作成を行う。
<img width="854" alt="image" src="https://github.com/drumsmidi/DrumMidieditorApp/assets/172901992/ccb93cb9-a3d2-4632-b317-011920e79260">

## Githubへの署名登録  
1. Visual Studioで［ツール］－［コマンドライン］－［開発者用 PowerShell］をクリックしてPowerShellを起動  

```BAT
cd '.\DrumMidiEditorApp\DrumMidiEditorApp (Package)\'

$pfx_cert = Get-Content '.\DrumMidiEditorApp (Package)_TemporaryKey.pfx' -Encoding Byte
[System.Convert]::ToBase64String($pfx_cert) | Out-File 'DrumMidiEditorApp (Package)_TemporaryKey.txt'
```

2. Githubの [Settings] - [Secrets] - [Actions] の　[New repository secret]ボタンより下記２つの設定を登録  

1個目  
- Name：「Base64_Encoded_Pfx」  
- Value：1で作成した「DrumMidiEditorApp (Package)_TemporaryKey.txt」の中身  

２個目  
- Name：「Pfx_Key」  
- Value：署名のパスワード  

## Workflowの追加  
<img width="275" alt="image" src="https://user-images.githubusercontent.com/97685486/182817247-828b0966-806b-43a8-bbe0-a1b1c8845b32.png">  

参考：WinUI 3 アプリの継続的インテグレーションをセットアップする  
https://docs.microsoft.com/ja-jp/windows/apps/package-and-deploy/ci-for-winui3?pivots=winui3-packaged-csharp  

設定メモ  
```yaml
    strategy:
      matrix:
        configuration: [Release]
        #configuration: [Debug, Release]
        platform: [x64]

    env:
      Solution_Name: DrumMidiEditorApp.sln
      Test_Project_Path: none
      Wap_Project_Directory: DrumMidiEditorApp\DrumMidiEditorApp (Package)
      Wap_Project_Path: DrumMidiEditorApp\DrumMidiEditorApp (Package)\DrumMidiEditorApp (Package).wapproj

    # Install the .NET Core workload
    - name: Install .NET Core
      uses: actions/setup-dotnet@v2
      with:
        dotnet-version: 8.0.x

    # Execute all unit tests in the solution
    # - name: Execute unit tests
    #   run: dotnet test

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
```

