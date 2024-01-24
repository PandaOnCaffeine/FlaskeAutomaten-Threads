using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
 
namespace FlaskeAutomaten_Threads
{
    internal class Producer
    {
        private Buffer _queue;
        private bool _running = true;
        private Random _random = new Random();
        public Producer(Buffer queue)
        {
            _queue = queue;
        }
        public void Run()
        {
            while (_running)
            {
                if (_queue.Count <= 0)
                {

                    int amount = _queue._limit;
                    while (_queue.Count < _queue._limit)
                    {
                        Beverage beverage;
                        int randomNr = _random.Next(0, 2);
                        switch (randomNr)
                        {
                            case 0:
                                beverage = new Beverage("Beer", randomNr);
                                break;
                            case 1:
                                beverage = new Beverage("Energy Drink", randomNr);
                                break;
                            default:
                                beverage = new Beverage("Fejl", -1);
                                break;
                        }
                        _queue.Produce(beverage);
                    }
                }
                Thread.Sleep(300);
            }
        }
        public void Stop()
        {
            _running = false;
        }
    }
}
