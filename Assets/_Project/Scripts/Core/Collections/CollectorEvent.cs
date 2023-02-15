using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    public class CollectorEvent<TResult>
    {
        private event Func<TResult> _internalEvent;

        public void Subscribe(Func<TResult> func)
        {
            _internalEvent += func;
        }
        public void Unsubscribe(Func<TResult> func)
        {
            _internalEvent -= func;
        }
        public void Clear()
        {
            _internalEvent = null;
        }

        public TResult[] Invoke()
        {
            if (_internalEvent is null)
                return new TResult[0];

            Delegate[] invocationList = _internalEvent.GetInvocationList();
            TResult[] results         = new TResult[invocationList.Length];

            for (int i = 0; i < invocationList.Length; i++)
            {
                var func = (Func<TResult>)invocationList[i];
                results.SetValue(func.Invoke(), i);
            }

            return results;
        }

        public static CollectorEvent<TResult> operator +(CollectorEvent<TResult> collectorEvent, Func<TResult> func)
        {
            collectorEvent._internalEvent += func;
            return collectorEvent;
        }
        public static CollectorEvent<TResult> operator -(CollectorEvent<TResult> collectorEvent, Func<TResult> func)
        {
            collectorEvent._internalEvent -= func;
            return collectorEvent;
        }
    }

    public class CollectorEvent<T, TResult>
    {
        private event Func<T, TResult> _internalEvent;

        public void Subscribe(Func<T, TResult> func)
        {
            _internalEvent += func;
        }
        public void Unsubscribe(Func<T, TResult> func)
        {
            _internalEvent -= func;
        }
        public void Clear()
        {
            _internalEvent = null;
        }

        public TResult[] Invoke(T arg)
        {
            if (_internalEvent is null)
                return new TResult[0];

            Delegate[] invocationList = _internalEvent.GetInvocationList();
            TResult[] results         = new TResult[invocationList.Length];

            for (int i = 0; i < invocationList.Length; i++)
            {
                var func = (Func<T, TResult>)invocationList[i];
                results.SetValue(func.Invoke(arg), i);
            }

            return results;
        }

        public static CollectorEvent<T, TResult> operator +(CollectorEvent<T, TResult> collectorEvent, Func<T, TResult> func)
        {
            collectorEvent._internalEvent += func;
            return collectorEvent;
        }
        public static CollectorEvent<T, TResult> operator -(CollectorEvent<T, TResult> collectorEvent, Func<T, TResult> func)
        {
            collectorEvent._internalEvent -= func;
            return collectorEvent;
        }
    }

    public class CollectorEvent<T1, T2, TResult>
    {
        private event Func<T1, T2, TResult> _internalEvent;

        public void Subscribe(Func<T1, T2, TResult> func)
        {
            _internalEvent += func;
        }
        public void Unsubscribe(Func<T1, T2, TResult> func)
        {
            _internalEvent -= func;
        }
        public void Clear()
        {
            _internalEvent = null;
        }

        public TResult[] Invoke(T1 arg1, T2 arg2)
        {
            if (_internalEvent is null)
                return new TResult[0];

            Delegate[] invocationList = _internalEvent.GetInvocationList();
            TResult[] results         = new TResult[invocationList.Length];

            for (int i = 0; i < invocationList.Length; i++)
            {
                var func = (Func<T1, T2, TResult>)invocationList[i];
                results.SetValue(func.Invoke(arg1, arg2), i);
            }

            return results;
        }

        public static CollectorEvent<T1, T2, TResult> operator +(CollectorEvent<T1, T2, TResult> collectorEvent, Func<T1, T2, TResult> func)
        {
            collectorEvent._internalEvent += func;
            return collectorEvent;
        }
        public static CollectorEvent<T1, T2, TResult> operator -(CollectorEvent<T1, T2, TResult> collectorEvent, Func<T1, T2, TResult> func)
        {
            collectorEvent._internalEvent -= func;
            return collectorEvent;
        }
    }

    public class CollectorEvent<T1, T2, T3, TResult>
    {
        private event Func<T1, T2, T3, TResult> _internalEvent;

        public void Subscribe(Func<T1, T2, T3, TResult> func)
        {
            _internalEvent += func;
        }
        public void Unsubscribe(Func<T1, T2, T3, TResult> func)
        {
            _internalEvent -= func;
        }
        public void Clear()
        {
            _internalEvent = null;
        }

        public TResult[] Invoke(T1 arg1, T2 arg2, T3 arg3)
        {
            if (_internalEvent is null)
                return new TResult[0];

            Delegate[] invocationList = _internalEvent.GetInvocationList();
            TResult[] results         = new TResult[invocationList.Length];

            for (int i = 0; i < invocationList.Length; i++)
            {
                var func = (Func<T1, T2, T3, TResult>)invocationList[i];
                results.SetValue(func.Invoke(arg1, arg2, arg3), i);
            }

            return results;
        }

        public static CollectorEvent<T1, T2, T3, TResult> operator +(CollectorEvent<T1, T2, T3, TResult> collectorEvent, Func<T1, T2, T3, TResult> func)
        {
            collectorEvent._internalEvent += func;
            return collectorEvent;
        }
        public static CollectorEvent<T1, T2, T3, TResult> operator -(CollectorEvent<T1, T2, T3, TResult> collectorEvent, Func<T1, T2, T3, TResult> func)
        {
            collectorEvent._internalEvent -= func;
            return collectorEvent;
        }
    }

    public class CollectorEvent<T1, T2, T3, T4, TResult>
    {
        private event Func<T1, T2, T3, T4, TResult> _internalEvent;

        public void Subscribe(Func<T1, T2, T3, T4, TResult> func)
        {
            _internalEvent += func;
        }
        public void Unsubscribe(Func<T1, T2, T3, T4, TResult> func)
        {
            _internalEvent -= func;
        }
        public void Clear()
        {
            _internalEvent = null;
        }

        public TResult[] Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (_internalEvent is null)
                return new TResult[0];

            Delegate[] invocationList = _internalEvent.GetInvocationList();
            TResult[] results         = new TResult[invocationList.Length];

            for (int i = 0; i < invocationList.Length; i++)
            {
                var func = (Func<T1, T2, T3, T4, TResult>)invocationList[i];
                results.SetValue(func.Invoke(arg1, arg2, arg3, arg4), i);
            }

            return results;
        }

        public static CollectorEvent<T1, T2, T3, T4, TResult> operator +(CollectorEvent<T1, T2, T3, T4, TResult> collectorEvent, Func<T1, T2, T3, T4, TResult> func)
        {
            collectorEvent._internalEvent += func;
            return collectorEvent;
        }
        public static CollectorEvent<T1, T2, T3, T4, TResult> operator -(CollectorEvent<T1, T2, T3, T4, TResult> collectorEvent, Func<T1, T2, T3, T4, TResult> func)
        {
            collectorEvent._internalEvent -= func;
            return collectorEvent;
        }
    }
}
