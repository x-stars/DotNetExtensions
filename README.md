# DotNetExtensionLibrary

天南十字星 (XstarS) 的 .NET Framework 扩展库。

C# 底层面向对象练习作品，同时也可用作自己开发时的实用库。

## 程序集 XstarS

对应 System 程序集，系统基础相关。

### 静态类 `XstarS.SystemHelper`

提供框架基础级别的帮助方法。

### 静态类 `XstarS.MathSp`

提供针对特殊情况进行优化的数学运算方法。

### 静态类 `XstarS.TimeSpanExtensions`

提供时间间隔 `System.TimeSpan` 的扩展方法。

### 静态类 `XstarS.TypeExtensions`

提供类型声明 `System.Type` 的扩展方法。

### 泛型类 `XstarS.Verbose<T>`

变量的值发生读写时，自动输出相关信息。

默认状态下，对 `Value` 属性进行读写操作时均会在控制台中输出相关信息。

可通过修改 `OnValueRead` 和 `OnValueWrite` 委托自定义读写值时应执行的操作。

### 类 `XstarS.ParamReader`

默认的命令行参数解析器，作为命令行参数解析器的基类和默认实现。

* 不支持 Unix / Linux shell 中连字符 "-" 后接多个开关参数的解析。
* 不支持 PowerShell 中允许省略参数名称的有名参数的解析。
* 不支持一个参数名称后跟多个参数值的有名参数的解析。
* 不支持多个同名的有名参数的解析。

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

### 静态类 `XstarS.IO.FileSystemInfoExtension`

提供文件系统信息 `System.IO.FileSystemInfo` 及其派生类的扩展方法。

## 程序集 XstarS.Collections

对应 System.Collections 程序集，泛型集合相关。

> 根据 .NET 习惯，泛型集合命名空间 `System.Collections.Generic` 的内容置于 System.Collections 程序集；
> 而非泛型集合命名空间 `System.Collections` 的内容则置于 System.Collections.NonGeneric 程序集。

### 泛型类 `XstarS.Collections.Generic.SequenceEqualityComparer<T>`

提供泛型集合 `System.Collections.Generic.IEnumberable<out T>` 的元素序列的相等比较的方法。

### 泛型类 `XstarS.Collections.Generic.IndexedLinkedList<T>`

继承 `System.Collections.Generic.LinkedList<T>` 类。

能够通过索引访问的双重链接列表。通过遍历元素并计数实现按索引值访问。

目前存在**严重性能问题**，暂无实用价值。

相比于 `System.Collections.Generic.LinkedList<T>`，
新增实现了 `System.Collections.Generic.IList<T>` 和
`System.Collections.Generic.IReadOnlyList<out T>` 两个泛型集合接口。

### 静态类 `XstarS.Collections.Generic.EnumerableExtensions`

提供公开枚举数 `System.Collections.Generic.IEnumerable<out T>` 的扩展方法。

### 静态类 `XstarS.Collections.Generic.ListExtensions`

提供可索引访问的集合 `System.Collections.Generic.IList<T>` 的扩展方法。

### 静态类 `XstarS.Collections.Generic.DictionaryExtensions`

提供键/值对的集合 `System.Collections.Generic.IDictionary<TKey, TValue>` 的扩展方法。

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

继承 `XstarS.ComponentModel.BindableObject` 类。

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

## 程序集 XstarS.ValueValidate

提供参数验证和抛出异常的方法。

## 泛型接口 `XstarS.IValidate<out T>`

提供参数验证所需的数据，包括参数值 `Value` 和参数名 `Name`。

## 静态类 `XstarS.Validate`

提供 `XstarS.IValidate<out T>` 接口实例的创建方法，以及参数验证和抛出异常的方法。

参数验证过程全部通过 `XstarS.IValidate<out T>` 的扩展方法实现，以便设定各种泛型约束。
每个验证方法均包含一个名为 `message` 的可选参数，可自定义异常消息。

### 参数验证示例

    using XstarS;

    // 存在一个 string 型的名为 param 的参数。
    // 现要求其以数字开头，并且包含在键的集合 keys 中，
    // 且不以 "." 结尾。

    Validate.Value(param, nameof(param))    // 使用 XstarS.Validate.Value<T>(T, string) 方法，
                                            // 创建一个 XstarS.IValidate<out T> 接口的实例。

        .IsNotNull()                        // 验证参数不为 null，否则抛出 System.ArgumentNullException 异常。

        .IsInRange("0", "9", "OutOfRange")  // 要求实现 System.IComparable<in T> 接口，验证参数是否在指定范围内，
                                            // 否则抛出 System.ArgumentException 异常，并设定异常消息为 "OutOfRange"。

        .IsInKeys(keys)                     // 验证参数在键的集合中，否则抛出
                                            // System.Collections.Generic.KeyNotFoundException 异常。

        .NotEndsWith(".", "InvalidEnd");    // 验证字符串不以 "." 结尾，否则抛出 System.ArgumentException 异常，
                                            // 并设定异常消息为 "InvalidEnd"。
                                            // 此方法也可使用正则表达式版本的 IsNotMatch(".$", "InvalidEnd") 方法实现。

    // ......
