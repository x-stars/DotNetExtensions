using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS.Linq
{
    /// <summary>
    /// �ṩ LINQ ��ѯ��������չ��
    /// </summary>
    public static class LinqExtensions
    {
#if NET461
        /// <summary>
        /// ��һ��ֵ���뵽����ĩβ��
        /// </summary>
        /// <typeparam name="TSource"><paramref name="source"/> �е�Ԫ�ص����͡�</typeparam>
        /// <param name="source">Ҫ��ĩβ����ֵ�����С�</param>
        /// <param name="element">Ҫ���뵽 <paramref name="source"/> ĩβ��ֵ��</param>
        /// <returns>�� <paramref name="element"/> ��β�������С�</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> Ϊ <see langword="null"/>��</exception>
        public static IEnumerable<TSource> Append<TSource>(
            this IEnumerable<TSource> source, TSource element)
        {
            return source.Concat(new[] { element });
        }
#endif

        /// <summary>
        /// ����һ��ֵ���뵽���е�ָ��λ�á�
        /// </summary>
        /// <typeparam name="TSource">���������е�Ԫ�ص����͡�</typeparam>
        /// <param name="source">Ҫ����ֵ�����С�</param>
        /// <param name="index">Ҫ�� <paramref name="source"/> �в���ֵ��λ�á�</param>
        /// <param name="element">Ҫ���뵽 <paramref name="source"/> ��ֵ��</param>
        /// <returns>�� <paramref name="source"/> �� <paramref name="index"/>
        /// ������ <paramref name="element"/> �õ��������С�</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> Ϊ <see langword="null"/>��</exception>
        public static IEnumerable<TSource> Insert<TSource>(
            this IEnumerable<TSource> source, int index, TSource element)
        {
            return source.Take(index).Append(element).Concat(source.Skip(index));
        }

        /// <summary>
        /// ��һ��ֵ���뵽���п�ͷ��
        /// </summary>
        /// <typeparam name="TSource"><paramref name="source"/> �е�Ԫ�ص����͡�</typeparam>
        /// <param name="source">Ҫ�ڿ�ͷ����ֵ�����С�</param>
        /// <param name="element">Ҫ���뵽 <paramref name="source"/> ��ͷ��ֵ��</param>
        /// <returns>�� <paramref name="element"/> ��ͷ�������С�</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> Ϊ <see langword="null"/>��</exception>
        public static IEnumerable<TSource> InsertHead<TSource>(
            this IEnumerable<TSource> source, TSource element)
        {
            return (new[] { element }).Concat(source);
        }

        /// <summary>
        /// ����һ�����в��뵽���е�ָ��λ�á�
        /// </summary>
        /// <typeparam name="TSource">���������е�Ԫ�ص����͡�</typeparam>
        /// <param name="source">Ҫ������һ���е����С�</param>
        /// <param name="index">Ҫ�� <paramref name="source"/> �в������е�λ�á�</param>
        /// <param name="other">Ҫ���뵽 <paramref name="source"/> �����С�</param>
        /// <returns>�� <paramref name="source"/> �� <paramref name="index"/>
        /// ������ <paramref name="other"/> �õ��������С�</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// �� <paramref name="other"/> Ϊ <see langword="null"/>��</exception>
        public static IEnumerable<TSource> InsertRange<TSource>(
            this IEnumerable<TSource> source, int index, IEnumerable<TSource> other)
        {
            return source.Take(index).Concat(other).Concat(source.Skip(index));
        }

        /// <summary>
        /// ����ǰ��������һ���е�Ԫ�ض�Ӧ���ӣ��õ� 2 Ԫ��Ľ�����С�
        /// </summary>
        /// <typeparam name="T1">��ǰ���е�Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T2">��һ���е�Ԫ�ص����͡�</typeparam>
        /// <param name="source">Ҫ����һ���е�Ԫ�ض�Ӧ���ӵ����С�</param>
        /// <param name="other">Ҫ�뵱ǰ���е�Ԫ�ض�Ӧ���ӵ����С�</param>
        /// <returns>һ������ <paramref name="source"/> ��
        /// <paramref name="other"/> �ж�ӦԪ����ɵ� 2 Ԫ������С�</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// �� <paramref name="other"/> Ϊ <see langword="null"/>��</exception>
        public static IEnumerable<(T1, T2)> Zip<T1, T2>(
            this IEnumerable<T1> source, IEnumerable<T2> other)
        {
            return Enumerable.Zip(source, other, ValueTuple.Create);
        }

        /// <summary>
        /// ����ǰ 2 Ԫ����������һ���е�Ԫ�ض�Ӧ���ӣ��õ� 3 Ԫ��Ľ�����С�
        /// </summary>
        /// <typeparam name="T1">��ǰ���еĵ� 1 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T2">��ǰ���еĵ� 2 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T3">��һ���е�Ԫ�ص����͡�</typeparam>
        /// <param name="source">Ҫ����һ���е�Ԫ�ض�Ӧ���ӵ� 2 Ԫ�����С�</param>
        /// <param name="other">Ҫ�뵱ǰ 2 Ԫ�����е�Ԫ�ض�Ӧ���ӵ����С�</param>
        /// <returns>һ������ <paramref name="source"/> ��
        /// <paramref name="other"/> �ж�ӦԪ����ɵ� 3 Ԫ������С�</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// �� <paramref name="other"/> Ϊ <see langword="null"/>��</exception>
        public static IEnumerable<(T1, T2, T3)> Zip<T1, T2, T3>(
            this IEnumerable<(T1, T2)> source, IEnumerable<T3> other)
        {
            return Enumerable.Zip(source, other, TupleOperators.Append);
        }

        /// <summary>
        /// ����ǰ 3 Ԫ����������һ���е�Ԫ�ض�Ӧ���ӣ��õ� 4 Ԫ��Ľ�����С�
        /// </summary>
        /// <typeparam name="T1">��ǰ���еĵ� 1 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T2">��ǰ���еĵ� 2 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T3">��ǰ���еĵ� 3 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T4">��һ���е�Ԫ�ص����͡�</typeparam>
        /// <param name="source">Ҫ����һ���е�Ԫ�ض�Ӧ���ӵ� 3 Ԫ�����С�</param>
        /// <param name="other">Ҫ�뵱ǰ 3 Ԫ�����е�Ԫ�ض�Ӧ���ӵ����С�</param>
        /// <returns>һ������ <paramref name="source"/> ��
        /// <paramref name="other"/> �ж�ӦԪ����ɵ� 4 Ԫ������С�</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// �� <paramref name="other"/> Ϊ <see langword="null"/>��</exception>
        public static IEnumerable<(T1, T2, T3, T4)> Zip<T1, T2, T3, T4>(
            this IEnumerable<(T1, T2, T3)> source, IEnumerable<T4> other)
        {
            return Enumerable.Zip(source, other, TupleOperators.Append);
        }

        /// <summary>
        /// ����ǰ 4 Ԫ����������һ���е�Ԫ�ض�Ӧ���ӣ��õ� 5 Ԫ��Ľ�����С�
        /// </summary>
        /// <typeparam name="T1">��ǰ���еĵ� 1 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T2">��ǰ���еĵ� 2 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T3">��ǰ���еĵ� 3 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T4">��ǰ���еĵ� 4 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T5">��һ���е�Ԫ�ص����͡�</typeparam>
        /// <param name="source">Ҫ����һ���е�Ԫ�ض�Ӧ���ӵ� 4 Ԫ�����С�</param>
        /// <param name="other">Ҫ�뵱ǰ 4 Ԫ�����е�Ԫ�ض�Ӧ���ӵ����С�</param>
        /// <returns>һ������ <paramref name="source"/> ��
        /// <paramref name="other"/> �ж�ӦԪ����ɵ� 5 Ԫ������С�</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// �� <paramref name="other"/> Ϊ <see langword="null"/>��</exception>
        public static IEnumerable<(T1, T2, T3, T4, T5)> Zip<T1, T2, T3, T4, T5>(
            this IEnumerable<(T1, T2, T3, T4)> source, IEnumerable<T5> other)
        {
            return Enumerable.Zip(source, other, TupleOperators.Append);
        }

        /// <summary>
        /// ����ǰ 5 Ԫ����������һ���е�Ԫ�ض�Ӧ���ӣ��õ� 6 Ԫ��Ľ�����С�
        /// </summary>
        /// <typeparam name="T1">��ǰ���еĵ� 1 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T2">��ǰ���еĵ� 2 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T3">��ǰ���еĵ� 3 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T4">��ǰ���еĵ� 4 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T5">��ǰ���еĵ� 5 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T6">��һ���е�Ԫ�ص����͡�</typeparam>
        /// <param name="source">Ҫ����һ���е�Ԫ�ض�Ӧ���ӵ� 5 Ԫ�����С�</param>
        /// <param name="other">Ҫ�뵱ǰ 5 Ԫ�����е�Ԫ�ض�Ӧ���ӵ����С�</param>
        /// <returns>һ������ <paramref name="source"/> ��
        /// <paramref name="other"/> �ж�ӦԪ����ɵ� 6 Ԫ������С�</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// �� <paramref name="other"/> Ϊ <see langword="null"/>��</exception>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6)> Zip<T1, T2, T3, T4, T5, T6>(
            this IEnumerable<(T1, T2, T3, T4, T5)> source, IEnumerable<T6> other)
        {
            return Enumerable.Zip(source, other, TupleOperators.Append);
        }

        /// <summary>
        /// ����ǰ 6 Ԫ����������һ���е�Ԫ�ض�Ӧ���ӣ��õ� 7 Ԫ��Ľ�����С�
        /// </summary>
        /// <typeparam name="T1">��ǰ���еĵ� 1 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T2">��ǰ���еĵ� 2 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T3">��ǰ���еĵ� 3 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T4">��ǰ���еĵ� 4 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T5">��ǰ���еĵ� 5 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T6">��ǰ���еĵ� 6 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T7">��һ���е�Ԫ�ص����͡�</typeparam>
        /// <param name="source">Ҫ����һ���е�Ԫ�ض�Ӧ���ӵ� 6 Ԫ�����С�</param>
        /// <param name="other">Ҫ�뵱ǰ 6 Ԫ�����е�Ԫ�ض�Ӧ���ӵ����С�</param>
        /// <returns>һ������ <paramref name="source"/> ��
        /// <paramref name="other"/> �ж�ӦԪ����ɵ� 7 Ԫ������С�</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// �� <paramref name="other"/> Ϊ <see langword="null"/>��</exception>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Zip<T1, T2, T3, T4, T5, T6, T7>(
            this IEnumerable<(T1, T2, T3, T4, T5, T6)> source, IEnumerable<T7> other)
        {
            return Enumerable.Zip(source, other, TupleOperators.Append);
        }

        /// <summary>
        /// ����ǰ 7 Ԫ����������һ���е�Ԫ�ض�Ӧ���ӣ��õ� 8 Ԫ��Ľ�����С�
        /// </summary>
        /// <typeparam name="T1">��ǰ���еĵ� 1 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T2">��ǰ���еĵ� 2 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T3">��ǰ���еĵ� 3 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T4">��ǰ���еĵ� 4 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T5">��ǰ���еĵ� 5 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T6">��ǰ���еĵ� 6 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T7">��ǰ���еĵ� 7 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T8">��һ���е�Ԫ�ص����͡�</typeparam>
        /// <param name="source">Ҫ����һ���е�Ԫ�ض�Ӧ���ӵ� 7 Ԫ�����С�</param>
        /// <param name="other">Ҫ�뵱ǰ 7 Ԫ�����е�Ԫ�ض�Ӧ���ӵ����С�</param>
        /// <returns>һ������ <paramref name="source"/> ��
        /// <paramref name="other"/> �ж�ӦԪ����ɵ� 8 Ԫ������С�</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// �� <paramref name="other"/> Ϊ <see langword="null"/>��</exception>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Zip<T1, T2, T3, T4, T5, T6, T7, T8>(
            this IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> source, IEnumerable<T8> other)
        {
            return Enumerable.Zip(source, other, TupleOperators.Append);
        }

        /// <summary>
        /// ����ǰ 7 Ԫ����������һԪ�����е�Ԫ�ض�Ӧ���ӣ��õ� n Ԫ��Ľ�����С�
        /// </summary>
        /// <typeparam name="T1">��ǰ���еĵ� 1 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T2">��ǰ���еĵ� 2 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T3">��ǰ���еĵ� 3 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T4">��ǰ���еĵ� 4 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T5">��ǰ���еĵ� 5 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T6">��ǰ���еĵ� 6 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="T7">��ǰ���еĵ� 7 ��Ԫ�ص����͡�</typeparam>
        /// <typeparam name="TRest">��һԪ�����е�Ԫ������͡�</typeparam>
        /// <param name="source">Ҫ����һԪ�����е�Ԫ�ض�Ӧ���ӵ� 7 Ԫ�����С�</param>
        /// <param name="other">Ҫ�뵱ǰ 7 Ԫ�����е�Ԫ�ض�Ӧ���ӵ�Ԫ�����С�</param>
        /// <returns>һ������ <paramref name="source"/> ��
        /// <paramref name="other"/> �ж�ӦԪ����ɵ� n Ԫ������С�</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// �� <paramref name="other"/> Ϊ <see langword="null"/>��</exception>
        public static IEnumerable<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>> ZipChain<T1, T2, T3, T4, T5, T6, T7, TRest>(
            this IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> source, IEnumerable<TRest> other) where TRest : struct
        {
            return Enumerable.Zip(source, other, TupleOperators.Concat);
        }
    }
}
