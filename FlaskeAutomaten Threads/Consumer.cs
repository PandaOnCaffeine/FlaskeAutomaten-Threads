using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlaskeAutomaten_Threads
{
    internal class Consumer
    {
        private Buffer _queue;
        private string _name;
        private TextBox _box;

        private bool _running = true;
        public Consumer(Buffer queue, string name, TextBox box)
        {
            _queue = queue;
            _name = name;
            _box = box;
        }

        public void Run()
        {
            while (_running)
            {
                while (_queue.Count > 0)
                {
                    Beverage beverage = _queue.Consume();
                    _box.WriteAt($"{beverage.Key}|{_name} Consumed {beverage.Name}", ConsoleColor.Red);
                    Thread.Sleep(250);
                }
                Thread.Sleep(250);
            }
        }
        public void Stop(bool alive)
        {
            while (alive)
            {

            }
            _running = false;
        }
    }
}
