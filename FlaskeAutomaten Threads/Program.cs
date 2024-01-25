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
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetWindowPosition(0, 0);

            object lockObj = new object();


            TextBox producerBox = new TextBox(40, 10, 5, 5,lockObj);
            TextBox splitterBox = new TextBox(50, 10, 55, 5, lockObj);

            TextBox beerBox = new TextBox(40, 10, 5, 20, lockObj);
            TextBox energyBox = new TextBox(40, 10, 55, 20, lockObj);


            Buffer mainBuffer = new Buffer("Main Buffer", 10);
            Buffer beerBuffer = new Buffer("Beer Buffer", 10);
            Buffer energyDrinkBuffer = new Buffer("EnergyDrink Buffer", 10);

            Buffer[] splitingBuffers = new Buffer[] { beerBuffer, energyDrinkBuffer };

            Producer producer = new Producer(mainBuffer, producerBox);

            Splitter splitter = new Splitter(mainBuffer, splitingBuffers, splitterBox);

            Consumer alcoholic = new Consumer(beerBuffer, "Beer", beerBox);
            Consumer teenager = new Consumer(energyDrinkBuffer, "Energy Drink", energyBox);



            Thread producerThread = new Thread(new ThreadStart(producer.Run));
            Thread splitterThread = new Thread(new ThreadStart(splitter.Run));
            Thread alcoholicThread = new Thread(new ThreadStart(alcoholic.Run));
            Thread teenagerThread = new Thread(new ThreadStart(teenager.Run));

            producerThread.Start();
            splitterThread.Start();
            alcoholicThread.Start();
            teenagerThread.Start();

            Console.ReadKey();

            producer.Stop();
            splitter.Stop();
            producerThread.Join();
            splitterThread.Join();

            alcoholic.Stop(splitterThread.IsAlive);
            teenager.Stop(splitterThread.IsAlive);

            alcoholicThread.Join();
            teenagerThread.Join();

            Console.ReadLine();
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
