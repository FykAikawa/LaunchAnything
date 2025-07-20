using System;
using System.Collections.ObjectModel;
using erlauncher.Models;

namespace erlauncher.Services
{
    /// <summary>
    /// グループ操作のサービス実装
    /// </summary>
    public class GroupService : IGroupService
    {
        /// <summary>
        /// グループ一覧を取得します。
        /// </summary>
        /// <returns>FolderInfo のコレクション</returns>
        public ObservableCollection<FolderInfo> GetGroups()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// グループを追加します。
        /// </summary>
        /// <param name="folder">追加する FolderInfo</param>
        public void AddGroup(FolderInfo folder)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// グループを削除します。
        /// </summary>
        /// <param name="id">削除するグループの ID</param>
        public void RemoveGroup(int id)
        {
            throw new NotImplementedException();
        }
    }
}