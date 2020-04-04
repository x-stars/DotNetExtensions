# 属性更改通知接口实现框架

本文叙述了 XstarS.ObjectModel 和 XstarS.ObservableProxy 程序集中包含的若干实现属性更改通知接口 `System.ComponentModel.INotifyPropertyChanged` 的类型的使用方法。结合集合更改通知列表 `System.Collections.ObjectModel.ObservableCollection<T>`，可实现数据绑定到客户端。

目前提供三种方案：

* 方法提取：原生方法，读写效率高，需要定义属性对应的字段。
* 属性委托：使用字典存储数据，读写效率稍低，无需定义属性对应的字段。
* 运行时类型生成：基于反射发出生成的动态类型，读写效率高但需要较多时间生成类型，无需定义属性对应的字段。

## 方法提取

将属性更改通知的公用代码提取为方法，并在属性的 `set` 处调用。

### 抽象类 `XstarS.ComponentModel.ObservableObject`

`System.ComponentModel.INotifyPropertyChanged` 接口的实现，用于实现通知客户端属性更改的抽象类。

此类包含了 `SetProperty<T>(ref T, T, string)` 方法，应在设置属性更改通知的属性的 `set` 处调用此方法，实现更改属性值的同时通知客户端属性值发生更改。

### 方法使用说明

``` CSharp
using System.ComponentModel;
using XstarS.ComponentModel;

public class ObservableRectangle : ObservableObject
{
    public ObservableRectangle() { }

    private double width;
    public double Width
    {
        get => this.width;
        set
        {
            this.SetProperty(ref this.width, value);
            this.NotifyPropertyChanged(nameof(this.Size));
        }
    }

    private double height;
    public double Height
    {
        get => this.height;
        set
        {
            this.SetProperty(ref this.height, value);
            this.NotifyPropertyChanged(nameof(this.Size));
        }
    }

    public double Size => this.width * this.height;
}
```

当更改 `Width` 或 `Height` 属性时，会通知客户端 `Width` 或 `Height` 的值发生更改，并同时通知 `Size` 的值发生更改。

## 属性委托

将属性的存储交由字典实现，并调用相关方法实现属性的读写。

### 抽象类 `XstarS.ComponentModel.ObservableDataObject`

`System.ComponentModel.INotifyPropertyChanged` 接口的实现，用于实现通知客户端属性更改的抽象类。

此类的属性的值由一个字典存储，包含了 `GetProperty<T>(string)` 和 `SetProperty<T>(T, string)` 方法，应在设置属性更改通知的属性的 `get` 和 `set` 处分别调用以上方法，实现属性的读写，并在更改属性值的同时通知客户端属性值发生更改。

### 属性委托使用说明

``` CSharp
using System.ComponentModel;
using XstarS.ComponentModel;

public class ObservableRectangle : ObservableObject
{
    public ObservableRectangle() { }

    public double Width
    {
        get => this.GetProperty<double>();
        set
        {
            this.SetProperty(value);
            this.NotifyPropertyChanged(nameof(this.Size));
        }
    }

    public double Height
    {
        get => this.GetProperty<double>();
        set
        {
            this.SetProperty(value);
            this.NotifyPropertyChanged(nameof(this.Size));
        }
    }

    public double Size => this.width * this.height;
}
```

当更改 `Width` 或 `Height` 属性时，会通知客户端 `Width` 或 `Height` 的值发生更改，并同时通知 `Size` 的值发生更改。

## 运行时类型生成

定义一个原型基类或接口，通过反射发出 `System.Reflection.Emit` 在运行时生成派生类，并在派生类的属性中实现属性更改通知的相关代码。

### 泛型密封类 `XstarS.ComponentModel.ObservableFactory<T>`

提供原型类型对应的属性更改通知的派生类型，并提供创建此属性更改通知派生类型的实例的方法。

* `BaseType`: 获取原型类型。
* `ObservableType`: 获取属性更改通知的派生类型。
* `CreateInstance()`: 创建一个 `ObservableType` 的实例。
* `CreateInstance(object[])`: 以指定参数创建一个 `ObservableType` 的实例。

### 特性类 `XstarS.ComponentModel.RelatedPropertiesAttribute`

标识当前属性更改时会引起其他属性的更改。

构造函数 `RelatedPropertiesAttribute(string propertyNames)`，其中 `propertyNames` 即为当前属性更改时会更改的其他属性的名称，用半角逗号 (`,`) 隔开。

### 运行时类型生成使用说明

``` CSharp
using System.ComponentModel;
using XstarS.ComponentModel;

public abstract class ObservableRectangle : INotifyPropertyChanged
{
    public ObservableRectangle() { }

    [RelatedProperties(nameof(Size))]
    public abstract double Width { get; set; }

    [RelatedProperties(nameof(Size))]
    public abstract double Height { get; set; }

    public double Size => this.Width * this.Height;

    public abstract event PropertyChangedEventHandler PropertyChanged;

    public ObservableRectangle Create() =>
        ObservableFactory<ObservableRectangle>.Default.CreateInstance();
}
```

定义一个原型类或接口，若原型为一个类，应包含 `public` 访问级别的构造函数。创建实例时使用 `ObservableFactory<ObservableRectangle>.Default` 属性获取 `ObservableFactory<ObservableRectangle>` 的默认实例，再调用 `CreateInstance()` 方法构造基于原型类型 `ObservableRectangle` 的实例。

注意，若定义的原型为一个类 (`class`)，则应将设置更改通知的属性定义为 `virtual` 或 `abstract`，使得此属性能够在派生类中被重写。`ObservableFactory<T>` 不会对非 `virtual` 或 `abstract` 的属性生成属性更改通知代码。同时，若定义了 `PropertyChanged` 事件，应将其应定义为 `abstract`，或是定义一个事件触发方法 `OnPropertyChanged`，否则会导致无法正确构造派生类。

> 若基类中的属性或方法或未定义为 `virtual` 或 `abstract`，则在派生类仅隐藏了基类中对应的定义，并未重写。当派生类的实例声明为基类时，则会调用基类中定义的属性或方法。

由于设置了 `RelatedProperties` 特性，在更改 `Width` 或 `Height` 属性时，不仅会通知客户端 `Width` 或 `Height` 的值发生更改，也会通知客户端 `Size` 的值发生更改。
