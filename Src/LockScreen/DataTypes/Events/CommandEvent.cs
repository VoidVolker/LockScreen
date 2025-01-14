using System;
using System.Windows.Input;

using Prism.Commands;

namespace LockScreen.DataTypes.Events
{
    /// <summary>
    /// Automatic connecting command and event
    /// </summary>
    public class CommandEvent<TCommandArg, TEventArgs>
        : CommandEvent<TEventArgs>
        where TEventArgs : EventArgs
    {
        public Func<TCommandArg, TEventArgs> Converter;

        /// <summary>
        /// Create new command and event connector
        /// </summary>
        /// <param name="argumentConverter">Converts command argument to event argument type</param>
        public CommandEvent(Func<TCommandArg, TEventArgs> argumentConverter)
        {
            Converter = argumentConverter;
            Command = new DelegateCommand<TCommandArg>(Trigger);
        }

        /// <summary>
        /// Create new command and event connector
        /// </summary>
        /// <param name="argumentConverter">Converts command argument to event argument type</param>
        public CommandEvent(Func<object> getSender, Func<TCommandArg, TEventArgs> argumentConverter)
        {
            Converter = argumentConverter;
            Command = new DelegateCommand<TCommandArg>(Trigger);
            GetSender = getSender;
        }

        /// <summary>
        /// Trigger event with GetSender delegate.
        /// Default handler for Command in constructor.
        /// </summary>
        /// <param name="arg"></param>
        public void Trigger(TCommandArg arg)
        {
            Trigger(Converter(arg));
        }

        /// <summary>
        /// Trigger event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        public void Trigger(object sender, TCommandArg arg)
        {
            Trigger(sender, Converter(arg));
        }
    }


    /// <summary>
    /// Automatic connecting command and event
    /// </summary>
    public class CommandEvent : CommandEvent<EventArgs>
    {
        /// <summary>
        /// Create new command and event connector
        /// </summary>
        public CommandEvent() : base() { }
        /// <summary>
        /// Create new command and event connector
        /// </summary>
        /// <param name="getArgs">Get event arguments</param>
        public CommandEvent(Func<EventArgs> getArgs) : base(getArgs) { }
        /// <summary>
        /// Create new command and event connector
        /// </summary>
        /// <param name="getSender">Get sender object</param>
        public CommandEvent(Func<object> getSender) : base(getSender) { }
        /// <summary>
        /// Create new command and event connector
        /// </summary>
        /// <param name="getSender">Get sender object</param>
        /// <param name="getArgs">Get event arguments</param>
        public CommandEvent(Func<object> getSender, Func<EventArgs> getArgs) : base(getSender, getArgs) { }
    }


    /// <summary>
    /// Automatic connecting command and event
    /// </summary>
    public class CommandEvent<TEventArgs> where TEventArgs : EventArgs
    {
        internal EventHandler<TEventArgs> EventHandlers;
        /// <summary>
        /// Event attached to comand
        /// </summary>
        public event EventHandler<TEventArgs> Event
        {
            add => EventHandlers += value;
            remove => EventHandlers -= value;
        }

        public Func<TEventArgs> GetArgs;
        public Func<object> GetSender;

        /// <summary>
        /// Command attached to event
        /// </summary>
        public ICommand Command { get; protected set; }

        /// <summary>
        /// Create new command and event connector
        /// </summary>
        public CommandEvent() => Command = new DelegateCommand<object>(Trigger);
        /// <summary>
        /// Create new command and event connector
        /// </summary>
        /// <param name="getSender">Get sender object</param>
        public CommandEvent(Func<object> getSender) : this() => GetSender = getSender;
        /// <summary>
        /// Create new command and event connector
        /// </summary>
        /// <param name="getArgs">Get event arguments</param>
        public CommandEvent(Func<TEventArgs> getArgs) : this() => GetArgs = getArgs;
        /// <summary>
        /// Create new command and event connector
        /// </summary>
        /// <param name="getArgs">Get event arguments</param>
        /// <param name="getSender">Get sender object</param>
        public CommandEvent(Func<object> getSender, Func<TEventArgs> getArgs) : this()
        {
            GetSender = getSender;
            GetArgs = getArgs;
        }

        /// <summary>
        /// Trigger event with GetArgs and GetSender delegates.
        /// Default handler for Command in constructor.
        /// </summary>
        public void Trigger()
        {
            object sender = this;
            TEventArgs args = default;
            sender = GetSender?.Invoke();
            args = GetArgs?.Invoke();
            EventHandlers?.Invoke(sender, args);
        }

        /// <summary>
        /// Trigger event with GetArgs delegate
        /// </summary>
        /// <param name="sender">sender object</param>
        public void Trigger(object sender)
        {
            TEventArgs args = default;
            args = GetArgs?.Invoke();
            EventHandlers?.Invoke(sender, args);
        }

        /// <summary>
        /// Trigger event with GetSender delegate
        /// </summary>
        /// <param name="args">Event arguments</param>
        public void Trigger(TEventArgs args)
        {
            object sender = this;
            sender = GetSender?.Invoke();
            EventHandlers?.Invoke(sender, args);
        }

        /// <summary>
        /// Trigger event
        /// </summary>
        public void Trigger(object sender, TEventArgs args)
        {
            EventHandlers?.Invoke(sender, args);
        }
    }
}
