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
                    while (_queue.Count <= 0)
                    {
                        Monitor.Pulse(_lock);
                        Monitor.Pulse(direction._lock);
                        Monitor.Wait(_lock);
                    }
                    Beverage beverage = _queue.Dequeue();
                    direction._queue.Enqueue(beverage);
                }
            }
        }
        public Beverage Next()
        {
            lock (_lock)
            {
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
                    Monitor.Wait(_lock);
                }
                Beverage beverage;
                beverage = _queue.Dequeue();
                return beverage;
            }
        }
    }
}
