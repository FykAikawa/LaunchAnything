using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using erlauncher.Services;

namespace erlauncher.ViewModels
{
    /// <summary>
    /// ツイート用 ViewModel
    /// </summary>
    public class TweetViewModel : ViewModelBase
    {
        private readonly ITweetService _tweetService;

        private string _tweetContent;
        private string _imagePath;

        /// <summary>
        /// ツイート内容
        /// </summary>
        public string TweetContent
        {
            get => _tweetContent;
            set => Set(ref _tweetContent, value);
        }

        /// <summary>
        /// 画像パス
        /// </summary>
        public string ImagePath
        {
            get => _imagePath;
            set => Set(ref _imagePath, value);
        }

        /// <summary>
        /// ツイート送信コマンド
        /// </summary>
        public ICommand SendTweetCommand { get; }

        /// <summary>
        /// キャンセルコマンド
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tweetService">ツイートサービス</param>
        public TweetViewModel(ITweetService tweetService)
        {
            _tweetService = tweetService;
            SendTweetCommand = new RelayCommand(OnSendTweet);
            CancelCommand = new RelayCommand(OnCancel);
        }

        /// <summary>
        /// ツイート送信処理
        /// </summary>
        private void OnSendTweet()
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