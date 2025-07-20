using System;
using System.Collections.ObjectModel;
using erlauncher.Models;

namespace erlauncher.Services
{
    /// <summary>
    /// スクリーンショット操作のサービスインターフェース
    /// </summary>
    public interface IScreenshotService
    {
        /// <summary>
        /// スクリーンショット一覧を取得します。
        /// </summary>
        /// <returns>ScreenShotInfo のコレクション</returns>
        ObservableCollection<ScreenShotInfo> GetScreenshots();

        /// <summary>
        /// スクリーンショットをキャプチャします。
        /// </summary>
        void CaptureScreenshot(IntPtr hwnd);

        /// <summary>
        /// スクリーンショットをキャプチャします（ゲーム情報を指定）。
        /// </summary>
        /// <param name="hwnd">ウィンドウハンドル</param>
        /// <param name="gameInfo">ゲーム情報</param>
        void CaptureScreenshot(IntPtr hwnd, erlauncher.Models.GameInfo gameInfo);

        /// <summary>
        /// スクリーンショットを削除します。
        /// </summary>
        /// <param name="id">削除するスクリーンショットの ID</param>
        void DeleteScreenshot(int id);
    }
}