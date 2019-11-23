using System.Windows.Input;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供更改命令 <see cref="ICommand"/> 的可执行状态的方法。
    /// </summary>
    public interface ICanExecuteToken
    {
        /// <summary>
        /// 获取或设置目标命令是否可在其当前状态下执行。
        /// </summary>
        bool IsExecutable { get; set; }

        /// <summary>
        /// 引发目标命令的 <see cref="ICommand.CanExecuteChanged"/> 事件。
        /// </summary>
        void OnCanExecuteChanged();
    }
}
