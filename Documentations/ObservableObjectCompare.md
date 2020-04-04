# 属性更改通知实现方式比较

| 实现方式       | 代码行数 | 定义字段 |
| -------------- | -------- | -------- |
| 原始实现       | 57       | 是       |
| 方法提取       | 31       | 是       |
| 属性委托       | 28       | 否       |
| 运行时类型生成 | 22       | 否       |

> 相关实现方式的使用说明参看[属性更改通知接口实现框架说明文档](ObservableObject.md)。

## 原始实现

不使用任何框架，直接实现 `System.ComponentModel.INotifyPropertyChanged` 接口。

``` CSharp
using System.ComponentModel;
using System.Runtime.CompilerServices;

// 类型定义。
public class ObservableBox : INotifyPropertyChanged
{
    public ObservableBox() { }

    private double length;
    public double Length
    {
        get => this.length;
        set
        {
            this.length = value;
            this.NotifyPropertyChanged();
        }
    }

    private double width;
    public double Width
    {
        get => this.width;
        set
        {
            this.width = value;
            this.NotifyPropertyChanged();
        }
    }

    private double height;
    public double Height
    {
        get => this.height;
        set
        {
            this.height = value;
            this.NotifyPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void NotifyPropertyChanged(
        [CallerMemberName] string propertyName = null)
    {
        this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

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
using XstarS.ComponentModel;

// 类型定义。
public class ObservableBox : ObservableObject
{
    public ObservableBox() { }

    private double length;
    public double Length
    {
        get => this.length;
        set => this.SetProperty(ref this.length, value);
    }

    private double width;
    public double Width
    {
        get => this.width;
        set => this.SetProperty(ref this.width, value);
    }

    private double height;
    public double Height
    {
        get => this.height;
        set => this.SetProperty(ref this.height, value);
    }
}

// 创建实例。
new ObservableBox();
```

## 属性委托

继承 `XstarS.ComponentModel.ObservableDataObject` 抽象类以使用 `GetProperty` 和 `SetProperty` 方法。

``` CSharp
using XstarS.ComponentModel;

// 类型定义。
public class ObservableBox : ObservableDataObject
{
    public ObservableBox() { }

    public double Length
    {
        get => this.GetProperty<double>();
        set => this.SetProperty(value);
    }

    public double Width
    {
        get => this.GetProperty<double>();
        set => this.SetProperty(value);
    }

    public double Height
    {
        get => this.GetProperty<double>();
        set => this.SetProperty(value);
    }
}

// 创建实例。
new ObservableBox();
```

## 运行时类型生成

使用 `XstarS.ComponentModel.ObservableFactory<T>` 在运行时生成属性更改通知派生类。

``` CSharp
using System.ComponentModel;
using XstarS.ComponentModel;

// 原型定义。
public abstract class ObservableBox : INotifyPropertyChanged
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
