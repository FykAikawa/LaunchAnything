using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using erlauncher.Services;

namespace erlauncher.ViewModels
{
    /// <summary>
    /// ゲーム情報作成用 ViewModel
    /// </summary>
    public class CreateGameInfoViewModel : ViewModelBase
    {
        private readonly IGameInfoService _gameInfoService;

        private string _title;
        private string _executablePath;
        private string _description;

        /// <summary>
        /// ゲームタイトル
        /// </summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        /// <summary>
        /// 実行ファイルパス
        /// </summary>
        public string ExecutablePath
        {
            get => _executablePath;
            set => Set(ref _executablePath, value);
        }

        /// <summary>
        /// 説明
        /// </summary>
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        /// <summary>
        /// ゲーム情報作成コマンド
        /// </summary>
        public ICommand CreateGameInfoCommand { get; }

        /// <summary>
        /// キャンセルコマンド
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gameInfoService">ゲーム情報サービス</param>
        public CreateGameInfoViewModel(IGameInfoService gameInfoService)
        {
            _gameInfoService = gameInfoService;
            CreateGameInfoCommand = new RelayCommand(OnCreateGameInfo);
            CancelCommand = new RelayCommand(OnCancel);
        }

        /// <summary>
        /// ゲーム情報作成処理
        /// </summary>
        private void OnCreateGameInfo()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// キャンセル処理
        /// </summary>
        private void OnCancel()
        {
            throw new NotImplementedException();
        }
    }
}