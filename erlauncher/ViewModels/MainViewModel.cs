using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using erlauncher.Models;
using erlauncher.Services;
using System.Runtime.InteropServices; // 追加

namespace erlauncher.ViewModels
{
    /// <summary>
    /// アプリのメイン画面用 ViewModel
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IGroupService _groupService;
        private readonly IGameInfoService _gameInfoService;
        private readonly IScreenshotService _screenshotService;
        private readonly ISettingsService _settingsService;

        /// <summary>
        /// フォルダグループ一覧
        /// </summary>
        public ObservableCollection<FolderInfo> Groups { get; } = new ObservableCollection<FolderInfo>();

        /// <summary>
        /// ゲーム情報一覧
        /// </summary>
        public ObservableCollection<GameInfo> GameInfos { get; } = new ObservableCollection<GameInfo>();

        /// <summary>
        /// スクリーンショット一覧
        /// </summary>
        public ObservableCollection<ScreenShotInfo> Screenshots { get; } = new ObservableCollection<ScreenShotInfo>();

        private Settings _appSettings;
        /// <summary>
        /// アプリ設定
        /// </summary>
        public Settings AppSettings
        {
            get => _appSettings;
            set => Set(ref _appSettings, value);
        }

        /// <summary>
        /// データ読み込みコマンド
        /// </summary>
        public ICommand LoadDataCommand { get; }

        /// <summary>
        /// スクリーンショットキャプチャコマンド
        /// </summary>
        public ICommand CaptureScreenshotCommand { get; }

        /// <summary>
        /// 設定保存コマンド
        /// </summary>
        public ICommand SaveSettingsCommand { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="groupService">グループサービス</param>
        /// <param name="gameInfoService">ゲーム情報サービス</param>
        /// <param name="screenshotService">スクリーンショットサービス</param>
        /// <param name="settingsService">設定サービス</param>
        public MainViewModel(
            IGroupService groupService,
            IGameInfoService gameInfoService,
            IScreenshotService screenshotService,
            ISettingsService settingsService)
        {
            _groupService = groupService;
            _gameInfoService = gameInfoService;
            _screenshotService = screenshotService;
            _settingsService = settingsService;

            LoadDataCommand = new RelayCommand(OnLoadData);
            CaptureScreenshotCommand = new RelayCommand(OnCaptureScreenshot);
            SaveSettingsCommand = new RelayCommand(OnSaveSettings);
        }

        /// <summary>
        /// データ読み込み処理
        /// </summary>
        private void OnLoadData()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// スクリーンショットキャプチャ処理
        /// </summary>
        public event EventHandler<string> ShowMessageRequested;

        /// <summary>
        /// スクリーンショットキャプチャ処理
        /// </summary>
        private void OnCaptureScreenshot()
        {
            ShowMessageRequested?.Invoke(this, "OnCaptureScreenshot called.");
            // 現在アクティブなウィンドウのハンドルを取得
            IntPtr activeWindowHandle = GetForegroundWindow();
            _screenshotService.CaptureScreenshot(activeWindowHandle);
        }

        /// <summary>
        /// 設定保存処理
        /// </summary>
        private void OnSaveSettings()
        {
            throw new NotImplementedException();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
    }
}