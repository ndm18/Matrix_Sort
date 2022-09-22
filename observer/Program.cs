using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace observer
{
    internal static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Startup time: " + DateTime.Now);
            IObservable countdown = new Countdown("Some information");

            countdown.Subscribe(new Subscriber("1", 10));
            countdown.Subscribe(new Subscriber("2", 2));
            countdown.Subscribe(new Subscriber("3", 5));

            while (true)
            {
                Thread.Sleep(100);
                countdown.NotifyAll();
            }
        }
    }

    public interface IObservable
    {
        void NotifyAll();
        void Subscribe(ISubscriber subscriber);
        bool Unsubscribe(ISubscriber subscriber);
    }

    public interface ISubscriber
    {
        void Run(IObservable observable);
    }

    public class Countdown : IObservable
    {
        private readonly IDictionary<ISubscriber, DateTime> _subscribers;
        private readonly string _message;

        public Countdown(string message)
        {
            _message = message;
            _subscribers = new Dictionary<ISubscriber, DateTime>();
        }

        public string GetMessage() => _message;

        public void NotifyAll()
        {
            var subscribers = _subscribers.Keys.ToArray();
            for (var i = 0; i < subscribers.Length; i++)
            {
                subscribers[i].Run(this);
            }
        }

        public void Subscribe(ISubscriber subscriber) => _subscribers.Add(subscriber, DateTime.Now);

        public bool Unsubscribe(ISubscriber subscriber) => _subscribers.Remove(subscriber);


        public bool GetDateTimeBySubscriber(ISubscriber subscriber, out DateTime dateTime) =>
            _subscribers.TryGetValue(subscriber, out dateTime);
    }

    public class Subscriber : ISubscriber
    {
        private readonly string _id;

        /*
         * offset is seconds
         */
        private readonly int _offset;

        public Subscriber(string id, int offset)
        {
            _offset = offset;
            this._id = id;
        }

        public void Run(IObservable observable)
        {
            var now = DateTime.Now;
            if (
                !(observable is Countdown countdown) ||
                !countdown.GetDateTimeBySubscriber(this, out var dateTime) ||
                dateTime.AddSeconds(_offset).CompareTo(now) == 1
            ) return;

            Console.WriteLine("id: {0}, offset: {1}, message: {2}, current time: {3}", _id, _offset, countdown.GetMessage(), now);
            countdown.Unsubscribe(this);
        }
    }
}