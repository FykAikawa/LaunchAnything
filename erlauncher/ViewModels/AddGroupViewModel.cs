using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using erlauncher.Services;

namespace erlauncher.ViewModels
{
    /// <summary>
    /// グループ追加用 ViewModel
    /// </summary>
    public class AddGroupViewModel : ViewModelBase
    {
        private readonly IGroupService _groupService;

        private string _newGroupName;

        /// <summary>
        /// 入力されたグループ名
        /// </summary>
        public string NewGroupName
        {
            get => _newGroupName;
            set => Set(ref _newGroupName, value);
        }

        /// <summary>
        /// グループ追加コマンド
        /// </summary>
        public ICommand AddGroupCommand { get; }

        /// <summary>
        /// キャンセルコマンド
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="groupService">グループサービス</param>
        public AddGroupViewModel(IGroupService groupService)
        {
            _groupService = groupService;
            AddGroupCommand = new RelayCommand(OnAddGroup);
            CancelCommand = new RelayCommand(OnCancel);
        }

        /// <summary>
        /// グループ追加処理
        /// </summary>
        private void OnAddGroup()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// キャンセル処理
        /// </summary>
        private void OnCancel()
        {
            throw new NotImplementedException();
        }
    }
}