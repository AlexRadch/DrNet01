using System;
using System.Collections.Generic;
using System.Linq;

using DrNet.Linq;

namespace DrNet.Tests
{
    public abstract class SpanTest<T>: IDisposable
    {
        protected int handle;

        protected int GetHandle(int handle) => handle == int.MaxValue ? this.handle : handle;

        protected abstract T NewT(int value);

        protected bool EqualityCompareT(T t1, T t2, bool creating)
        {
            if (!creating)
                OnCompareActions<T>.OnCompare(handle, t1, t2);

            if (t1 is IEquatable<T> equatable)
                return equatable.Equals(t2);
            return t1.Equals(t2);
        }

        protected bool EqualityCompareT(T t1, T t2) => EqualityCompareT(t1, t2, false);

        protected bool IsLogSupported() => IsLogSupported(typeof(T));

        protected static bool IsLogSupported(Type t) => t == typeof(TObject<T>) || t == typeof(TEquatable<T>) || 
            t == typeof(TEquatableInt);

        protected T NextT(Random random) => NewT(random.Next(int.MinValue, int.MaxValue));

        protected IEnumerable<T> RepeatT(Random random) => DrNetEnumerable.Repeat(() => NextT(random));

        protected IEnumerable<T> WhereNotEqualT(IEnumerable<T> source, T value) => source.Where(t => 
            {
                try
                {
                    return !EqualityCompareT(value, t, true);
                }
                catch
                {
                    return true;
                }
            });

        protected T NextNotEqualT(Random random, T value) => WhereNotEqualT(RepeatT(random), value).First();

        #region IDisposable Support

        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;

                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                if (handle > 0)
                    OnCompareActions<T>.RemoveHandler(handle);
                handle = 0;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        ~SpanTest()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(false);
        }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    public abstract class SpanTest<T, TSource> : SpanTest<T>
    {
        protected abstract TSource NewTSource(T value, int handle = 0);

        protected static T AsT(TSource item)
        {
            if (item is T t)
                return t;
            if (item is TObject<T> o)
                return o.Value;
            if (item is TEquatable<T> e)
                return e.Value;
            throw new NotImplementedException();
        }

        protected bool EqualityCompareS(TSource s1, TSource s2, bool creating) => 
            EqualityCompareT(AsT(s1), AsT(s2), creating);

        protected bool EqualityCompareS(TSource s1, TSource s2) => EqualityCompareS(s1, s2, false);

        protected new bool IsLogSupported() => IsLogSupported(typeof(TSource));

        protected TSource NextS(Random random, int handle = int.MaxValue) => 
            NewTSource(NextT(random), GetHandle(handle));

        protected TSource NextNotDefaultS(Random random, int handle = int.MaxValue) =>
            NewTSource(NextNotEqualT(random, default), GetHandle(handle));
    }

    public abstract class SpanTest<T, TSource, TValue> : SpanTest<T, TSource>
    {
        protected abstract TValue NewTValue(T value, int handle = 0);

        protected static T AsT(TValue item)
        {
            if (item is T t)
                return t;
            if (item is TObject<T> o)
                return o.Value;
            if (item is TEquatable<T> e)
                return e.Value;
            throw new NotImplementedException();
        }

        protected bool EqualityCompareSV(TSource s, TValue v, bool creating) =>
            EqualityCompareT(AsT(s), AsT(v), creating);

        protected bool EqualityCompareSV(TSource s, TValue v) => EqualityCompareSV(s, v, false);

        protected bool EqualityCompareVS(TValue v, TSource s, bool creating) =>
            EqualityCompareT(AsT(v), AsT(s), creating);

        protected bool EqualityCompareVS(TValue v, TSource s) => EqualityCompareVS(v, s, false);

        protected bool EqualityCompareV(TValue v1, TValue v2, bool creating) =>
            EqualityCompareT(AsT(v1), AsT(v2), creating);

        protected bool EqualityCompareV(TValue v1, TValue v2) => EqualityCompareV(v1, v2, false);

        protected new bool IsLogSupported() => base.IsLogSupported() || IsLogSupported(typeof(TValue));

        protected TValue NextV(Random random, int handle = int.MaxValue) => NewTValue(NextT(random), GetHandle(handle));

        protected TValue NextNotDefaultV(Random random, int handle = int.MaxValue) =>
            NewTValue(NextNotEqualT(random, default), GetHandle(handle)); 
    }
}
