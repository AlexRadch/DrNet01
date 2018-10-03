using System;
using System.Collections.Generic;

namespace DrNet.Tests
{
    public class OnCompareActions<T>
    {
        private static readonly List<Action<T, T>> Actions = new List<Action<T, T>>();

        public static int CreateHandler(Action<T, T> action)
        {
            if (action == null)
                action = (T x, T y) => { };
            int handle;
            lock (Actions)
            {
                handle = Actions.IndexOf(null);
                if (handle < 0)
                {
                    Actions.Add(action);
                    handle = Actions.Count - 1;
                }
                else
                    Actions[handle] = action;
            }

            return handle + 1;
        }

        public static void RemoveHandler(int handle)
        {
            lock (Actions)
            {
                Actions[handle - 1] = null;
            }
        }

        public static void Add(int handle, Action<T, T> action)
        {
            if (action != null)
            {
                lock (Actions)
                {
                    Action<T, T> actions = Actions[handle - 1];
                    actions += action;
                    Actions[handle - 1] = actions;
                }
            }
        }

        public static void Remove(int handle, Action<T, T> action)
        {
            if (action != null)
            {
                lock (Actions)
                {
                    Action<T, T> actions = Actions[handle - 1];
                    actions -= action;
                    Actions[handle - 1] = actions;
                }
            }
        }

        public static void OnCompare(int handle, T x, T y)
        {
            if (handle > 0)
                Actions[handle - 1]?.Invoke(x, y);
        }
    }
}
