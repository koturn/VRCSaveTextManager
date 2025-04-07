// #define INCLUDE_OBSOLETE

using System;
#if INCLUDE_OBSOLETE
using System.Runtime.CompilerServices;
#endif  // INCLUDE_OBSOLETE
using System.Threading;
using System.Windows.Threading;


namespace VRCSaveTextManager.Internals.Extensions
{
    /// <summary>
    /// Provide extension methods for <see cref="Dispatcher"/>.
    /// </summary>
    internal static class DispatcherExtension
    {
        /// <summary>
        /// Execute <see cref="Action"/>, a delegate without argument or return value on UI thread.
        /// </summary>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="action">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <remarks><seealso cref="Dispatcher.Invoke(Action)"/></remarks>
        public static void InvokeIfNeeded(this Dispatcher dispatcher, Action action)
        {
            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.Invoke(action);
            }
        }

        /// <summary>
        /// Execute <see cref="Action"/>, a delegate without argument or return value on UI thread.
        /// </summary>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="action">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="priority">The priority with which the specified method is invoked, relative to the other pending operations in the <see cref="Dispatcher"/> event queue.</param>
        /// <remarks><seealso cref="Dispatcher.Invoke(Action, DispatcherPriority)"/></remarks>
        public static void InvokeIfNeeded(this Dispatcher dispatcher, Action action, DispatcherPriority priority)
        {
            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.Invoke(action, priority);
            }
        }

        /// <summary>
        /// Execute <see cref="Action"/>, a delegate without argument or return value on UI thread.
        /// </summary>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="action">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="priority">The priority with which the specified method is invoked, relative to the other pending operations in the <see cref="Dispatcher"/> event queue.</param>
        /// <param name="cancellationToken">An object that indicates whether to cancel the action</param>
        /// <remarks><seealso cref="Dispatcher.Invoke(Action, DispatcherPriority, CancellationToken)"/></remarks>
        public static void InvokeIfNeeded(this Dispatcher dispatcher, Action action, DispatcherPriority priority, CancellationToken cancellationToken)
        {
            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.Invoke(action, priority, cancellationToken);
            }
        }

        /// <summary>
        /// Execute <see cref="Action"/>, a delegate without argument or return value on UI thread.
        /// </summary>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="action">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="priority">The priority with which the specified method is invoked, relative to the other pending operations in the <see cref="Dispatcher"/> event queue.</param>
        /// <param name="cancellationToken">An object that indicates whether to cancel the action</param>
        /// <param name="timeout">The maximum amount of time to wait for the operation to start.</param>
        /// <remarks><seealso cref="Dispatcher.Invoke(Action, DispatcherPriority, CancellationToken, TimeSpan)"/></remarks>
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1068 // CancellationToken parameters must come last
        public static void InvokeIfNeeded(this Dispatcher dispatcher, Action action, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout)
#pragma warning restore CA1068 // CancellationToken parameters must come last
#pragma warning restore IDE0079 // Remove unnecessary suppression
        {
            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.Invoke(action, priority, cancellationToken, timeout);
            }
        }

#if INCLUDE_OBSOLETE
        /// <summary>
        /// Execute <see cref="Action{T}"/>, a delegate with one argument and no return value on UI thread.
        /// </summary>
        /// <typeparam name="T">Type of the <paramref name="arg"/>, which can be inferred from the first type parameter (type of the first argument) of <paramref name="action"/>.</typeparam>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="action">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="arg">An angument for <paramref name="action"/>.</param>
        /// <remarks><seealso cref="Dispatcher.Invoke(DispatcherPriority, Delegate, object)"/></remarks>
        [Obsolete("This method uses legacy overload of Dispatcher.Invoke(). Consider using another overload of InvokeIfNeeded().")]
        public static void InvokeIfNeeded<T>(this Dispatcher dispatcher, Action<T> action, T arg)
        {
            if (dispatcher.CheckAccess())
            {
                action(arg);
            }
            else
            {
                dispatcher.Invoke(DispatcherPriority.Send, action, arg);
            }
        }

        /// <summary>
        /// Execute <see cref="Action{T}"/>, a delegate with one argument and no return value on UI thread.
        /// </summary>
        /// <typeparam name="T">Type of the <paramref name="arg"/>, which can be inferred from the first type parameter (type of the first argument) of <paramref name="action"/>.</typeparam>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="action">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="arg">An angument for <paramref name="action"/>.</param>
        /// <param name="priority">The priority with which the specified method is invoked, relative to the other pending operations in the <see cref="Dispatcher"/> event queue.</param>
        /// <remarks><seealso cref="Dispatcher.Invoke(DispatcherPriority, Delegate, object)"/></remarks>
        [Obsolete("This method uses legacy overload of Dispatcher.Invoke(). Consider using another overload of InvokeIfNeeded().")]
        public static void InvokeIfNeeded<T>(this Dispatcher dispatcher, Action<T> action, T arg, DispatcherPriority priority)
        {
            if (dispatcher.CheckAccess())
            {
                action(arg);
            }
            else
            {
                dispatcher.Invoke(priority, action, arg);
            }
        }

        /// <summary>
        /// Execute <see cref="Action{T}"/>, a delegate with one argument and no return value on UI thread.
        /// </summary>
        /// <typeparam name="T">Type of the <paramref name="arg"/>, which can be inferred from the first type parameter (type of the first argument) of <paramref name="action"/>.</typeparam>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="action">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="arg">An angument for <paramref name="action"/>.</param>
        /// <param name="priority">The priority with which the specified method is invoked, relative to the other pending operations in the <see cref="Dispatcher"/> event queue.</param>
        /// <param name="timeout">The maximum amount of time to wait for the operation to start.</param>
        /// <remarks><seealso cref="Dispatcher.Invoke(DispatcherPriority, TimeSpan, Delegate, object)"/></remarks>
        [Obsolete("This method uses legacy overload of Dispatcher.Invoke(). Consider using another overload of InvokeIfNeeded().")]
        public static void InvokeIfNeeded<T>(this Dispatcher dispatcher, Action<T> action, T arg, DispatcherPriority priority, TimeSpan timeout)
        {
            if (dispatcher.CheckAccess())
            {
                action(arg);
            }
            else
            {
                dispatcher.Invoke(priority, timeout, action, arg);
            }
        }

        /// <summary>
        /// Execute <see cref="Delegate"/>, a delegate with N arguments on UI thread.
        /// </summary>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="callback">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="args">An arguments for <paramref name="callback"/>.</param>
        /// <remarks><seealso cref="Dispatcher.Invoke(Delegate, object[])"/></remarks>
        [Obsolete("This method uses legacy overload of Dispatcher.Invoke(). Consider using another overload of InvokeIfNeeded().")]
        public static void InvokeIfNeeded(this Dispatcher dispatcher, Delegate callback, params object[] args)
        {
            if (dispatcher.CheckAccess())
            {
                callback.DynamicInvoke(args);
            }
            else
            {
                dispatcher.Invoke(callback, args);
            }
        }

        /// <summary>
        /// Execute <see cref="Delegate"/>, a delegate with N arguments on UI thread.
        /// </summary>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="callback">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="priority">The priority with which the specified method is invoked, relative to the other pending operations in the <see cref="Dispatcher"/> event queue.</param>
        /// <param name="args">An arguments for <paramref name="callback"/>.</param>
        /// <remarks><seealso cref="Dispatcher.Invoke(Delegate, DispatcherPriority, object[])"/></remarks>
        [Obsolete("This method uses legacy overload of Dispatcher.Invoke(). Consider using another overload of InvokeIfNeeded().")]
        public static void InvokeIfNeeded(this Dispatcher dispatcher, Delegate callback, DispatcherPriority priority, params object[] args)
        {
            if (dispatcher.CheckAccess())
            {
                callback.DynamicInvoke(args);
            }
            else
            {
                dispatcher.Invoke(callback, priority, args);
            }
        }

        /// <summary>
        /// Execute <see cref="Delegate"/>, a delegate with N arguments on UI thread.
        /// </summary>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="callback">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="timeout">The maximum amount of time to wait for the operation to start.</param>
        /// <param name="args">An arguments for <paramref name="callback"/>.</param>
        /// <remarks><seealso cref="Dispatcher.Invoke(Delegate, TimeSpan, object[])"/></remarks>
        [Obsolete("This method uses legacy overload of Dispatcher.Invoke(). Consider using another overload of InvokeIfNeeded().")]
        public static void InvokeIfNeeded(this Dispatcher dispatcher, Delegate callback, TimeSpan timeout, params object[] args)
        {
            if (dispatcher.CheckAccess())
            {
                callback.DynamicInvoke(args);
            }
            else
            {
                dispatcher.Invoke(callback, timeout, args);
            }
        }

        /// <summary>
        /// Execute <see cref="Delegate"/>, a delegate with N arguments on UI thread.
        /// </summary>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="callback">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="priority">The priority with which the specified method is invoked, relative to the other pending operations in the <see cref="Dispatcher"/> event queue.</param>
        /// <param name="timeout">The maximum amount of time to wait for the operation to start.</param>
        /// <param name="args">An arguments for <paramref name="callback"/>.</param>
        /// <remarks><seealso cref="Dispatcher.Invoke(Delegate, TimeSpan, DispatcherPriority, object[])"/></remarks>
        [Obsolete("This method uses legacy overload of Dispatcher.Invoke(). Consider using another overload of InvokeIfNeeded().")]
        public static void InvokeIfNeeded(this Dispatcher dispatcher, Delegate callback, DispatcherPriority priority, TimeSpan timeout, params object[] args)
        {
            if (dispatcher.CheckAccess())
            {
                callback.DynamicInvoke(args);
            }
            else
            {
                dispatcher.Invoke(callback, timeout, priority, args);
            }
        }
#endif  // INCLUDE_OBSOLETE

        /// <summary>
        /// Execute <see cref="Func{R}"/>, a delegate with no argument and return value on UI thread.
        /// </summary>
        /// <typeparam name="R">Type of return value, which can be inferred from the first type parameter (type of return value) of <paramref name="func"/>.</typeparam>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="func">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <returns>Return value of <paramref name="func"/>.</returns>
        /// <remarks><seealso cref="Dispatcher.Invoke{R}(Func{R})"/></remarks>
        public static R InvokeFuncIfNeeded<R>(this Dispatcher dispatcher, Func<R> func)
        {
            return dispatcher.CheckAccess() ? func() : dispatcher.Invoke(func);
        }

        /// <summary>
        /// Execute <see cref="Func{R}"/>, a delegate with no argument and return value on UI thread.
        /// </summary>
        /// <typeparam name="R">Type of return value, which can be inferred from the first type parameter (type of the return value) of <paramref name="func"/>.</typeparam>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="func">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="priority">The priority with which the specified method is invoked, relative to the other pending operations in the <see cref="Dispatcher"/> event queue.</param>
        /// <returns>Return value of <paramref name="func"/>.</returns>
        /// <remarks><seealso cref="Dispatcher.Invoke{R}(Func{R}, DispatcherPriority)"/></remarks>
        public static R InvokeFuncIfNeeded<R>(this Dispatcher dispatcher, Func<R> func, DispatcherPriority priority)
        {
            return dispatcher.CheckAccess() ? func() : dispatcher.Invoke(func, priority);
        }

        /// <summary>
        /// Execute <see cref="Func{R}"/>, a delegate with no argument and return value on UI thread.
        /// </summary>
        /// <typeparam name="R">Type of return value, which can be inferred from the first type parameter (type of return value) of <paramref name="func"/>.</typeparam>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="func">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="priority">The priority with which the specified method is invoked, relative to the other pending operations in the <see cref="Dispatcher"/> event queue.</param>
        /// <param name="cancellationToken">An object that indicates whether to cancel the action</param>
        /// <returns>Return value of <paramref name="func"/>.</returns>
        /// <remarks><seealso cref="Dispatcher.Invoke{R}(Func{R}, DispatcherPriority, CancellationToken)"/></remarks>
        public static R InvokeFuncIfNeeded<R>(this Dispatcher dispatcher, Func<R> func, DispatcherPriority priority, CancellationToken cancellationToken)
        {
            return dispatcher.CheckAccess() ? func() : dispatcher.Invoke(func, priority, cancellationToken);
        }

        /// <summary>
        /// Execute <see cref="Func{R}"/>, a delegate with no argument and return value on UI thread.
        /// </summary>
        /// <typeparam name="R">Type of return value, which can be inferred from the first type parameter (type of return value) of <paramref name="func"/>.</typeparam>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="func">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="priority">The priority with which the specified method is invoked, relative to the other pending operations in the <see cref="Dispatcher"/> event queue.</param>
        /// <param name="cancellationToken">An object that indicates whether to cancel the action</param>
        /// <param name="timeout">The maximum amount of time to wait for the operation to start.</param>
        /// <returns>Return value of <paramref name="func"/>.</returns>
        /// <remarks><seealso cref="Dispatcher.Invoke{R}(Func{R}, DispatcherPriority, CancellationToken, TimeSpan)"/></remarks>
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1068 // CancellationToken parameters must come last
        public static R InvokeFuncIfNeeded<R>(this Dispatcher dispatcher, Func<R> func, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout)
#pragma warning restore CA1068 // CancellationToken parameters must come last
#pragma warning restore IDE0079 // Remove unnecessary suppression
        {
            return dispatcher.CheckAccess() ? func() : dispatcher.Invoke(func, priority, cancellationToken, timeout);
        }

#if INCLUDE_OBSOLETE
        /// <summary>
        /// Execute <see cref="Func{T, R}"/>, a delegate with one argument and return value on UI thread.
        /// </summary>
        /// <typeparam name="T">Type of the <paramref name="arg"/>, which can be inferred from the second type parameter (type of the first argument of <paramref name="func"/>) of <paramref name="func"/>.</typeparam>
        /// <typeparam name="R">Type of return value, which can be inferred from the first type parameter (type of return value) of <paramref name="func"/>.</typeparam>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="func">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="arg">An angument for <paramref name="func"/>.</param>
        /// <returns>Return value of <paramref name="func"/>.</returns>
        /// <remarks><seealso cref="Dispatcher.Invoke(DispatcherPriority, Delegate, object)"/></remarks>
        [Obsolete("This method uses legacy overload of Dispatcher.Invoke(). Consider using another overload of InvokeFuncIfNeeded().")]
        public static R InvokeFuncIfNeeded<T, R>(this Dispatcher dispatcher, Func<T, R> func, T arg)
        {
            return dispatcher.CheckAccess() ? func(arg) : (R)dispatcher.Invoke(DispatcherPriority.Send, func, arg);
        }

        /// <summary>
        /// Execute <see cref="Func{T, R}"/>, a delegate with one argument and return value on UI thread.
        /// </summary>
        /// <typeparam name="T">Type of the <paramref name="arg"/>, which can be inferred from the second type parameter (type of the first argument of <paramref name="func"/>) of <paramref name="func"/>.</typeparam>
        /// <typeparam name="R">Type of return value, which can be inferred from the first type parameter (type of return value) of <paramref name="func"/>.</typeparam>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="func">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="arg">An angument for <paramref name="func"/>.</param>
        /// <param name="priority">The priority with which the specified method is invoked, relative to the other pending operations in the <see cref="Dispatcher"/> event queue.</param>
        /// <returns>Return value of <paramref name="func"/>.</returns>
        /// <remarks><seealso cref="Dispatcher.Invoke(DispatcherPriority, Delegate, object)"/></remarks>
        [Obsolete("This method uses legacy overload of Dispatcher.Invoke(). Consider using another overload of InvokeFuncIfNeeded().")]
        public static R InvokeFuncIfNeeded<T, R>(this Dispatcher dispatcher, Func<T, R> func, T arg, DispatcherPriority priority)
        {
            return dispatcher.CheckAccess() ? func(arg) : (R)dispatcher.Invoke(priority, func, arg);
        }

        /// <summary>
        /// Execute <see cref="Func{T, R}"/>, a delegate with one argument and return value on UI thread.
        /// </summary>
        /// <typeparam name="T">Type of the <paramref name="arg"/>, which can be inferred from the second type parameter (type of the first argument of <paramref name="func"/>) of <paramref name="func"/>.</typeparam>
        /// <typeparam name="R">Type of return value, which can be inferred from the first type parameter (type of return value) of <paramref name="func"/>.</typeparam>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="func">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="arg">An angument for <paramref name="func"/>.</param>
        /// <param name="priority">The priority with which the specified method is invoked, relative to the other pending operations in the <see cref="Dispatcher"/> event queue.</param>
        /// <param name="timeout">The maximum amount of time to wait for the operation to start.</param>
        /// <returns>Return value of <paramref name="func"/>.</returns>
        /// <remarks><seealso cref="Dispatcher.Invoke(DispatcherPriority, TimeSpan, Delegate, object)"/></remarks>
        [Obsolete("This method uses legacy overload of Dispatcher.Invoke(). Consider using another overload of InvokeFuncIfNeeded().")]
        public static R InvokeFuncIfNeeded<T, R>(this Dispatcher dispatcher, Func<T, R> func, T arg, DispatcherPriority priority, TimeSpan timeout)
        {
            return dispatcher.CheckAccess() ? func(arg) : (R)dispatcher.Invoke(priority, timeout, func, arg);
        }

        /// <summary>
        /// Execute <see cref="Delegate"/>, a delegate with N arguments and return value on UI thread.
        /// </summary>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="callback">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="args">An arguments for <paramref name="callback"/>.</param>
        /// <remarks><seealso cref="Dispatcher.Invoke(Delegate, object[])"/></remarks>
        [Obsolete("This method uses legacy overload of Dispatcher.Invoke(). Consider using another overload of InvokeFuncIfNeeded().")]
        public static object? InvokeFuncIfNeeded(this Dispatcher dispatcher, Delegate callback, params object[] args)
        {
            return dispatcher.CheckAccess() ? callback.DynamicInvoke(args) : dispatcher.Invoke(callback, args);
        }

        /// <summary>
        /// Execute <see cref="Delegate"/>, a delegate with N arguments and return value on UI thread.
        /// </summary>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="callback">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="priority">The priority with which the specified method is invoked, relative to the other pending operations in the <see cref="Dispatcher"/> event queue.</param>
        /// <param name="args">An arguments for <paramref name="callback"/>.</param>
        /// <remarks><seealso cref="Dispatcher.Invoke(Delegate, DispatcherPriority, object[])"/></remarks>
        [Obsolete("This method uses legacy overload of Dispatcher.Invoke(). Consider using another overload of InvokeFuncIfNeeded().")]
        public static object? InvokeFuncIfNeeded(this Dispatcher dispatcher, Delegate callback, DispatcherPriority priority, params object[] args)
        {
            return dispatcher.CheckAccess() ? callback.DynamicInvoke(args) : dispatcher.Invoke(callback, priority, args);
        }

        /// <summary>
        /// Execute <see cref="Delegate"/>, a delegate with N arguments and return value on UI thread.
        /// </summary>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="callback">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="timeout">The maximum amount of time to wait for the operation to start.</param>
        /// <param name="args">An arguments for <paramref name="callback"/>.</param>
        /// <remarks><seealso cref="Dispatcher.Invoke(Delegate, TimeSpan, object[])"/></remarks>
        [Obsolete("This method uses legacy overload of Dispatcher.Invoke(). Consider using another overload of InvokeFuncIfNeeded().")]
        public static object? InvokeFuncIfNeeded(this Dispatcher dispatcher, Delegate callback, TimeSpan timeout, params object[] args)
        {
            return dispatcher.CheckAccess() ? callback.DynamicInvoke(args) : dispatcher.Invoke(callback, timeout, args);
        }

        /// <summary>
        /// Execute <see cref="Delegate"/>, a delegate with N arguments and return value on UI thread.
        /// </summary>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="callback">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="priority">The priority with which the specified method is invoked, relative to the other pending operations in the <see cref="Dispatcher"/> event queue.</param>
        /// <param name="timeout">The maximum amount of time to wait for the operation to start.</param>
        /// <param name="args">An arguments for <paramref name="callback"/>.</param>
        /// <remarks><seealso cref="Dispatcher.Invoke(Delegate, TimeSpan, DispatcherPriority, object[])"/></remarks>
        [Obsolete("This method uses legacy overload of Dispatcher.Invoke(). Consider using another overload of InvokeFuncIfNeeded().")]
        public static object? InvokeFuncIfNeeded(this Dispatcher dispatcher, Delegate callback, DispatcherPriority priority, TimeSpan timeout, params object[] args)
        {
            return dispatcher.CheckAccess() ? callback.DynamicInvoke(args) : dispatcher.Invoke(callback, timeout, priority, args);
        }

        /// <summary>
        /// Execute <see cref="Delegate"/>, a delegate with N arguments and return value on UI thread.
        /// </summary>
        /// <typeparam name="R">Type of return value.</typeparam>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="callback">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="args">An arguments for <paramref name="callback"/>.</param>
        /// <remarks><seealso cref="InvokeFuncIfNeeded(Dispatcher, Delegate, object[])"/></remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Obsolete("This method uses legacy overload of Dispatcher.Invoke(). Consider using another overload of InvokeFuncIfNeeded().")]
        public static R? InvokeFuncIfNeeded<R>(this Dispatcher dispatcher, Delegate callback, params object[] args)
        {
            return (R?)InvokeFuncIfNeeded(dispatcher, callback, args);
        }

        /// <summary>
        /// Execute <see cref="Delegate"/>, a delegate with N arguments and return value on UI thread.
        /// </summary>
        /// <typeparam name="R">Type of return value.</typeparam>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="callback">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="priority">The priority with which the specified method is invoked, relative to the other pending operations in the <see cref="Dispatcher"/> event queue.</param>
        /// <param name="args">An arguments for <paramref name="callback"/>.</param>
        /// <remarks><seealso cref="InvokeFuncIfNeeded(Dispatcher, Delegate, DispatcherPriority, object[])"/></remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Obsolete("This method uses legacy overload of Dispatcher.Invoke(). Consider using another overload of InvokeFuncIfNeeded().")]
        public static R? InvokeFuncIfNeeded<R>(this Dispatcher dispatcher, Delegate callback, DispatcherPriority priority, params object[] args)
        {
            return (R?)InvokeFuncIfNeeded(dispatcher, callback, priority, args);
        }

        /// <summary>
        /// Execute <see cref="Delegate"/>, a delegate with N arguments and return value on UI thread.
        /// </summary>
        /// <typeparam name="R">Type of return value.</typeparam>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="callback">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="timeout">The maximum amount of time to wait for the operation to start.</param>
        /// <param name="args">An arguments for <paramref name="callback"/>.</param>
        /// <remarks><seealso cref="InvokeFuncIfNeeded(Dispatcher, Delegate, TimeSpan, object[])"/></remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Obsolete("This method uses legacy overload of Dispatcher.Invoke(). Consider using another overload of InvokeFuncIfNeeded().")]
        public static R? InvokeFuncIfNeeded<R>(this Dispatcher dispatcher, Delegate callback, TimeSpan timeout, params object[] args)
        {
            return (R?)InvokeFuncIfNeeded(dispatcher, callback, timeout, args);
        }

        /// <summary>
        /// Execute <see cref="Delegate"/>, a delegate with N arguments and return value on UI thread.
        /// </summary>
        /// <typeparam name="R">Type of return value.</typeparam>
        /// <param name="dispatcher">A <see cref="Dispatcher"/>.</param>
        /// <param name="callback">A process you want to execute on <see cref="Dispatcher.Thread"/>.</param>
        /// <param name="priority">The priority with which the specified method is invoked, relative to the other pending operations in the <see cref="Dispatcher"/> event queue.</param>
        /// <param name="timeout">The maximum amount of time to wait for the operation to start.</param>
        /// <param name="args">An arguments for <paramref name="callback"/>.</param>
        /// <remarks><seealso cref="InvokeFuncIfNeeded(Dispatcher, Delegate, DispatcherPriority, TimeSpan, object[])"/></remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Obsolete("This method uses legacy overload of Dispatcher.Invoke(). Consider using another overload of InvokeFuncIfNeeded().")]
        public static R? InvokeFuncIfNeeded<R>(this Dispatcher dispatcher, Delegate callback, DispatcherPriority priority, TimeSpan timeout, params object[] args)
        {
            return (R?)InvokeFuncIfNeeded(dispatcher, callback, priority, timeout, args);
        }
#endif  // INCLUDE_OBSOLETE
    }
}
