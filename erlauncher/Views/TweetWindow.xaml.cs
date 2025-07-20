/// <summary>
/// TweetWindow.xaml のコードビハインド
/// </summary>
using System.Windows;
using erlauncher.ViewModels;
using GalaSoft.MvvmLight.Ioc;

namespace erlauncher.Views
{
    /// <summary>
    /// TweetWindow の相互作用ロジック
    /// </summary>
    public partial class TweetWindow : Window
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TweetWindow()
        {
            InitializeComponent();
            // DataContext に TweetViewModel を設定
            DataContext = SimpleIoc.Default.GetInstance<TweetViewModel>();
        }
    }
}