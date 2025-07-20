/// <summary>
/// CreateGameInfoWindow.xaml のコードビハインド
/// </summary>
using System.Windows;
using erlauncher.ViewModels;
using GalaSoft.MvvmLight.Ioc;

namespace erlauncher.Views
{
    /// <summary>
    /// CreateGameInfoWindow の相互作用ロジック
    /// </summary>
    public partial class CreateGameInfoWindow : Window
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CreateGameInfoWindow()
        {
            InitializeComponent();
            // DataContext に CreateGameInfoViewModel を設定
            DataContext = SimpleIoc.Default.GetInstance<CreateGameInfoViewModel>();
        }
    }
}