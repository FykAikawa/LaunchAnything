using System;
using System.Collections.ObjectModel;
using erlauncher.Models;

namespace erlauncher.Services
{
    /// <summary>
    /// ゲーム情報操作のサービス実装
    /// </summary>
    public class GameInfoService : IGameInfoService
    {
        /// <summary>
        /// ゲーム情報一覧を取得します。
        /// </summary>
        /// <returns>GameInfo のコレクション</returns>
        public ObservableCollection<GameInfo> GetGameInfos()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ゲーム情報を作成します。
        /// </summary>
        /// <param name="gameInfo">作成する GameInfo</param>
        public void CreateGameInfo(GameInfo gameInfo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ゲーム情報を更新します。
        /// </summary>
        /// <param name="gameInfo">更新する GameInfo</param>
        public void UpdateGameInfo(GameInfo gameInfo)
        {
            throw new NotImplementedException();
        }
    }
}