using System;
using System.ComponentModel;

namespace erlauncher.Models
{
    /// <summary>
    /// アプリ設定を保持するモデル
    /// </summary>
    public class Settings : INotifyPropertyChanged
    {
        private string _defaultSavePath;
        private bool _autoTweetEnabled;
        private string _shortcutKey;

        /// <summary>
        /// デフォルト保存パス
        /// </summary>
        public string DefaultSavePath
        {
            get => _defaultSavePath;
            set
            {
                if (_defaultSavePath != value)
                {
                    _defaultSavePath = value;
                    OnPropertyChanged(nameof(DefaultSavePath));
                }
            }
        }

        /// <summary>
        /// 自動ツイートの有効化フラグ
        /// </summary>
        public bool AutoTweetEnabled
        {
            get => _autoTweetEnabled;
            set
            {
                if (_autoTweetEnabled != value)
                {
                    _autoTweetEnabled = value;
                    OnPropertyChanged(nameof(AutoTweetEnabled));
                }
            }
        }

        /// <summary>
        /// ショートカットキー
        /// </summary>
        public string ShortcutKey
        {
            get => _shortcutKey;
            set
            {
                if (_shortcutKey != value)
                {
                    _shortcutKey = value;
                    OnPropertyChanged(nameof(ShortcutKey));
                }
            }
        }

        /// <summary>
        /// 初期化用コンストラクタ
        /// </summary>
        public Settings(string defaultSavePath, bool autoTweetEnabled, string shortcutKey)
        {
            _defaultSavePath = defaultSavePath;
            _autoTweetEnabled = autoTweetEnabled;
            _shortcutKey = shortcutKey;
        }

        /// <summary>
        /// デシリアライズ用パラメータレスコンストラクタ
        /// </summary>
        public Settings() { }

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