using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 表示枚举类型的列表视图。
    /// </summary>
    /// <typeparam name="TEnum">枚举的类型。</typeparam>
    [Serializable]
    public class EnumListView<TEnum> : ReadOnlyObservableCollection<TEnum>
        where TEnum : struct, Enum
    {
        /// <summary>
        /// 表示当前选中的枚举值的索引。
        /// </summary>
        private int _SelectedIndex;

        /// <summary>
        /// 初始化 <see cref="EnumListView{TEnum}"/> 类的新实例。
        /// </summary>
        public EnumListView() : base(new ObservableCollection<TEnum>())
        {
            foreach (var value in Enum.GetValues(typeof(TEnum)))
            {
                this.Items.Add((TEnum)value);
            }
        }

        /// <summary>
        /// 获取或设置当前选中的枚举值的索引。
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/>
        /// 不在 -1 和 <see cref="Collection{T}.Count"/> - 1 之间。</exception>
        public int SelectedIndex
        {
            get => this._SelectedIndex;
            set => this.SetSelectedIndex(value);
        }

        /// <summary>
        /// 获取或设置当前选中的枚举值。
        /// </summary>
        public TEnum SelectedItem
        {
            get => this[this.SelectedIndex];
            set => this.SelectedIndex = this.IndexOf(value);
        }

        /// <summary>
        /// 设置当前选中的枚举值的索引。
        /// </summary>
        /// <param name="index">要设置的选中的枚举值的索引。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>
        /// 不在 -1 和 <see cref="Collection{T}.Count"/> - 1 之间。</exception>
        protected virtual void SetSelectedIndex(int index)
        {
            if (index == -1) { index = 0; }
            if ((index < 0) || (index >= this.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            this._SelectedIndex = index;
            this.OnPropertyChanged(
                new PropertyChangedEventArgs(nameof(this.SelectedIndex)));
            this.OnPropertyChanged(
                new PropertyChangedEventArgs(nameof(this.SelectedItem)));
        }

        /// <summary>
        /// 将此实例的值转换为其等效的字符串表示形式。
        /// </summary>
        /// <returns>此实例的值的字符串表示形式。</returns>
        public override string ToString()
        {
            var index = this.SelectedIndex;
            var names = Enum.GetNames(typeof(TEnum));
            names[index] = $"*{names[index]}";
            return string.Join(", ", names);
        }
    }
}
