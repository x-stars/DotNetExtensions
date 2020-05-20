# 属性更改通知实现方式比较

| 实现方式       | 代码行数 | 定义字段 |
| -------------- | -------- | -------- |
| 原始实现       | 57       | 是       |
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

    private double _Length;
    public double Length
    {
        get => this._Length;
        set
        {
            this._Length = value;
            this.NotifyPropertyChanged();
        }
    }

    private double _Width;
    public double Width
    {
        get => this._Width;
        set
        {
            this._Width = value;
            this.NotifyPropertyChanged();
        }
    }

    private double _Height;
    public double Height
    {
        get => this._Height;
        set
        {
            this._Height = value;
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
