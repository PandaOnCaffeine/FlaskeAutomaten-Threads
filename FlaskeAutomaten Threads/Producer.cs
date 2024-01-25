using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlaskeAutomaten_Threads
{
    internal class Producer
    {
        private Buffer _queue;
        private TextBox _box;
        private bool _running = true;
        private Random _random = new Random();
        public Producer(Buffer queue, TextBox box)
        {
            _queue = queue;
            _box = box;
        }
        public void Run()
        {
            int key = 1;
            while (_running)
            {
                Thread.Sleep(100);
                Beverage beverage;
                int randomNr = _random.Next(0, 2);
                switch (randomNr)
                {
                    case 0:
                        beverage = new Beverage("Beer", randomNr, key);
                        break;
                    case 1:
                        beverage = new Beverage("Energy Drink", randomNr, key);
                        break;
                    default:
                        beverage = new Beverage("Fejl", -1, key);
                        break;
                }
                _box.WriteAt($"{beverage.Key}|Produced Beverage: {beverage.Name}", ConsoleColor.Green);
                _queue.Produce(beverage);
                key++;
            }
        }
        public void Stop()
        {
            _running = false;
        }
    }
}
