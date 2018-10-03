using System;

namespace DrNet.Tests
{
    public abstract class SpanTest<T>: IDisposable
    {
        protected abstract T NewT(int value);

        protected int handle;

        protected bool EqualityCompareT(T t1, T t2)
        {
            if (t1 is IEquatable<T> equatable)
                return equatable.Equals(t2);
            return t1.Equals(t2);
        }

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

            OnCompareActions<T>.OnCompare(handle, t1, t2);
            return EqualityCompareT(t1, t2);
        }
    }

    public abstract class SpanTest<T, TSource, TValue> : SpanTest<T, TSource>
    {
        protected abstract TValue NewTValue(T value, int handle = 0);

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

            OnCompareActions<T>.OnCompare(handle, t1, t2);
            return EqualityCompareT(t1, t2);
        }

        protected bool EqualityCompareVS(TValue v, TSource s)
        {
            T t1 = AsT(v);
            T t2 = AsT(s);

            OnCompareActions<T>.OnCompare(handle, t1, t2);
            return EqualityCompareT(t1, t2);
        }

        protected bool IsLogSupported()
        {
            bool sourceWithLog = typeof(TSource) == typeof(TObject<T>) || typeof(TSource) == typeof(TEquatable<T>);
            bool valueWithLog = typeof(TValue) == typeof(TObject<T>) || typeof(TValue) == typeof(TEquatable<T>);
            return sourceWithLog || valueWithLog;
        }
    }
}
