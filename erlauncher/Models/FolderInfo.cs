using System.Collections.Generic;

namespace erlauncher.Models
{
    /// <summary>
    /// ゲームグループ（フォルダ）を表すモデル
    /// </summary>
    public class FolderInfo
    {
        /// <summary>グループ名</summary>
        public string Name { get; set; }

        /// <summary>所属するゲーム情報リスト</summary>
        public List<GameInfo> GameList { get; set; } = new List<GameInfo>();

        /// <summary>
        /// 指定された名前でインスタンスを生成するコンストラクタ
        /// </summary>
        public FolderInfo(string name)
        {
            Name = name;
        }

        /// <summary>デシリアライズ用パラメータレスコンストラクタ</summary>
        public FolderInfo() { }

        /// <summary>GameInfo インスタンスを追加する</summary>
        public void AddGame(GameInfo game)
        {
            GameList.Add(game);
        }

        /// <summary>名前とパスから新規 GameInfo を生成して追加する</summary>
        public void AddGame(string displayName, string path)
        {
            var game = new GameInfo(displayName, path);
            GameList.Add(game);
        }
    }
}