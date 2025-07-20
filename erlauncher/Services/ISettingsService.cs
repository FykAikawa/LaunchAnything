using erlauncher.Models;

namespace erlauncher.Services
{
    /// <summary>
    /// 設定操作のサービスインターフェース
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// 設定を読み込みます。
        /// </summary>
        /// <returns>Settings オブジェクト</returns>
        Settings LoadSettings();

        /// <summary>
        /// 設定を保存します。
        /// </summary>
        /// <param name="settings">保存する Settings</param>
        void SaveSettings(Settings settings);
    }
}