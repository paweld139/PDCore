using System;

namespace PDCore.Extensions
{
    public static class DelegateExtension
    {
        public static Func<TResult> Partial<TParam1, TResult>(this Func<TParam1, TResult> func, TParam1 parameter)
        {
            return () => func(parameter);
        }

        public static Func<TResult> Partial<TParam1, TParam2, TResult>(this Func<TParam1, TParam2, TResult> func, TParam1 param1, TParam2 param2)
        {
            return () => func(param1, param2);
        }

        public static Action Partial<TResult>(this Func<TResult> func)
        {
            return () => func();
        }

        public static Action Partial<TParam1>(this Action<TParam1> action, TParam1 parameter)
        {
            return () => action(parameter);
        }

        public static Action Partial<TParam1, TParam2>(this Action<TParam1, TParam2> action, TParam1 param1, TParam2 param2)
        {
            return () => action(param1, param2);
        }

        public static Func<TParam1, Func<TResult>> Curry<TParam1, TResult>(this Func<TParam1, TResult> func)
        {
            return parameter => () => func(parameter);
        }

        public static Func<TParam1, TParam2, Func<TResult>> Curry<TParam1, TParam2, TResult>(this Func<TParam1, TParam2, TResult> func)
        {
            return (param1, param2) => () => func(param1, param2);
        }
    }
}
