using System;
using System.ComponentModel;

namespace erlauncher.Models
{
    /// <summary>
    /// ツイート情報を保持するモデル
    /// </summary>
    public class TweetInfo : INotifyPropertyChanged
    {
        private int _id;
        private string _content;
        private string _imagePath;
        private DateTime _postedAt;

        /// <summary>
        /// 一意の識別子
        /// </summary>
        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        /// <summary>
        /// ツイート内容
        /// </summary>
        public string Content
        {
            get => _content;
            set
            {
                if (_content != value)
                {
                    _content = value;
                    OnPropertyChanged(nameof(Content));
                }
            }
        }

        /// <summary>
        /// 画像パス
        /// </summary>
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (_imagePath != value)
                {
                    _imagePath = value;
                    OnPropertyChanged(nameof(ImagePath));
                }
            }
        }

        /// <summary>
        /// 投稿日時
        /// </summary>
        public DateTime PostedAt
        {
            get => _postedAt;
            set
            {
                if (_postedAt != value)
                {
                    _postedAt = value;
                    OnPropertyChanged(nameof(PostedAt));
                }
            }
        }

        /// <summary>
        /// 初期化用コンストラクタ
        /// </summary>
        public TweetInfo(int id, string content, string imagePath, DateTime postedAt)
        {
            _id = id;
            _content = content;
            _imagePath = imagePath;
            _postedAt = postedAt;
        }

        /// <summary>
        /// プロパティ変更通知イベント
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティ変更通知を発生させる
        /// </summary>
        /// <param name="propertyName">変更されたプロパティ名</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}