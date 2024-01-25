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
        private TextBox _box;

        public Splitter(Buffer mainQueue, Buffer[] splitterQueues, TextBox box)
        {
            _mainQueue = mainQueue;
            _splitterBuffers = splitterQueues;
            _box = box;
        }

        public void Run()
        {
            while (_running)
            {
                if (_mainQueue.Count >= _mainQueue._limit)
                {
                    while (_mainQueue.Count != 0)
                    {
                        Beverage beverage = _mainQueue.Next();
                        if (beverage.Id == 0)
                        {
                            _box.WriteAt($"{beverage.Key}|Splitting {beverage.Name} To {_splitterBuffers[0].Name}", ConsoleColor.Yellow);
                            Thread.Sleep(10);
                            _mainQueue.Split(_splitterBuffers[0]);
                        }
                        else
                        {
                            _box.WriteAt($"{beverage.Key}|Splitting {beverage.Name} To {_splitterBuffers[1].Name}", ConsoleColor.Yellow);
                            Thread.Sleep(10);
                            _mainQueue.Split(_splitterBuffers[1]);
                        }
                        Thread.Sleep(250);
                    }
                    Thread.Sleep(250);
                }
            }
        }
        public void Stop()
        {
            _running = false;
        }
    }
}
