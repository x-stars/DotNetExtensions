# 数据绑定接口实现框架

本文叙述了数据绑定接口 `System.ComponentModel.INotifyPropertyChanged`
的实现框架 XstarS.ComponentModel.Bindable 程序集的使用方法，
结合 System 程序集中的可绑定列表 `System.ComponentModel.BindingList<T>`，可实现便捷的数据绑定。

目前提供三种方案：方法提取、绑定值封装、动态生成数据绑定派生类。三种方案各有优缺点，目前各自的建议使用场景：

* 方法提取：在属性值不由一个字段决定时使用；较为自由，但代码量也会相应较大。
* 绑定值封装：在要设置数据绑定的属性较少，或各个属性之间不成结构时使用。
* 动态生成数据绑定派生类：在要设置数据绑定的属性较多，或各个属性之间能形成结构时使用。

## 方法提取

将属性绑定的公用代码提取为方法，并在属性的 `set` 处调用，减少重复代码。

### 抽象类 `XstarS.ComponentModel.BindableObject`

`System.ComponentModel.INotifyPropertyChanged` 接口的实现，用于实现数据绑定到用户控件的抽象类。

此类包含了一个 `SetProperty<T>(ref T, T, string)` 的方法，
应在要绑定到用户控件的属性的 `set` 处调用此方法，实现更改属性值的同时通知客户端属性值发生更改。

`XstarS.ComponentModel.BindableObject` 为一抽象类，用法基于类的继承。

### 方法使用说明

``` CSharp
using System.ComponentModel;
using XstarS.ComponentModel;

public class BindableRectangle : BindableObject
{
    private double width;
    private double height;

    public double Width
    {
        get => this.width;
        set
        {
            this.SetProperty(ref this.width, value);
            this.OnPropertyChanged(nameof(this.Size));
        }
    }

    public double Height
    {
        get => this.height;
        set
        {
            this.SetProperty(ref this.height, value);
            this.OnPropertyChanged(nameof(this.Size));
        }
    }

    public double Size => this.width * this.height;
}
```

若将上例中 `BindableRectangle` 的实例的任意属性绑定到用户控件的某属性，
则当服务端更改 `BindableRectangle` 实例的 `Width` 或 `Height` 属性时，
将会通知客户端 `Width` 或 `Height` 以及 `Size` 属性的值发生更改。

## 绑定值封装

将用于绑定的值封装到一个类中，并在类的某个属性实现属性更改通知客户端。

### 泛型类 `XstarS.ComponentModel.Bindable<T>`

继承 `XstarS.ComponentModel.BindableObject` 类。

`System.ComponentModel.INotifyPropertyChanged` 接口的实现，用于实现数据绑定到用户控件的泛型类。

当某一类已经继承了另一个类时，将无法继承 `XstarS.ComponentModel.BindableObject` 抽象类，
此时可考虑将要绑定到用户控件的属性设置为 `XstarS.ComponentModel.Bindable<T>` 类。

`XstarS.ComponentModel.Bindable<T>` 内含一个 `Value` 属性，此属性更改时将会通知客户端。

除初始化以外，不应直接给 `XstarS.ComponentModel.Bindable<T>` 实例赋值，建议可如上所示定义为一个只读自动属性。
直接更改实例的值将不会触发 `System.ComponentModel.INotifyPropertyChanged.PropertyChanged` 事件，
并会替换 `System.ComponentModel.INotifyPropertyChanged.PropertyChanged` 事件委托，破坏绑定关系。

### 封装类使用说明

``` CSharp
// ......
using System.Windows;
using XstarS.ComponentModel;

public class MainWindow : Window
{
    public MainWindow()
    {
        this.Flag = false;
        // ......
        this.InitializeComponent();
    }

    // ......

    public Bindable<bool> Flag { get; }

    private void Method()
    {
        // 修改属性值并通知客户端属性值发生更改。
        this.Flag.Value = true;
    }
}
```

若将上例中 `Flag` 属性的 `Value` 属性绑定到用户控件的某属性，
则当服务端更改 `Flag` 属性的 `Value` 属性时，将会通知客户端属性值发生更改。

## 动态生成数据绑定派生类

定义一个原型基类或接口，通过 `System.Reflection.Emit` 命名空间提供的类来动态生成派生类，
并在派生类的属性中实现数据绑定的相关代码。

### 泛型接口 `XstarS.ComponentModel.IBindableTypeProvider<out T>`

用于提供原型类型对应的可用于数据绑定的派生类型，并提供创建此派生类型的实例的方法。

* `BindableType` 属性返回构造完成的用于数据绑定的派生类型。
* `CreateInstance()` 方法构造一个 `BindableType` 的实例。
* `CreateInstance(object[])` 方法以指定参数构造一个 `BindableType` 的实例。

### `XstarS.ComponentModel.IBindableTypeProvider<out T>` 的实现

* `BindableTypeProvider` 提供非泛型实现。
* `BindableTypeProvider<T>` 提供泛型实现。

### 动态生成使用说明

首先定义一个原型基类或接口，若原型为一个类，应包含 `public` 或 `protected` 访问级别的构造函数。

``` CSharp
using System.ComponentModel;

public interface IBindableData : INotifyPropertyChanged
{
    int Value { get; set; }
    int BindingValue { get; set; }
}
```

注意，若定义的原型为一个类 (`class`)，则应将用于绑定的属性定义为 `virtual` 或 `abstract`，使得此属性能够在派生类中被重写。
`BindableTypeProvider<T>` 不会对非 `virtual` 或 `abstract` 的属性生成绑定代码。
同时，若定义了 `PropertyChanged` 事件，应将其应定义为 `abstract`，
或是定义一个事件触发函数 `System.Void OnPropertyChanged(System.String)`，否则会导致无法正确构造派生类。

> 若基类中的属性或方法或未定义为 `virtual` 或 `abstract`，则在派生类仅隐藏了基类中对应的定义，并未重写。
> 当派生类的实例声明为基类时，则会调用基类中定义的属性或方法。

而后在设置绑定处通过 `Default` 属性获取 `BindableTypeProvider<IBindableData>` 的默认实例，
或是以自定义属性筛选条件通过 `Custom` 方法创建 `BindableTypeProvider<IBindableData>` 的自定义实例，
再调用 `CreateInstance` 方法构造基于原型接口 `IBindableData` 的实例。

``` CSharp
// ......
using System.Windows;
using XstarS.ComponentModel;

public class MainWindow : Window
{
    public MainWindow()
    {
        // 对所有可重写属性设置绑定。
        var defaultProvider = BindableTypeProvider<IBindableData>.Default;
        // 仅对名称以 Binding 开头的属性设置绑定。
        var customProvider = BindableTypeProvider<IBindableData>.Custom(
            prop => prop.Name.StartsWith("Binding"));
        this.BindingData = customProvider.CreateInstance();
        // ......
        this.InitializeComponent();
    }

    // ......

    public IBindableData BindingData { get; }
}
```

此时若更改 `MainWindow.BindingData` 的 `BindingValue` 属性会通知客户端属性发生更改，而更改 `Value` 属性则不会。
若使用 `defaultProvider` 创建 `IBindableData` 的实例，则两属性都会在发生更改时通知客户端。
