using System;

namespace erlauncher.Models
{
    /// <summary>
    /// ゲームエントリ情報を保持するモデル
    /// </summary>
    public class GameInfo
    {
        /// <summary>一意の識別子</summary>
        public int Id { get; set; }

        /// <summary>画面に表示する名前</summary>
        public string DisplayName { get; set; }

        /// <summary>実行ファイルなどへのパス</summary>
        public string Path { get; set; }

        /// <summary>サムネイル画像のパス</summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 新しい GameInfo を生成するコンストラクタ
        /// </summary>
        public GameInfo(string displayName, string path)
        {
            DisplayName = displayName;
            Path = path;
        }

        /// <summary>デシリアライズ用パラメータレスコンストラクタ</summary>
        public GameInfo() { }
    }
}