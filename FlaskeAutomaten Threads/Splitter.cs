using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
 
namespace FlaskeAutomaten_Threads
{
    internal class Splitter
    {
        private Buffer _mainQueue;
        private Buffer[] _splitterBuffers;
        private bool _running = true;


        public Splitter(Buffer mainQueue, Buffer[] splitterQueues)
        {
            _mainQueue = mainQueue;
            _splitterBuffers = splitterQueues;
        }

        public void Run()
        {
            while (_running)
            {
                if (_mainQueue.Count >= _mainQueue._limit)
                {
                    while (_mainQueue.Count > 0)
                    {
                        Beverage beverage = _mainQueue.Pull();
                        if (beverage.Id == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"{beverage.Name} To {_splitterBuffers[0].Name}");
                            _mainQueue.Split(_splitterBuffers[0], beverage);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"{beverage.Name} To {_splitterBuffers[1].Name}");
                            _mainQueue.Split(_splitterBuffers[1], beverage);
                        }
                    }
                }
                Thread.Sleep(300);

                //Queue<Beverage> beverages = _mainQueue.Move();
                //Beverage[] beers = new Beverage[_splitterQueues[0]._limit];
                //Beverage[] energyDrinks = new Beverage[_splitterQueues[1]._limit];
                //Console.WriteLine("TTTTT");
                //int amount = _mainQueue.Count;
                //for (int i = 0; i < amount; i++)
                //{
                //    Beverage beverage = beverages[i];
                //    switch (beverage.Id)
                //    {
                //        case 0:
                //            beers.SetValue(beverage, i);
                //            break;
                //        case 1:
                //            energyDrinks.SetValue(beverage, i);
                //            break;
                //        default:
                //            Console.WriteLine($"Splitter Switch ERROR: Id {beverage.Id}, Name {beverage.Name}");
                //            break;
                //    }
                //}
            }
        }
        public void Stop()
        {
            _running = false;

        }
    }
}
