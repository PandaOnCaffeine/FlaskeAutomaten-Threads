using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlaskeAutomaten_Threads
{
    internal class Buffer
    {
        private Queue<Beverage> _queue = new Queue<Beverage>();
        private readonly object _lock = new object();
        public bool Waiting { get; private set; } = false;
        public int Count { get { return _queue.Count; } }
        public int _limit { get; }
        public string Name { get; set; }
        public Buffer(string bufferName, int limit)
        {
            Name = bufferName;
            _limit = limit;
        }
        public void Produce(Beverage beverage)
        {
            lock (_lock)
            {
                while (_queue.Count >= _limit)
                {
                    Monitor.Pulse(_lock);
                    Monitor.Wait(_lock);
                }
                _queue.Enqueue(beverage);
            }
        }
        public void Split(Buffer direction)
        {
            lock (_lock)
            {
                lock (direction._lock)
                {
                    while (direction.Count >= direction._limit)
                    {
                        Monitor.Pulse(direction._lock);
                        Monitor.Wait(direction._lock);
                    }
                    Beverage beverage = _queue.Dequeue();
                    direction._queue.Enqueue(beverage);
                }
            }
        }
        public Beverage Next(TextBox box)
        {
            lock (_lock)
            {
                while (_queue.Count <= 0)
                {
                    box.WriteAt("Split Waiting", ConsoleColor.DarkRed);
                    Monitor.Pulse(_lock);
                    Monitor.Wait(_lock);
                }
                Beverage beverage;
                beverage = _queue.Peek();
                return beverage;
            }
        }
        public Beverage Consume()
        {
            lock (_lock)
            {
                while (_queue.Count <= 0)
                {
                    Monitor.Pulse(_lock);
                    Monitor.Wait(_lock);
                }
                Beverage beverage;
                beverage = _queue.Dequeue();
                return beverage;
            }
        }
    }
}
