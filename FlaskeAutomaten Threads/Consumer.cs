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
        private bool _running = true;

        public Consumer(Buffer queue)
        {
            _queue = queue;
        }

        public void Run()
        {
            while (_running)
            {

                Thread.Sleep(300);
            }
        }
        public void Stop()
        {
            _running = false;

        }
    }
}
