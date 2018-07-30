# DotNetExtensionLibrary

天南十字星 (XstarS) 的 .NET Framework 扩展库。

C# 底层面向对象练习作品，同时也可用作自己开发时的实用库。

## 程序集 System.ComponentModel.Binding

文件名：System.ComponentModel.Binding.dll

包含 `System.ComponentModel.INotifyPropertyChanged` 接口的若干实现，用于属性绑定到用户控件。

结合 System.dll 中的可绑定列表 `System.ComponentModel.BindingList<T>`，可实现便捷的数据绑定。

### 抽象类 `System.ComponentModel.BindableObject`

`System.ComponentModel.INotifyPropertyChanged` 接口的实现，用于实现数据绑定到用户控件的抽象类。

此类包含了一个 `SetProperty<T>(ref T, T, string)` 的方法，
应在要绑定到用户控件的属性的 `set` 处调用此方法，实现更改属性值的同时通知客户端属性值发生更改。

`System.ComponentModel.BindableObject` 为一抽象类，用法基于类的继承。

    using System.ComponentModel;

    public class BindableData : BindableObject
    {
        private int data;

        public int Data
        {
            get => this.data;
            set => this.SetProperty(ref this.data, value);
        }
    }

若将上例中 `BindableData` 的实例的 `Data` 属性绑定到用户控件的某属性，
则当服务端更改 `BindableData` 实例的 `Data` 属性时，将会通知客户端属性值发生更改。

### 泛型密封类 `System.ComponentModel.Bindable<T>`

`System.ComponentModel.INotifyPropertyChanged` 接口的实现，用于实现数据绑定到用户控件的泛型类。

当某一类已经继承了另一个类时，将无法继承 `System.ComponentModel.BindableObject` 抽象类，
此时可考虑将要绑定到用户控件的属性设置为 `System.ComponentModel.Bindable<T>` 类。

`System.ComponentModel.Bindable<T>` 内含一个 `Value` 属性，此属性更改时将会通知客户端。

    // ......
    using System.ComponentModel;
    using System.Windows;
    // ......

    public class MainWindow : Window
    {
        // ......

        public MainWindow()
        {
            // ......
            this.Flag = false;
            // ......
        }

        // ......

        public Bindable<bool> Flag { get; }

        // ......

        private void Method()
        {
            this.Flag.Value = true; // 此时将会通知客户端属性值发生更改。
        }

        // ......
    }

    // ......

若将上例中 `Flag` 属性的 `Value` 属性绑定到用户控件的某属性，
则当服务端更改 `Flag` 属性的 `Value` 属性时，将会通知客户端属性值发生更改。

除初始化以外，不应直接给 `System.ComponentModel.Bindable<T>` 实例赋值，建议可如上所示定义为一个只读自动属性。
直接更改实例的值将不会触发 `System.ComponentModel.INotifyPropertyChanged.PropertyChanged` 事件，
并会替换 `System.ComponentModel.INotifyPropertyChanged.PropertyChanged` 事件委托，破坏绑定关系。

## 程序集 XstarS.Linq

文件名：XstarS.Linq.dll

对应 System.Linq.dll 程序集，泛型集合扩展方法相关。

### 静态类 `XstarS.Linq.List`

提供 `System.Collections.Generic.IList<T>` 的扩展方法的静态类。

### 静态类 `XstarS.Linq.Dictionary`

提供 `System.Collections.Generic.IDictionary<TKey, TValue>` 的扩展方法的静态类。

## 程序集 XstarS.ParamReader

文件名：XstarS.ParamReader.dll

提供若干命令行参数解析器。
目前暂时只有一个默认实现。

### 类 `XstarS.ParamReader`

默认的命令行参数解析器，作为命令行参数解析器的基类和默认实现。

* 不支持 Unix / Linux shell 中连字符 "-" 后接多个开关参数的解析。
* 不支持 PowerShell 中允许省略参数名称的有名参数的解析。
