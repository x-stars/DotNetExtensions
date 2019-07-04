# 数据绑定实现方式比较

相关实现请参看[数据绑定接口实现框架说明文档](BindableValue.md)，此处不再赘述。

以下各种方法实现数据绑定接口的对外表现如无特殊说明均完全相同，但代码量却逐渐减少。

| 实现方式                      | 代码行数 | 可维护性 |
| ----------------------------- | -------- | -------- |
| 原始实现                      | 61       | 弱       |
| 方法提取                      | 34       | 中       |
| 绑定值封装 (对外表现不同)     | 20       | 中       |
| 运行时代码自动生成 (非密封类) | 21       | 极强     |
| 运行时代码自动生成 (接口)     | 14       | 强       |

## 原始实现

不使用任何框架，直接实现 `System.ComponentModel.INotifyPropertyChanged` 接口。

``` CSharp
using System.Collections.Generic;
using System.ComponentModel;

// 类型定义。
public class BindableBox : INotifyPropertyChanged
{
    private double length;
    private double width;
    private double height;

    public BindableBox() { }

    public double Length
    {
        get => this.length;
        set
        {
            if (!EqualityComparer<double>.Default.Equals(this.length, value))
            {
                this.length = value;
                this.OnPropertyChanged(nameof(this.Length));
            }
        }
    }

    public double Width
    {
        get => this.width;
        set
        {
            if (!EqualityComparer<double>.Default.Equals(this.width, value))
            {
                this.width = value;
                this.OnPropertyChanged(nameof(this.Width));
            }
        }
    }

    public double Height
    {
        get => this.height;
        set
        {
            if (!EqualityComparer<double>.Default.Equals(this.height, value))
            {
                this.height = value;
                this.OnPropertyChanged(nameof(this.Height));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

// 创建实例。
new BindableBox();
```

## 方法提取

继承 `XstarS.ComponentModel.BindableObject` 抽象类以使用 `SetProperty` 方法。

``` CSharp
using System.Collections.Generic;
using System.ComponentModel;
using XstarS.ComponentModel;

// 类型定义。
public class BindableBox : BindableObject
{
    private double length;
    private double width;
    private double height;

    public BindableBox() { }

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
new BindableBox();
```

## 绑定值封装

使用 `XstarS.ComponentModel.Bindable<T>` 封装要设置绑定的属性。

``` CSharp
using System.ComponentModel;
using XstarS.ComponentModel;

// 类型定义。
public class BindableBox
{
    public BindableBox()
    {
        this.Length = new Bindable<double>();
        this.Width = new Bindable<double>();
        this.Height = new Bindable<double>();
    }

    public Bindable<double> Length { get; }
    public Bindable<double> Width { get; }
    public Bindable<double> Height { get; }
}

// 创建实例。
new BindableBox();
```

此方法实现的数据绑定的表现与其他方法实现的并不相同，设置绑定目标时，应设置到 `(属性名称).Value`。
这造成了维护的不便，一般仅在需要绑定的属性较少，且相互之间不形成结构时使用。

## 运行时代码自动生成

使用 `XstarS.ComponentModel.BindableBuilder<T>` 动态生成数据绑定派生类。

### 非密封类方式

``` CSharp
using System.ComponentModel;
using XstarS.ComponentModel;

// 原型定义。
public abstract class BindableBox
    : INotifyPropertyChanged  // 可不写。
{
    public BindableBox() { }

    public abstract double Length { get; set; }
    public abstract double Width { get; set; }
    public abstract double Height { get; set; }

    public abstract event PropertyChangedEventHandler PropertyChanged;

    public static BindableBox Create() =>
        BindableBuilder<BindableBox>.Default.CreateInstance();
}

// 创建实例。
BindableBox.Create();
```

此实现无需在类外调用 `BindableBuilder`，可维护性较强。

### 接口方式

``` CSharp
using System.ComponentModel;
using XstarS.ComponentModel;

// 原型定义。
public interface IBindableBox
    : INotifyPropertyChanged  // 可不写。
{
    double Length { get; set; }
    double Width { get; set; }
    double Height { get; set; }
}

// 创建实例。
BindableBuilder<IBindableBox>.Default.CreateInstance();
```

此实现需要在类外调用 `BindableBuilder` 以创建实例，可维护性比非密封类稍差。
