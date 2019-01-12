# DotNetExtensionLibrary

天南十字星 (XstarS) 的 .NET Framework 扩展库。

C# 底层面向对象练习作品，同时也可用作自己开发时的实用库。

工程结构和命名空间均仿照 .NET 框架库。

## 程序集 XstarS

对应 mscorlib 和 System 程序集，系统基础相关。

除提供系统基础相关的类型和扩展方法外，还包括了集合相关和 Win32 相关的内容。

目前包含的命名空间：

* `XstarS`
* `XstarS.Collections`
* `XstarS.Collections.Generic`
* `XstarS.Diagnostics`
* `XstarS.IO`
* `XstarS.Reflection.Emit`
* `XstarS.Win32`

## 程序集 XstarS.ComponentModel.Binding

包含 `System.ComponentModel.INotifyPropertyChanged` 接口的若干实现，用于属性绑定到用户控件。

结合 System 程序集中的可绑定列表 `System.ComponentModel.BindingList<T>`，可实现便捷的数据绑定。

### 方法提取

将属性绑定的公用代码提取为方法，并在属性的 `set` 处调用，减少重复代码。

#### 抽象类 `XstarS.ComponentModel.BindableObject`

`System.ComponentModel.INotifyPropertyChanged` 接口的实现，用于实现数据绑定到用户控件的抽象类。

此类包含了一个 `SetProperty<T>(ref T, T, string)` 的方法，
应在要绑定到用户控件的属性的 `set` 处调用此方法，实现更改属性值的同时通知客户端属性值发生更改。

`XstarS.ComponentModel.BindableObject` 为一抽象类，用法基于类的继承。

#### 静态类 `XstarS.ComponentModel.BindingExtensions`

提供数据绑定相关的扩展方法。

目前提供与 `XstarS.ComponentModel.BindableObject` 几乎完全一致的扩展方法。

在类（非显式）实现 `System.ComponentModel.INotifyPropertyChanged` 接口后，
即可在属性的 `set` 处直接调用 `SetProperty<T>(ref T, T, string)` 扩展方法以修改属性并触发属性更改事件。

由于在类外部不能直接触发事件，扩展方法中的事件触发只能基于反射调用。
反射调用可能存在性能问题，当绑定属性的数量较大时不建议采用此方案。

#### 方法使用说明

两类的使用方法完全一致，都要求实现 `System.ComponentModel.INotifyPropertyChanged` 接口。
当继承 `BindableObject` 类时，则不会调用 `BindingExtensions` 类中的扩展方法。

``` CSharp
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
```

若将上例中 `BindableData` 的实例的 `Data` 属性绑定到用户控件的某属性，
则当服务端更改 `BindableData` 实例的 `Data` 属性时，将会通知客户端属性值发生更改。

### 绑定值封装

将用于绑定的值封装到一个类中，并在类的某个属性实现属性更改通知客户端。

#### 泛型类 `XstarS.ComponentModel.Bindable<T>`

继承 `XstarS.ComponentModel.BindableObject` 类。

`System.ComponentModel.INotifyPropertyChanged` 接口的实现，用于实现数据绑定到用户控件的泛型类。

当某一类已经继承了另一个类时，将无法继承 `XstarS.ComponentModel.BindableObject` 抽象类，
此时可考虑将要绑定到用户控件的属性设置为 `XstarS.ComponentModel.Bindable<T>` 类。

`XstarS.ComponentModel.Bindable<T>` 内含一个 `Value` 属性，此属性更改时将会通知客户端。

除初始化以外，不应直接给 `XstarS.ComponentModel.Bindable<T>` 实例赋值，建议可如上所示定义为一个只读自动属性。
直接更改实例的值将不会触发 `System.ComponentModel.INotifyPropertyChanged.PropertyChanged` 事件，
并会替换 `System.ComponentModel.INotifyPropertyChanged.PropertyChanged` 事件委托，破坏绑定关系。

#### 封装类使用说明

``` CSharp
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
        this.Flag.Value = true;  // 此时将会通知客户端属性值发生更改。
    }

    // ......
}
```

若将上例中 `Flag` 属性的 `Value` 属性绑定到用户控件的某属性，
则当服务端更改 `Flag` 属性的 `Value` 属性时，将会通知客户端属性值发生更改。

### 动态生成数据绑定派生类

定义一个原型基类或接口，通过 `System.Reflection.Emit` 命名空间提供的类来动态生成派生类，
并在派生类的属性中实现数据绑定的相关代码。

#### 泛型接口 `XstarS.ComponentModel.IBindingBuilder<out T>`

提供从原型构造用于数据绑定的实例的方法。

`BindableOnly` 属性指定是否仅对有 `System.ComponentModel.BindableAttribute` 特性的属性构造绑定关系。

`CreateInstance()` 方法则构造一个基于 `T` 类型的派生类的实例，并根据 `BindableOnly` 属性的指示，实现某些属性的数据绑定。

#### 泛型抽象类 `XstarS.ComponentModel.BindingBuilder<T>`

实现 `XstarS.ComponentModel.IBindingBuilder<out T>` 接口。

提供从原型构造用于数据绑定的实例的方法的基类和工厂方法。

通过 `Default` 或 `Bindable` 属性，可构造一个 `BindingBuilder<T>` 类的实例，
调用此实例的 `CreateInstance()` 方法可构造 `T` 类型用于数据绑定的实例。

#### 动态生成使用说明

首先定义一个原型基类或接口，原型必须（非显式）实现 `System.ComponentModel.INotifyPropertyChanged` 接口。

``` CSharp
using System.ComponentModel;

public interface IBindableData : INotifyPropertyChanged
{
    int Value { get; set; }

    [Bindable(true)]
    int BindingValue { get; set; }
}
```

注意，若定义的原型为一个类，则应将用于绑定的属性定义为 `virtual` 或 `abstract`，使得此属性能够在派生类中被重写。
`BindingBuilder<T>` 不会对非 `virtual` 或 `abstract` 的属性生成绑定代码。
同时，`PropertyChanged` 事件也应定义为 `abstract`，或是定义一个事件触发函数 `OnPropertyChanged(string)`，否则会导致无法正确构造派生类。

> 若基类中的属性或方法或未定义为 `virtual` 或 `abstract`，则在派生类仅隐藏了基类中对应的定义，并未重写。
> 当派生类的实例声明为基类时，则会调用基类中定义的属性或方法。

而后在设置绑定处通过 `Default` 或 `Bindable` 属性构造 `BindingBuilder<IBindableData>` 的实例，
调用 `CreateInstance()` 方法构造基于原型接口 `IBindableData` 的实例。

``` CSharp
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
        //var builder = BindingBuilder<IBindableData>.Default;  // 对所有属性设置绑定。
        var builder = BindingBuilder<IBindableData>.Bindable;   // 仅对 Bindable 属性设置绑定。
        this.BindingData = builder.CreateInstance();
        // ......
    }

    // ......

    public IBindableData BindingData { get; }

    // ......
}
```

此时若更改 `MainWindow.BindingData` 的 `BindingValue` 属性会通知客户端属性发生更改，而更改 `Value` 属性则不会。
若使用 `Default` 属性构造 `BindingBuilder<IBindableData>`，则两属性都会在发生更改时通知客户端。

## 程序集 XstarS.ParamReaders

提供简易的命令行参数解析器，以及以此为基础的多种风格的命令行参数解析器。

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
* 支持连字符 "-" 开头的参数值的解析。
* 不支持 PowerShell 中允许省略参数名称的有名参数的解析。
* 不支持一个参数名称后跟多个参数值的有名参数的解析。

## 程序集 XstarS.ValueValidate

提供参数验证和抛出异常的方法。

### 泛型接口 `XstarS.IValidate<out T>`

提供参数验证所需的数据，包括参数值 `Value` 和参数名 `Name`。

### 静态类 `XstarS.Validate`

提供 `XstarS.IValidate<out T>` 接口实例的工厂方法，以及参数验证和抛出异常的方法。

参数验证过程全部通过 `XstarS.IValidate<out T>` 的扩展方法实现，以便设定各种泛型约束。
每个验证方法均包含一个名为 `message` 的可选参数，可自定义异常消息。

### 参数验证示例

``` CSharp
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
```

## 程序集 XstarS.Windows

对应 PresentationCore 和 PresentationFramework 程序集，WPF 相关。

目前包含的命名空间：

* `XstarS.Windows.Controls`
* `XstarS.Windows.Media`
