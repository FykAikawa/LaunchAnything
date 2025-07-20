/// <summary>
/// AddGroupWindow.xaml のコードビハインド
/// </summary>
using System.Windows;
using erlauncher.ViewModels;
using GalaSoft.MvvmLight.Ioc;

namespace erlauncher.Views
{
    /// <summary>
    /// AddGroupWindow の相互作用ロジック
    /// </summary>
    public partial class AddGroupWindow : Window
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AddGroupWindow()
        {
            InitializeComponent();
            // DataContext に AddGroupViewModel を設定
            DataContext = SimpleIoc.Default.GetInstance<AddGroupViewModel>();
        }
    }
}