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

        public int Count
        {
            get { return _queue.Count; }
        }

        public int _limit { get; }
        private readonly object _lock = new object();

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
                    Monitor.Wait(_lock);
                }
                _queue.Enqueue(beverage);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Produced {beverage.Name} In main buffer {_queue.Count}");

                Monitor.Pulse(_lock);
            }
            Thread.Sleep(200);
        }
        public void Split(Buffer direction, Beverage beverage)
        {
            lock (_lock)
            {
                lock (direction._lock)
                {
                    if (_queue.Count <= 0)
                    {
                        Monitor.Wait(_lock);
                    }
                    else if (direction.Count >= direction._limit)
                    {
                         Monitor.Wait(direction._lock);
                    }
                    else
                    {
                        direction._queue.Enqueue(beverage);
                    }
                }
            }
            Thread.Sleep(300);
        }
        public Beverage Pull()
        {
            Beverage beverage;
            lock (_lock)
            {
                while (_queue.Count <= 0)
                {
                    Monitor.Pulse(_lock);
                    Monitor.Wait(_lock);
                }
                beverage = _queue.Dequeue();
            }
            Thread.Sleep(100);
            return beverage;
        }
        //public Queue<Beverage> Move()
        //{
        //    lock (_lock)
        //    {
        //        while (_queue.Count < _limit)
        //        {
        //            Monitor.Wait(_lock);
        //        }
        //        Queue<Beverage> queue = new Queue<Beverage>();

        //        return _queue;
        //        Beverage[] beverages = new Beverage[_queue.Count];
        //        int amount = _queue.Count;
        //        for (int i = 0; i < amount; i++)
        //        {
        //            beverages[i] = _queue.Dequeue();
        //        }
        //        Thread.Sleep(100);
        //        return beverages;
        //    }
        //}
    }
}
