using System;
using System.ComponentModel;

namespace erlauncher.Models
{
    /// <summary>
    /// スクリーンショット情報を保持するモデル
    /// </summary>
    public class ScreenShotInfo : INotifyPropertyChanged
    {
        private int _id;
        private string _filePath;
        private DateTime _createdAt;

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
        /// ファイルパス
        /// </summary>
        public string FilePath
        {
            get => _filePath;
            set
            {
                if (_filePath != value)
                {
                    _filePath = value;
                    OnPropertyChanged(nameof(FilePath));
                }
            }
        }

        /// <summary>
        /// 作成日時
        /// </summary>
        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                if (_createdAt != value)
                {
                    _createdAt = value;
                    OnPropertyChanged(nameof(CreatedAt));
                }
            }
        }

        /// <summary>
        /// 初期化用コンストラクタ
        /// </summary>
        public ScreenShotInfo(int id, string filePath, DateTime createdAt)
        {
            _id = id;
            _filePath = filePath;
            _createdAt = createdAt;
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