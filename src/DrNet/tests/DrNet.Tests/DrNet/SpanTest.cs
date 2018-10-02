using System;

namespace DrNet.Tests
{
    public abstract class SpanTest<T>
    {
        protected abstract T NewT(int value);

        protected event Action<T, T> OnCompare;

        protected void DoOnCompare(T t1, T t2)
        {
            OnCompare?.Invoke(t1, t2);
        }

        protected bool EqualityCompareT(T t1, T t2)
        {
            if (t1 is IEquatable<T> equatable)
                return equatable.Equals(t2);
            return t1.Equals(t2);
        }
    }

    public abstract class SpanTest<T, TSource> : SpanTest<T>
    {
        protected abstract TSource NewTSource(T value, Action<T, T> onCompare = default);

        public static T AsT(TSource item)
        {
            if (item is T t)
                return t;
            if (item is TObject<T> o)
                return o.Value;
            if (item is TEquatable<T> e)
                return e.Value;
            throw new NotImplementedException();
        }

        protected bool EqualityCompareS(TSource s1, TSource s2)
        {
            T t1 = AsT(s1);
            T t2 = AsT(s2);

            DoOnCompare(t1, t2);
            return EqualityCompareT(t1, t2);
        }
    }

    public abstract class SpanTest<T, TSource, TValue> : SpanTest<T, TSource>
    {
        protected abstract TValue NewTValue(T value, Action<T, T> onCompare = default);

        public static T AsT(TValue item)
        {
            if (item is T t)
                return t;
            if (item is TObject<T> o)
                return o.Value;
            if (item is TEquatable<T> e)
                return e.Value;
            throw new NotImplementedException();
        }

        protected bool EqualityCompareSV(TSource s, TValue v)
        {
            T t1 = AsT(s);
            T t2 = AsT(v);

            DoOnCompare(t1, t2);
            return EqualityCompareT(t1, t2);
        }

        protected bool EqualityCompareVS(TValue v, TSource s)
        {
            T t1 = AsT(v);
            T t2 = AsT(s);

            DoOnCompare(t1, t2);
            return EqualityCompareT(t1, t2);
        }
    }
}
