using System;
using System.ComponentModel;
using System.Windows.Input;
using XstarS.ComponentModel;

namespace XstarS.Windows.Input
{
    /// <summary>
    /// 表示由委托定义的命令 <see cref="ICommand"/>。
    /// </summary>
    public class DelegateCommand : CommandBase
    {
        /// <summary>
        /// 表示 <see cref="DelegateCommand.Execute(object)"/> 方法调用的委托。
        /// </summary>
        private readonly Action<object?> ExecuteDelegate;

        /// <summary>
        /// 表示 <see cref="DelegateCommand.CanExecute(object)"/> 方法调用的委托。
        /// </summary>
        private readonly Predicate<object?> CanExecuteDelegate;

        /// <summary>
        /// 使用指定的委托初始化 <see cref="DelegateCommand"/> 类的新实例。
        /// </summary>
        /// <param name="executeDelegate">
        /// <see cref="DelegateCommand.Execute(object)"/> 方法调用的委托。</param>
        /// <param name="canExecuteDelegate">
        /// <see cref="DelegateCommand.CanExecute(object)"/> 方法调用的委托。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="executeDelegate"/> 为 <see langword="null"/>。</exception>
        public DelegateCommand(Action<object?> executeDelegate,
            Predicate<object?>? canExecuteDelegate = null)
        {
            this.ExecuteDelegate = executeDelegate ??
                throw new ArgumentNullException(nameof(executeDelegate));
            this.CanExecuteDelegate = canExecuteDelegate ?? base.CanExecute;
        }

        /// <summary>
        /// 使用指定的无参数委托创建 <see cref="DelegateCommand"/> 类的实例。
        /// </summary>
        /// <param name="executeDelegate">
        /// <see cref="DelegateCommand.Execute(object)"/> 方法调用的委托。</param>
        /// <param name="canExecuteDelegate">
        /// <see cref="DelegateCommand.CanExecute(object)"/> 方法调用的委托。</param>
        /// <returns>一个执行输入的委托的 <see cref="DelegateCommand"/> 对象。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="executeDelegate"/> 为 <see langword="null"/>。</exception>
        public static DelegateCommand Create(Action executeDelegate,
            Func<bool>? canExecuteDelegate = null)
        {
            var wrapper = new Parameterless(executeDelegate, canExecuteDelegate);
            return new DelegateCommand(wrapper.Execute, wrapper.CanExecute);
        }

        /// <summary>
        /// 使用指定的泛型参数委托创建 <see cref="DelegateCommand"/> 类的实例。
        /// </summary>
        /// <param name="executeDelegate">
        /// <see cref="DelegateCommand.Execute(object)"/> 方法调用的委托。</param>
        /// <param name="canExecuteDelegate">
        /// <see cref="DelegateCommand.CanExecute(object)"/> 方法调用的委托。</param>
        /// <returns>一个执行输入的委托的 <see cref="DelegateCommand"/> 对象。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="executeDelegate"/> 为 <see langword="null"/>。</exception>
        public static DelegateCommand Create<TParam>(Action<TParam?> executeDelegate,
            Predicate<TParam?>? canExecuteDelegate = null)
        {
            var wrapper = new Parameter<TParam>(executeDelegate, canExecuteDelegate);
            return new DelegateCommand(wrapper.Execute, wrapper.CanExecute);
        }

        /// <summary>
        /// 在当前状态下执行此命令。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        public override void Execute(object? parameter)
        {
            this.ExecuteDelegate.Invoke(parameter);
        }

        /// <summary>
        /// 确定此命令是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        /// <returns>如果可执行此命令，则为 <see langword="true"/>；
        /// 否则为 <see langword="false"/>。</returns>
        public override bool CanExecute(object? parameter)
        {
            return this.CanExecuteDelegate.Invoke(parameter);
        }

        /// <summary>
        /// 通知当前命令的可执行状态已更改。
        /// </summary>
        public new void NotifyCanExecuteChanged()
        {
            base.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// 设定在指定属性发生更改时，通知当前命令的可执行状态已更改。
        /// </summary>
        /// <param name="source">发出属性更改通知的事件源对象。</param>
        /// <param name="propertyName">要接收更改通知的属性的名称。</param>
        /// <returns>当前 <see cref="DelegateCommand"/> 实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public DelegateCommand ObserveCanExecute(
            INotifyPropertyChanged source, string? propertyName)
        {
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            var observer = new SimplePropertyObserver(source, propertyName);
            observer.ObservingPropertyChanged += this.OnObservingCanExecuteChanged;
            return this;
        }

        /// <summary>
        /// 当指定属性发生更改时，通知命令的可执行状态发生更改。
        /// </summary>
        /// <param name="sender">属性更改通知的事件源。</param>
        /// <param name="e">提供属性更改通知的事件数据。</param>
        private void OnObservingCanExecuteChanged(object? sender, EventArgs e)
        {
            this.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// 表示由无参数委托定义的命令 <see cref="ICommand"/>。
        /// </summary>
        private sealed class Parameterless : CommandBase
        {
            /// <summary>
            /// 表示 <see cref="Parameterless.Execute(object)"/> 方法调用的委托。
            /// </summary>
            private readonly Action ExecuteDelegate;

            /// <summary>
            /// 表示 <see cref="Parameterless.CanExecute(object)"/> 方法调用的委托。
            /// </summary>
            private readonly Func<bool> CanExecuteDelegate;

            /// <summary>
            /// 使用指定的委托初始化 <see cref="Parameterless"/> 类的新实例。
            /// </summary>
            /// <param name="executeDelegate">
            /// <see cref="Parameterless.Execute(object)"/> 方法调用的委托。</param>
            /// <param name="canExecuteDelegate">
            /// <see cref="Parameterless.CanExecute(object)"/> 方法调用的委托。</param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="executeDelegate"/> 为 <see langword="null"/>。</exception>
            internal Parameterless(Action executeDelegate,
                Func<bool>? canExecuteDelegate = null)
            {
                static bool DefaultCanExecute() => true;
                this.ExecuteDelegate = executeDelegate ??
                    throw new ArgumentNullException(nameof(executeDelegate));
                this.CanExecuteDelegate = canExecuteDelegate ?? DefaultCanExecute;
            }

            /// <summary>
            /// 在当前状态下执行此命令。
            /// </summary>
            /// <param name="parameter">此命令使用的数据。</param>
            public override void Execute(object? parameter)
            {
                this.ExecuteDelegate.Invoke();
            }

            /// <summary>
            /// 确定此命令是否可在其当前状态下执行。
            /// </summary>
            /// <param name="parameter">此命令使用的数据。</param>
            /// <returns>如果可执行此命令，则为 <see langword="true"/>；
            /// 否则为 <see langword="false"/>。</returns>
            public override bool CanExecute(object? parameter)
            {
                return this.CanExecuteDelegate.Invoke();
            }
        }

        /// <summary>
        /// 表示由泛型参数委托定义的命令 <see cref="ICommand"/>。
        /// </summary>
        /// <typeparam name="T">命令使用的参数的类型。</typeparam>
        private sealed class Parameter<T> : CommandBase
        {
            /// <summary>
            /// 表示 <see cref="Parameter{T}.Execute(object)"/> 方法调用的委托。
            /// </summary>
            private readonly Action<T?> ExecuteDelegate;

            /// <summary>
            /// 表示 <see cref="Parameter{T}.CanExecute(object)"/> 方法调用的委托。
            /// </summary>
            private readonly Predicate<T?> CanExecuteDelegate;

            /// <summary>
            /// 使用指定的委托初始化 <see cref="Parameter{T}"/> 类的新实例。
            /// </summary>
            /// <param name="executeDelegate">
            /// <see cref="Parameter{T}.Execute(object)"/> 方法调用的委托。</param>
            /// <param name="canExecuteDelegate">
            /// <see cref="Parameter{T}.CanExecute(object)"/> 方法调用的委托。</param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="executeDelegate"/> 为 <see langword="null"/>。</exception>
            internal Parameter(Action<T?> executeDelegate,
                Predicate<T?>? canExecuteDelegate = null)
            {
                static bool DefaultCanExecute(T? parameter) => true;
                this.ExecuteDelegate = executeDelegate ??
                    throw new ArgumentNullException(nameof(executeDelegate));
                this.CanExecuteDelegate = canExecuteDelegate ?? DefaultCanExecute;
            }

            /// <summary>
            /// 在当前状态下执行此命令。
            /// </summary>
            /// <param name="parameter">此命令使用的数据。</param>
            public override void Execute(object? parameter)
            {
                var typedParam = (parameter is null) ? default(T) : (T)parameter;
                this.ExecuteDelegate.Invoke(typedParam);
            }

            /// <summary>
            /// 确定此命令是否可在其当前状态下执行。
            /// </summary>
            /// <param name="parameter">此命令使用的数据。</param>
            /// <returns>如果可执行此命令，则为 <see langword="true"/>；
            /// 否则为 <see langword="false"/>。</returns>
            public override bool CanExecute(object? parameter)
            {
                var typedParam = (parameter is null) ? default(T) : (T)parameter;
                return this.CanExecuteDelegate.Invoke(typedParam);
            }
        }
    }
}
