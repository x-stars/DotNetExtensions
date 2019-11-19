# 属性更改通知实现方式比较

相关实现请参看[属性更改通知接口实现框架说明文档](ObservableValue.md)，此处不再赘述。

以下各种方法实现属性更改通知接口的对外表现如无特殊说明均完全相同，但代码量却逐渐减少。

| 实现方式                      | 代码行数 | 可维护性 |
| ----------------------------- | -------- | -------- |
| 原始实现                      | 55       | 弱       |
| 方法提取                      | 34       | 中       |
| 值封装 (对外表现不同)         | 20       | 中       |
| 运行时代码自动生成 (非密封类) | 21       | 极强     |
| 运行时代码自动生成 (接口)     | 14       | 强       |

## 原始实现

不使用任何框架，直接实现 `System.ComponentModel.INotifyPropertyChanged` 接口。

``` CSharp
using System.Collections.Generic;
using System.ComponentModel;

// 类型定义。
public class ObservableBox : INotifyPropertyChanged
{
    private double length;
    private double width;
    private double height;

    public ObservableBox() { }

    public double Length
    {
        get => this.length;
        set
        {
            this.length = value;
            this.OnPropertyChanged(
                new PropertyChangedEventArgs(nameof(this.Length)));
        }
    }

    public double Width
    {
        get => this.width;
        set
        {
            this.width = value;
            this.OnPropertyChanged(
                new PropertyChangedEventArgs(nameof(this.Width)));
        }
    }

    public double Height
    {
        get => this.height;
        set
        {
            this.height = value;
            this.OnPropertyChanged(
                new PropertyChangedEventArgs(nameof(this.Height)));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        this.PropertyChanged?.Invoke(this, e);
    }
}

// 创建实例。
new ObservableBox();
```

## 方法提取

继承 `XstarS.ComponentModel.ObservableObject` 抽象类以使用 `SetProperty` 方法。

``` CSharp
using System.Collections.Generic;
using System.ComponentModel;
using XstarS.ComponentModel;

// 类型定义。
public class ObservableBox : ObservableObject
{
    private double length;
    private double width;
    private double height;

    public ObservableBox() { }

    public double Length
    {
        get => this.length;
        set => this.SetProperty(ref this.length, value);
    }

    public double Width
    {
        get => this.width;
        set => this.SetProperty(ref this.width, value);
    }

    public double Height
    {
        get => this.height;
        set => this.SetProperty(ref this.height, value);
    }
}

// 创建实例。
new ObservableBox();
```

## 值封装

使用 `XstarS.ComponentModel.Observable<T>` 封装要实现更改通知的值。

``` CSharp
using System.ComponentModel;
using XstarS.ComponentModel;

// 类型定义。
public class ObservableBox
{
    public ObservableBox()
    {
        this.Length = new Observable<double>();
        this.Width = new Observable<double>();
        this.Height = new Observable<double>();
    }

    public Observable<double> Length { get; }
    public Observable<double> Width { get; }
    public Observable<double> Height { get; }
}

// 创建实例。
new ObservableBox();
```

此方法实现的属性更改通知的表现与其他方法实现的并不相同，若要设置数据绑定到客户端，路径应为 `(属性名称).Value`。

## 运行时代码自动生成

使用 `XstarS.ComponentModel.ObservableFactory<T>` 动态生成属性更改通知派生类。

### 非密封类方式

``` CSharp
using System.ComponentModel;
using XstarS.ComponentModel;

// 原型定义。
public abstract class ObservableBox
    : INotifyPropertyChanged  // 可不写。
{
    public ObservableBox() { }

    public abstract double Length { get; set; }
    public abstract double Width { get; set; }
    public abstract double Height { get; set; }

    public abstract event PropertyChangedEventHandler PropertyChanged;

    public static ObservableBox Create() =>
        ObservableFactory<ObservableBox>.Default.CreateInstance();
}

// 创建实例。
ObservableBox.Create();
```

此实现无需在类外调用 `ObservableFactory`，可维护性较强。

### 接口方式

``` CSharp
using System.ComponentModel;
using XstarS.ComponentModel;

// 原型定义。
public interface IObservableBox
    : INotifyPropertyChanged  // 可不写。
{
    double Length { get; set; }
    double Width { get; set; }
    double Height { get; set; }
}

// 创建实例。
ObservableFactory<IObservableBox>.Default.CreateInstance();
```

此实现需要在类外调用 `ObservableFactory` 以创建实例，可维护性比非密封类稍差。
