using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
 
namespace FlaskeAutomaten_Threads
{
    internal class Program
    {
        //static object _writeLock = new object();
        static void Main(string[] args)
        {
            Buffer mainBuffer = new Buffer("Main Buffer", 10);
            Buffer beerBuffer = new Buffer("Beer Buffer", 10);
            Buffer energyDrinkBuffer = new Buffer("EnergyDrink Buffer", 10);
            Buffer[] splitingBuffers = new Buffer[] { beerBuffer, energyDrinkBuffer };

            Producer producer = new Producer(mainBuffer);

            Splitter splitter = new Splitter(mainBuffer, splitingBuffers);

            Consumer alcoholic = new Consumer(beerBuffer);
            Consumer teenager = new Consumer(energyDrinkBuffer);



            Thread producerThread = new Thread(new ThreadStart(producer.Run));
            Thread splitterThread = new Thread(new ThreadStart(splitter.Run));
            Thread alcoholicThread = new Thread(new ThreadStart(alcoholic.Run));
            Thread teenagerThread = new Thread(new ThreadStart(teenager.Run));

            producerThread.Start();
            splitterThread.Start();
            alcoholicThread.Start();
            teenagerThread.Start();

            Console.ReadLine();

            producer.Stop();
            splitter.Stop();
            alcoholic.Stop();
            teenager.Stop();

            producerThread.Join();
            splitterThread.Join();
            alcoholicThread.Join();
            teenagerThread.Join();
        }
        // Writing method with color
        //static void Write(string startText, ConsoleColor color)
        //{
        //    lock (_writeLock)
        //    {
        //        Console.ForegroundColor = color;
        //        Console.WriteLine(startText);
        //    }
        //}
    }
}
