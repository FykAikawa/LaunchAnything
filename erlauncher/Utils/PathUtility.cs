using System;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;

namespace erlauncher.Utils
{
    /// <summary>
    /// パス操作のユーティリティクラス
    /// </summary>
    public static class PathUtility
    {
        /// <summary>
        /// ゲーム用スクリーンショットフォルダのパスを取得します
        /// </summary>
        /// <param name="gameDisplayName">ゲームの表示名</param>
        /// <returns>スクリーンショットフォルダのパス</returns>
        public static string GetGameScreenshotFolderPath(string gameDisplayName)
        {
            // App.configからスクリーンショットベースディレクトリを取得
            string baseScreenshotDir = ConfigurationManager.AppSettings["ScreenShotDir"];
            
            // 設定値が存在しない場合はデフォルトパスを使用
            if (string.IsNullOrEmpty(baseScreenshotDir))
            {
                baseScreenshotDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
            }
            
            // ゲーム名をファイルシステム安全な名前に変換
            string safeFolderName = SanitizeGameNameForFileSystem(gameDisplayName);
            
            // フルパスを生成
            string gameScreenshotPath = Path.Combine(baseScreenshotDir, safeFolderName);
            
            // フォルダが存在しない場合は作成
            if (!Directory.Exists(gameScreenshotPath))
            {
                Directory.CreateDirectory(gameScreenshotPath);
            }
            
            return gameScreenshotPath;
        }
        
        /// <summary>
        /// ゲーム名をファイルシステム安全な名前に変換します
        /// </summary>
        /// <param name="gameName">ゲーム名</param>
        /// <returns>ファイルシステム安全な名前</returns>
        public static string SanitizeGameNameForFileSystem(string gameName)
        {
            if (string.IsNullOrEmpty(gameName))
            {
                return "UnknownGame";
            }
            
            // ファイルシステムで使用できない文字を除去または置換
            char[] invalidChars = Path.GetInvalidFileNameChars();
            string sanitized = gameName;
            
            foreach (char invalidChar in invalidChars)
            {
                sanitized = sanitized.Replace(invalidChar, '_');
            }
            
            // 追加で問題となる可能性のある文字を置換
            sanitized = sanitized.Replace(':', '_')
                                 .Replace('*', '_')
                                 .Replace('?', '_')
                                 .Replace('"', '_')
                                 .Replace('<', '_')
                                 .Replace('>', '_')
                                 .Replace('|', '_');
            
            // 連続するアンダースコアを単一に変換
            sanitized = Regex.Replace(sanitized, "_+", "_");
            
            // 先頭・末尾のアンダースコアを除去
            sanitized = sanitized.Trim('_');
            
            // 空文字列になった場合のフォールバック
            if (string.IsNullOrEmpty(sanitized))
            {
                sanitized = "UnknownGame";
            }
            
            return sanitized;
        }
        
        /// <summary>
        /// スクリーンショットファイルのパスを生成します
        /// </summary>
        /// <param name="gameDisplayName">ゲームの表示名</param>
        /// <param name="timestamp">タイムスタンプ（nullの場合は現在時刻を使用）</param>
        /// <returns>スクリーンショットファイルのフルパス</returns>
        public static string GenerateScreenshotFilePath(string gameDisplayName, DateTime? timestamp = null)
        {
            string folderPath = GetGameScreenshotFolderPath(gameDisplayName);
            DateTime fileTimestamp = timestamp ?? DateTime.Now;
            string fileName = $"screenshot_{fileTimestamp:yyyyMMdd_HHmmss}.png";
            return Path.Combine(folderPath, fileName);
        }
    }
}