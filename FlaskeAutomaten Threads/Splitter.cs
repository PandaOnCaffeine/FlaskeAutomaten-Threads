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
                Thread.Sleep(100);
                Beverage beverage = _mainQueue.Next(_box);
                if (beverage.Id == 0)
                {
                    _box.WriteAt($"{beverage.Key}|Splitting {beverage.Name} To {_splitterBuffers[0].Name}", ConsoleColor.Yellow);
                    _mainQueue.Split(_splitterBuffers[0]);
                }
                else
                {
                    _box.WriteAt($"{beverage.Key}|Splitting {beverage.Name} To {_splitterBuffers[1].Name}", ConsoleColor.Yellow);
                    _mainQueue.Split(_splitterBuffers[1]);
                }
            }
        }
        public void Stop()
        {
            _running = false;
        }
    }
}
