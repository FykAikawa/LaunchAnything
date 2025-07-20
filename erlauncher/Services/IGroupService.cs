using System.Collections.ObjectModel;
using erlauncher.Models;

namespace erlauncher.Services
{
    /// <summary>
    /// グループ操作のサービスインターフェース
    /// </summary>
    public interface IGroupService
    {
        /// <summary>
        /// グループ一覧を取得します。
        /// </summary>
        /// <returns>FolderInfo のコレクション</returns>
        ObservableCollection<FolderInfo> GetGroups();

        /// <summary>
        /// グループを追加します。
        /// </summary>
        /// <param name="folder">追加する FolderInfo</param>
        void AddGroup(FolderInfo folder);

        /// <summary>
        /// グループを削除します。
        /// </summary>
        /// <param name="id">削除するグループの ID</param>
        void RemoveGroup(int id);
    }
}