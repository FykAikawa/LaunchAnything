/// <summary>
/// MainWindow.xaml のコードビハインド
/// </summary>
using System.Windows;
using erlauncher.ViewModels;

namespace erlauncher.Views
{
    /// <summary>
    /// MainWindow の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            // DataContext に MainViewModel を設定
        }
    }
}