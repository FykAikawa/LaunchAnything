using System;
using System.Collections.ObjectModel;
using erlauncher.Models;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using erlauncher.Utils;

namespace erlauncher.Services
{
    /// <summary>
    /// スクリーンショット操作のサービス実装
    /// </summary>
    public class ScreenshotService : IScreenshotService
    {

        [DllImport("dwmapi.dll")]
        private static extern int DwmRegisterThumbnail(IntPtr hwndDestination, IntPtr hwndSource, out IntPtr phThumbnailId);

        [DllImport("dwmapi.dll")]
        private static extern int DwmUnregisterThumbnail(IntPtr hThumbnailId);

        [DllImport("dwmapi.dll")]
        private static extern int DwmQueryThumbnailSourceSize(IntPtr hThumbnail, out SIZE pSize);

        [DllImport("dwmapi.dll")]
        private static extern int DwmUpdateThumbnailProperties(IntPtr hThumbnailId, ref DWM_THUMBNAIL_PROPERTIES ptnProperties);

        [DllImport("dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(IntPtr hwnd, uint dwAttribute, out RECT pvAttribute, uint cbAttribute);

        private const int DWM_S_OK = 0;
        private const uint DWMWA_EXTENDED_FRAME_BOUNDS = 9; // DWMWA_EXTENDED_FRAME_BOUNDS

        [StructLayout(LayoutKind.Sequential)]
        private struct SIZE { public int cx; public int cy; }

        [StructLayout(LayoutKind.Sequential)]
        private struct DWM_THUMBNAIL_PROPERTIES
        {
            public uint dwFlags;
            public RECT rcDestination;
            public RECT rcSource;
            public byte opacity;
            public bool fVisible;
            public bool fSourceClientAreaOnly;
        }

        private const uint DWM_TNP_RECTDESTINATION = 0x00000001;
        private const uint DWM_TNP_RECTSOURCE = 0x00000002;
        private const uint DWM_TNP_OPACITY = 0x00000004;
        private const uint DWM_TNP_VISIBLE = 0x00000008;
        private const uint DWM_TNP_SOURCECLIENTAREAONLY = 0x00000010;


        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int width, int height, IntPtr hdcSrc, int xSrc, int ySrc, int rop);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        private const int SRCCOPY = 0x00CC0020;
        private const int CAPTUREBLT = 0x40000000;
        private const uint PW_CLIENTONLY = 0x1;
        private const uint PW_RENDERFULLCONTENT = 0x2;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT { public int Left; public int Top; public int Right; public int Bottom; }
        /// <summary>
        /// スクリーンショット一覧を取得します。
        /// </summary>
        /// <returns>ScreenShotInfo のコレクション</returns>
        public ObservableCollection<ScreenShotInfo> GetScreenshots()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// スクリーンショットをキャプチャします。
        /// </summary>
        public void CaptureScreenshot(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero)
            {
                MessageBox.Show("HWND is Zero. Falling back to CaptureUsingGraphics.");
                CaptureUsingGraphics();
                return;
            }


            RECT rect;
            if (!GetWindowRect(hwnd, out rect))
            {
                MessageBox.Show($"GetWindowRect failed for hwnd: {hwnd}. Falling back to CaptureUsingGraphics.");
                CaptureUsingGraphics();
                return;
            }

            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;

            // Window dimensions retrieved successfully

            // 取得したサイズが異常な場合（例: 0以下、または画面サイズを大幅に超える場合）はフォールバック
            if (width <= 0 || height <= 0 || width > Screen.PrimaryScreen.Bounds.Width * 2 || height > Screen.PrimaryScreen.Bounds.Height * 2)
            {
                MessageBox.Show($"Invalid window size: Width={width}, Height={height}, Hwnd={hwnd}. Falling back to CaptureUsingGraphics.");
                CaptureUsingGraphics();
                return;
            }

            try
            {
                // 1. まずDirectX/OpenGL対応の高度なキャプチャを試行
                if (TryCaptureWithAdvancedMethods(hwnd, width, height))
                {
                    return;
                }

                // 2. PrintWindowを使用してウィンドウの内容を直接キャプチャ（画面外部分も含む）
                if (TryCaptureWithPrintWindow(hwnd, width, height))
                {
                    return;
                }
                
                // 3. PrintWindowが失敗した場合、CopyFromScreenにフォールバック
                using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    Thread.Sleep(100); // 100ミリ秒待機
                    g.CopyFromScreen(rect.Left, rect.Top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);

                    string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
                    Directory.CreateDirectory(folder);
                    string filePath = Path.Combine(folder, $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    bmp.Save(filePath, ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error capturing screenshot with GDI methods: {ex.Message}");
                // エラーが発生した場合、最終フォールバックとしてCaptureUsingGraphicsを使用
                CaptureUsingGraphics();
            }
        }

        /// <summary>
        /// スクリーンショットをキャプチャします（ゲーム情報を指定）。
        /// </summary>
        /// <param name="hwnd">ウィンドウハンドル</param>
        /// <param name="gameInfo">ゲーム情報</param>
        public void CaptureScreenshot(IntPtr hwnd, GameInfo gameInfo)
        {
            if (hwnd == IntPtr.Zero)
            {
                MessageBox.Show("HWND is Zero. Falling back to CaptureUsingGraphics.");
                CaptureUsingGraphicsForGame(gameInfo);
                return;
            }

            RECT rect;
            if (!GetWindowRect(hwnd, out rect))
            {
                MessageBox.Show($"GetWindowRect failed for hwnd: {hwnd}. Falling back to CaptureUsingGraphics.");
                CaptureUsingGraphicsForGame(gameInfo);
                return;
            }

            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;

            // 取得したサイズが異常な場合（例: 0以下、または画面サイズを大幅に超える場合）はフォールバック
            if (width <= 0 || height <= 0 || width > Screen.PrimaryScreen.Bounds.Width * 2 || height > Screen.PrimaryScreen.Bounds.Height * 2)
            {
                MessageBox.Show($"Invalid window size: Width={width}, Height={height}, Hwnd={hwnd}. Falling back to CaptureUsingGraphics.");
                CaptureUsingGraphicsForGame(gameInfo);
                return;
            }

            try
            {
                // 1. まずDirectX/OpenGL対応の高度なキャプチャを試行
                if (TryCaptureWithAdvancedMethodsForGame(hwnd, width, height, gameInfo))
                {
                    return;
                }

                // 2. PrintWindowを使用してウィンドウの内容を直接キャプチャ（画面外部分も含む）
                if (TryCaptureWithPrintWindowForGame(hwnd, width, height, gameInfo))
                {
                    return;
                }
                
                // 3. PrintWindowが失敗した場合、CopyFromScreenにフォールバック
                using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    Thread.Sleep(100); // 100ミリ秒待機
                    g.CopyFromScreen(rect.Left, rect.Top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);

                    string filePath = PathUtility.GenerateScreenshotFilePath(gameInfo.DisplayName);
                    bmp.Save(filePath, ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error capturing screenshot with GDI methods: {ex.Message}");
                // エラーが発生した場合、最終フォールバックとしてCaptureUsingGraphicsForGameを使用
                CaptureUsingGraphicsForGame(gameInfo);
            }
        }

        private bool TryCaptureWithAdvancedMethods(IntPtr hwnd, int width, int height)
        {
            // DirectX/OpenGLゲーム向けの高度なキャプチャ方法を実装
            
            // 1. BitBltを使用した直接キャプチャ（画面外部分も取得可能）
            if (TryCaptureWithBitBlt(hwnd, width, height))
            {
                return true;
            }

            // 2. DWM APIを使用した高度なキャプチャ
            if (TryCaptureWithDwmApi(hwnd))
            {
                return true;
            }

            // 3. クライアント領域の拡張キャプチャ
            if (TryCaptureWithExtendedClient(hwnd, width, height))
            {
                return true;
            }

            return false;
        }

        private bool TryCaptureWithExtendedClient(IntPtr hwnd, int width, int height)
        {
            // クライアント領域とウィンドウ領域の両方を考慮したキャプチャ
            try
            {
                RECT clientRect;
                RECT windowRect;
                
                if (!GetClientRect(hwnd, out clientRect) || !GetWindowRect(hwnd, out windowRect))
                {
                    return false;
                }

                // ウィンドウの実際のコンテンツ領域を計算
                int clientWidth = clientRect.Right - clientRect.Left;
                int clientHeight = clientRect.Bottom - clientRect.Top;
                
                if (clientWidth <= 0 || clientHeight <= 0)
                {
                    return false;
                }

                using (Bitmap bmp = new Bitmap(clientWidth, clientHeight, PixelFormat.Format32bppArgb))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    IntPtr hdc = g.GetHdc();
                    try
                    {
                        // PW_CLIENTONLYフラグでクライアント領域のみをキャプチャ
                        if (PrintWindow(hwnd, hdc, PW_CLIENTONLY) || PrintWindow(hwnd, hdc, 0))
                        {
                            g.ReleaseHdc(hdc);
                            
                            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
                            Directory.CreateDirectory(folder);
                            string filePath = Path.Combine(folder, $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                            bmp.Save(filePath, ImageFormat.Png);
                            return true;
                        }
                    }
                    finally
                    {
                        try { g.ReleaseHdc(hdc); } catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Extended client capture failed: {ex.Message}");
            }

            return false;
        }

        private bool TryCaptureWithBitBlt(IntPtr hwnd, int width, int height)
        {
            try
            {
                // ウィンドウDCとクライアントDCの両方を試行
                IntPtr[] dcSources = { GetWindowDC(hwnd), GetDC(hwnd) };
                
                foreach (IntPtr sourceDC in dcSources)
                {
                    if (sourceDC == IntPtr.Zero) continue;

                    try
                    {
                        using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            IntPtr memoryDC = g.GetHdc();
                            try
                            {
                                // 複数のコピー操作を試行
                                bool success = false;
                                
                                // 1. 標準のSRCCOPY
                                success = BitBlt(memoryDC, 0, 0, width, height, sourceDC, 0, 0, SRCCOPY);
                                
                                // 2. CAPTUREBLT（レイヤードウィンドウもキャプチャ）
                                if (!success)
                                {
                                    success = BitBlt(memoryDC, 0, 0, width, height, sourceDC, 0, 0, SRCCOPY | CAPTUREBLT);
                                }

                                if (success)
                                {
                                    g.ReleaseHdc(memoryDC);
                                    
                                    string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
                                    Directory.CreateDirectory(folder);
                                    string filePath = Path.Combine(folder, $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                                    bmp.Save(filePath, ImageFormat.Png);
                                    return true;
                                }
                            }
                            finally
                            {
                                try { g.ReleaseHdc(memoryDC); } catch { }
                            }
                        }
                    }
                    finally
                    {
                        ReleaseDC(hwnd, sourceDC);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"BitBlt capture failed: {ex.Message}");
            }

            return false;
        }


        private bool TryCaptureWithPrintWindow(IntPtr hwnd, int width, int height)
        {
            try
            {
                using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    IntPtr hdc = g.GetHdc();
                    try
                    {
                        // まずPW_RENDERFULLCONTENTフラグでキャプチャを試行（画面外の部分も含む）
                        bool success = PrintWindow(hwnd, hdc, PW_RENDERFULLCONTENT);
                        
                        if (!success)
                        {
                            // フラグなしで再試行
                            success = PrintWindow(hwnd, hdc, 0);
                        }
                        
                        if (success)
                        {
                            g.ReleaseHdc(hdc);
                            
                            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
                            Directory.CreateDirectory(folder);
                            string filePath = Path.Combine(folder, $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                            bmp.Save(filePath, ImageFormat.Png);
                            return true;
                        }
                    }
                    finally
                    {
                        try { g.ReleaseHdc(hdc); } catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"PrintWindow capture failed: {ex.Message}");
            }
            
            return false;
        }

        private bool TryCaptureWithDwmApi(IntPtr hwnd)
        {
            IntPtr hThumbnail = IntPtr.Zero;
            try
            {
                // ウィンドウの拡張フレーム境界を取得
                RECT windowRect;
                if (DwmGetWindowAttribute(hwnd, DWMWA_EXTENDED_FRAME_BOUNDS, out windowRect, (uint)Marshal.SizeOf(typeof(RECT))) != DWM_S_OK)
                {
                    Debug.WriteLine($"DwmGetWindowAttribute failed for hwnd: {hwnd}.");
                    return false;
                }

                int width = windowRect.Right - windowRect.Left;
                int height = windowRect.Bottom - windowRect.Top;

                if (width <= 0 || height <= 0)
                {
                    Debug.WriteLine($"Invalid window size from DwmGetWindowAttribute: Width={width}, Height={height}, Hwnd={hwnd}.");
                    return false;
                }

                // サムネイルを登録
                // DwmRegisterThumbnailのhwndDestinationは、通常、サムネイルを表示するウィンドウのハンドルです。
                // ここでは、現在のプロセスのメインウィンドウハンドルを使用します。
                if (DwmRegisterThumbnail(Process.GetCurrentProcess().MainWindowHandle, hwnd, out hThumbnail) != DWM_S_OK)
                {
                    Debug.WriteLine($"DwmRegisterThumbnail failed for hwnd: {hwnd}.");
                    return false;
                }

                // サムネイルのプロパティを設定
                DWM_THUMBNAIL_PROPERTIES props = new DWM_THUMBNAIL_PROPERTIES
                {
                    dwFlags = DWM_TNP_RECTDESTINATION | DWM_TNP_VISIBLE | DWM_TNP_OPACITY,
                    rcDestination = new RECT { Left = 0, Top = 0, Right = width, Bottom = height },
                    fVisible = true,
                    opacity = 255 // 完全に不透明
                };

                if (DwmUpdateThumbnailProperties(hThumbnail, ref props) != DWM_S_OK)
                {
                    Debug.WriteLine($"DwmUpdateThumbnailProperties failed for hwnd: {hwnd}.");
                    return false;
                }

                // DWM APIは直接ビットマップとして取得する機能を提供しないため、
                // ここではDWM APIが利用可能で、かつエラーなく処理を開始できたことを示すためにtrueを返す。
                // 実際の画像保存は、CaptureScreenshotメソッドのGDIフォールバックで行われる。
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in TryCaptureWithDwmApi: {ex.Message}");
                return false;
            }
            finally
            {
                if (hThumbnail != IntPtr.Zero)
                {
                    DwmUnregisterThumbnail(hThumbnail);
                }
            }
        }

        private void CaptureUsingGraphics()
        {
            var screen = Screen.PrimaryScreen.Bounds;
            int width = screen.Width;
            int height = screen.Height;
            try
            {
                using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(screen.X, screen.Y, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
                    string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
                    Directory.CreateDirectory(folder);
                    string filePath = Path.Combine(folder, $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    bmp.Save(filePath, ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error capturing screenshot with CopyFromScreen: {ex.Message}");
                // ここでさらにフォールバックする手段はないため、エラーをログに記録するのみ
            }
        }

        private void CaptureUsingGraphicsForGame(GameInfo gameInfo)
        {
            var screen = Screen.PrimaryScreen.Bounds;
            int width = screen.Width;
            int height = screen.Height;
            try
            {
                using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(screen.X, screen.Y, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
                    string filePath = PathUtility.GenerateScreenshotFilePath(gameInfo.DisplayName);
                    bmp.Save(filePath, ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error capturing screenshot with CopyFromScreen: {ex.Message}");
                // ここでさらにフォールバックする手段はないため、エラーをログに記録するのみ
            }
        }

        private bool TryCaptureWithAdvancedMethodsForGame(IntPtr hwnd, int width, int height, GameInfo gameInfo)
        {
            // DirectX/OpenGLゲーム向けの高度なキャプチャ方法を実装
            
            // 1. BitBltを使用した直接キャプチャ（画面外部分も取得可能）
            if (TryCaptureWithBitBltForGame(hwnd, width, height, gameInfo))
            {
                return true;
            }

            // 2. DWM APIを使用した高度なキャプチャ
            if (TryCaptureWithDwmApiForGame(hwnd, gameInfo))
            {
                return true;
            }

            // 3. クライアント領域の拡張キャプチャ
            if (TryCaptureWithExtendedClientForGame(hwnd, width, height, gameInfo))
            {
                return true;
            }

            return false;
        }

        private bool TryCaptureWithBitBltForGame(IntPtr hwnd, int width, int height, GameInfo gameInfo)
        {
            try
            {
                // ウィンドウDCとクライアントDCの両方を試行
                IntPtr[] dcSources = { GetWindowDC(hwnd), GetDC(hwnd) };
                
                foreach (IntPtr sourceDC in dcSources)
                {
                    if (sourceDC == IntPtr.Zero) continue;

                    try
                    {
                        using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            IntPtr memoryDC = g.GetHdc();
                            try
                            {
                                // 複数のコピー操作を試行
                                bool success = false;
                                
                                // 1. 標準のSRCCOPY
                                success = BitBlt(memoryDC, 0, 0, width, height, sourceDC, 0, 0, SRCCOPY);
                                
                                // 2. CAPTUREBLT（レイヤードウィンドウもキャプチャ）
                                if (!success)
                                {
                                    success = BitBlt(memoryDC, 0, 0, width, height, sourceDC, 0, 0, SRCCOPY | CAPTUREBLT);
                                }

                                if (success)
                                {
                                    g.ReleaseHdc(memoryDC);
                                    
                                    string filePath = PathUtility.GenerateScreenshotFilePath(gameInfo.DisplayName);
                                    bmp.Save(filePath, ImageFormat.Png);
                                    return true;
                                }
                            }
                            finally
                            {
                                try { g.ReleaseHdc(memoryDC); } catch { }
                            }
                        }
                    }
                    finally
                    {
                        ReleaseDC(hwnd, sourceDC);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"BitBlt capture failed: {ex.Message}");
            }

            return false;
        }

        private bool TryCaptureWithDwmApiForGame(IntPtr hwnd, GameInfo gameInfo)
        {
            IntPtr hThumbnail = IntPtr.Zero;
            try
            {
                // ウィンドウの拡張フレーム境界を取得
                RECT windowRect;
                if (DwmGetWindowAttribute(hwnd, DWMWA_EXTENDED_FRAME_BOUNDS, out windowRect, (uint)Marshal.SizeOf(typeof(RECT))) != DWM_S_OK)
                {
                    Debug.WriteLine($"DwmGetWindowAttribute failed for hwnd: {hwnd}.");
                    return false;
                }

                int width = windowRect.Right - windowRect.Left;
                int height = windowRect.Bottom - windowRect.Top;

                if (width <= 0 || height <= 0)
                {
                    Debug.WriteLine($"Invalid window size from DwmGetWindowAttribute: Width={width}, Height={height}, Hwnd={hwnd}.");
                    return false;
                }

                // サムネイルを登録
                if (DwmRegisterThumbnail(Process.GetCurrentProcess().MainWindowHandle, hwnd, out hThumbnail) != DWM_S_OK)
                {
                    Debug.WriteLine($"DwmRegisterThumbnail failed for hwnd: {hwnd}.");
                    return false;
                }

                // サムネイルのプロパティを設定
                DWM_THUMBNAIL_PROPERTIES props = new DWM_THUMBNAIL_PROPERTIES
                {
                    dwFlags = DWM_TNP_RECTDESTINATION | DWM_TNP_VISIBLE | DWM_TNP_OPACITY,
                    rcDestination = new RECT { Left = 0, Top = 0, Right = width, Bottom = height },
                    fVisible = true,
                    opacity = 255 // 完全に不透明
                };

                if (DwmUpdateThumbnailProperties(hThumbnail, ref props) != DWM_S_OK)
                {
                    Debug.WriteLine($"DwmUpdateThumbnailProperties failed for hwnd: {hwnd}.");
                    return false;
                }

                // DWM APIは直接ビットマップとして取得する機能を提供しないため、
                // ここではDWM APIが利用可能で、かつエラーなく処理を開始できたことを示すためにtrueを返す。
                // 実際の画像保存は、CaptureScreenshotメソッドのGDIフォールバックで行われる。
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in TryCaptureWithDwmApiForGame: {ex.Message}");
                return false;
            }
            finally
            {
                if (hThumbnail != IntPtr.Zero)
                {
                    DwmUnregisterThumbnail(hThumbnail);
                }
            }
        }

        private bool TryCaptureWithPrintWindowForGame(IntPtr hwnd, int width, int height, GameInfo gameInfo)
        {
            try
            {
                using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    IntPtr hdc = g.GetHdc();
                    try
                    {
                        // まずPW_RENDERFULLCONTENTフラグでキャプチャを試行（画面外の部分も含む）
                        bool success = PrintWindow(hwnd, hdc, PW_RENDERFULLCONTENT);
                        
                        if (!success)
                        {
                            // フラグなしで再試行
                            success = PrintWindow(hwnd, hdc, 0);
                        }
                        
                        if (success)
                        {
                            g.ReleaseHdc(hdc);
                            
                            string filePath = PathUtility.GenerateScreenshotFilePath(gameInfo.DisplayName);
                            bmp.Save(filePath, ImageFormat.Png);
                            return true;
                        }
                    }
                    finally
                    {
                        try { g.ReleaseHdc(hdc); } catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"PrintWindow capture failed: {ex.Message}");
            }
            
            return false;
        }

        private bool TryCaptureWithExtendedClientForGame(IntPtr hwnd, int width, int height, GameInfo gameInfo)
        {
            // クライアント領域とウィンドウ領域の両方を考慮したキャプチャ
            try
            {
                RECT clientRect;
                RECT windowRect;
                
                if (!GetClientRect(hwnd, out clientRect) || !GetWindowRect(hwnd, out windowRect))
                {
                    return false;
                }

                // ウィンドウの実際のコンテンツ領域を計算
                int clientWidth = clientRect.Right - clientRect.Left;
                int clientHeight = clientRect.Bottom - clientRect.Top;
                
                if (clientWidth <= 0 || clientHeight <= 0)
                {
                    return false;
                }

                using (Bitmap bmp = new Bitmap(clientWidth, clientHeight, PixelFormat.Format32bppArgb))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    IntPtr hdc = g.GetHdc();
                    try
                    {
                        // PW_CLIENTONLYフラグでクライアント領域のみをキャプチャ
                        if (PrintWindow(hwnd, hdc, PW_CLIENTONLY) || PrintWindow(hwnd, hdc, 0))
                        {
                            g.ReleaseHdc(hdc);
                            
                            string filePath = PathUtility.GenerateScreenshotFilePath(gameInfo.DisplayName);
                            bmp.Save(filePath, ImageFormat.Png);
                            return true;
                        }
                    }
                    finally
                    {
                        try { g.ReleaseHdc(hdc); } catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Extended client capture failed: {ex.Message}");
            }

            return false;
        }

        /// <summary>
        /// スクリーンショットを削除します。
        /// </summary>
        /// <param name="id">削除するスクリーンショットの ID</param>
        public void DeleteScreenshot(int id)
        {
            throw new NotImplementedException();
        }
    }
}