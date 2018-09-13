# DotNetExtensionLibrary

天南十字星 (XstarS) 的 .NET Framework 扩展库。

C# 底层面向对象练习作品，同时也可用作自己开发时的实用库。

## 程序集 XstarS.Collections

对应 System.Collections 程序集，泛型集合相关。

> 根据 .NET 习惯，泛型集合命名空间 `System.Collections.Generic` 的内容置于 System.Collections 程序集；
> 而非泛型集合命名空间 `System.Collections` 的内容则置于 System.Collections.NonGeneric 程序集。

### 泛型类 `XstarS.Collections.Generic.CollectionEqualityComparer<T>`

提供泛型集合 `System.Collections.Generic.IEnumberable<T>` 的相等比较的方法，通过遍历每个元素进行比较。

### 泛型类 `XstarS.Collections.Generic.EquatableList<T>`

继承 `System.Collections.Generic.List<T>` 类。

可进行相等比较的泛型列表，通过遍历每个元素进行相等比较实现列表的相等比较。

重写了 `Equals(object)`、`GetHashCode()` 和 `ToString()` 方法，
实现了 `System.IEquatable<T>` 接口，并定义了 `==` 和 `!=` 运算符。

### 泛型类 `XstarS.Collections.Generic.EquatableDictionary<T>`

继承 `System.Collections.Generic.Dictionary<T>` 类。

可进行相等比较的泛型字典，通过遍历每个元素并对其键和值进行相等比较实现字典的相等比较。

重写了 `Equals(object)`、`GetHashCode()` 和 `ToString()` 方法，
实现了 `System.IEquatable<T>` 接口，并定义了 `==` 和 `!=` 运算符。

### 静态类 `XstarS.Collections.Generic.ListExtension`

提供 `System.Collections.Generic.IList<T>` 的扩展方法的静态类。

### 静态类 `XstarS.Collections.Generic.DictionaryExtension`

提供 `System.Collections.Generic.IDictionary<TKey, TValue>` 的扩展方法的静态类。

## 程序集 XstarS.ComponentModel.Binding

包含 `System.ComponentModel.INotifyPropertyChanged` 接口的若干实现，用于属性绑定到用户控件。

结合 System 程序集中的可绑定列表 `System.ComponentModel.BindingList<T>`，可实现便捷的数据绑定。

### 抽象类 `XstarS.ComponentModel.BindableObject`

`System.ComponentModel.INotifyPropertyChanged` 接口的实现，用于实现数据绑定到用户控件的抽象类。

此类包含了一个 `SetProperty<T>(ref T, T, string)` 的方法，
应在要绑定到用户控件的属性的 `set` 处调用此方法，实现更改属性值的同时通知客户端属性值发生更改。

`XstarS.ComponentModel.BindableObject` 为一抽象类，用法基于类的继承。

    using XstarS.ComponentModel;

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

### 泛型类 `XstarS.ComponentModel.Bindable<T>`

`System.ComponentModel.INotifyPropertyChanged` 接口的实现，用于实现数据绑定到用户控件的泛型类。

当某一类已经继承了另一个类时，将无法继承 `XstarS.ComponentModel.BindableObject` 抽象类，
此时可考虑将要绑定到用户控件的属性设置为 `XstarS.ComponentModel.Bindable<T>` 类。

`XstarS.ComponentModel.Bindable<T>` 内含一个 `Value` 属性，此属性更改时将会通知客户端。

    // ......
    using System.Windows;
    using XstarS.ComponentModel;
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

若将上例中 `Flag` 属性的 `Value` 属性绑定到用户控件的某属性，
则当服务端更改 `Flag` 属性的 `Value` 属性时，将会通知客户端属性值发生更改。

除初始化以外，不应直接给 `XstarS.ComponentModel.Bindable<T>` 实例赋值，建议可如上所示定义为一个只读自动属性。
直接更改实例的值将不会触发 `System.ComponentModel.INotifyPropertyChanged.PropertyChanged` 事件，
并会替换 `System.ComponentModel.INotifyPropertyChanged.PropertyChanged` 事件委托，破坏绑定关系。

## 程序集 XstarS.IO

对应 System.IO 程序集，文件系统访问相关。

### 静态类 `XstarS.IO.FileSystemInfoExtension`

提供 `System.IO.FileSystemInfo` 及其派生类的扩展方法的静态类。

## 程序集 XstarS.ParamReader

提供若干命令行参数解析器。

### 类 `XstarS.ParamReader`

默认的命令行参数解析器，作为命令行参数解析器的基类和默认实现。

* 不支持 Unix / Linux shell 中连字符 "-" 后接多个开关参数的解析。
* 不支持 PowerShell 中允许省略参数名称的有名参数的解析。
* 不支持一个参数名称后跟多个参数值的有名参数的解析。
* 不支持多个同名的有名参数的解析。

### 类 `XstarS.MultiValueParamReader`

继承 `XstarS.ParamReader` 类。

默认的多值命令行参数解析器。一个参数名能出现多次，之后也能接多个参数值。

目前为占位类，有待实现。

### 类 `XstarS.CmdParamReader`

继承 `XstarS.ParamReader` 类。

命令提示符 (CMD) 风格的命令行参数解析器，参数名称忽略大小写，有名参数名称与参数值用冒号 ":" 分隔。

* 支持多个同名的有名参数的解析。
* 不支持 Unix / Linux shell 中连字符 "-" 后接多个开关参数的解析。
* 不支持 PowerShell 中允许省略参数名称的有名参数的解析。
* 不支持一个参数名称后跟多个参数值的有名参数的解析。

### 类 `XstarS.PowerShellParamReader`

继承 `XstarS.ParamReader` 类。

PowerShell 风格的命令行参数解析器，参数名称忽略大小写。

* 不支持无名参数的解析，但支持省略名称的有名参数的解析。
* 不支持 Unix / Linux shell 中连字符 "-" 后接多个开关参数的解析。
* 不支持一个参数名称后跟多个参数值的有名参数的解析。
* 不支持多个同名的有名参数的解析。

### 类 `XstarS.UnixShellParamReader`

继承 `XstarS.ParamReader` 类。

Unix / Linux Shell 风格的命令行参数解析器，参数名称区分大小写。

* 支持连字符 "-" 后接多个开关参数的解析。
* 暂不支持连字符 "-" 开头的参数值的解析。
* 不支持 PowerShell 中允许省略参数名称的有名参数的解析。
* 不支持一个参数名称后跟多个参数值的有名参数的解析。
