using System.Collections.ObjectModel;
using erlauncher.Models;

namespace erlauncher.Services
{
    /// <summary>
    /// ゲーム情報操作のサービスインターフェース
    /// </summary>
    public interface IGameInfoService
    {
        /// <summary>
        /// ゲーム情報一覧を取得します。
        /// </summary>
        /// <returns>GameInfo のコレクション</returns>
        ObservableCollection<GameInfo> GetGameInfos();

        /// <summary>
        /// ゲーム情報を作成します。
        /// </summary>
        /// <param name="gameInfo">作成する GameInfo</param>
        void CreateGameInfo(GameInfo gameInfo);

        /// <summary>
        /// ゲーム情報を更新します。
        /// </summary>
        /// <param name="gameInfo">更新する GameInfo</param>
        void UpdateGameInfo(GameInfo gameInfo);
    }
}