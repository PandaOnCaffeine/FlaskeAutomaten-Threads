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
        static Queue<Beverage> _mainBuffer = new Queue<Beverage>();
        static Queue<Beverage> _beerBuffer = new Queue<Beverage>();
        static Queue<Beverage> _sodaBuffer = new Queue<Beverage>();

        static object _writeLock = new object();
        static void Main(string[] args)
        {
            Thread producerThread = new Thread(() => Producer());
            Thread splitterThread = new Thread(() => Splitter());
            Thread beerConsumerThread = new Thread(() => Consumer(_beerBuffer));
            Thread sodaConsumerThread = new Thread(() => Consumer(_sodaBuffer));

            producerThread.Start();
            splitterThread.Start();
            beerConsumerThread.Start();
            sodaConsumerThread.Start();

            Console.ReadLine();
        }
        static void Producer()
        {
            while (true)
            {
                try
                {
                    Monitor.Enter(_mainBuffer);
                    while (_mainBuffer.Count < 10)
                    {
                        if (new Random().Next(1, 3) == 1)
                        {
                            _mainBuffer.Enqueue(new Beverage("Beer", 1));
                            WriteAt($"New Beer produced", ConsoleColor.Green, 40, $"Main Buffer count: {_mainBuffer.Count}", ConsoleColor.DarkGreen);
                        }
                        else
                        {
                            _mainBuffer.Enqueue(new Beverage("Soda", 2));
                            WriteAt($"New Soda produced", ConsoleColor.Green, 40, $"Main Buffer count: {_mainBuffer.Count}", ConsoleColor.DarkGreen);

                        }
                        Thread.Sleep(200);
                    }
                    Monitor.PulseAll(_mainBuffer);
                }
                finally
                {
                    WriteAt($"Producer Waiting", ConsoleColor.Green, 40, "", ConsoleColor.DarkGreen);
                    Monitor.Wait(_mainBuffer);
                }
            }
        }
        static void Splitter()
        {
            while (true)
            {
                try
                {
                    Monitor.Enter(_mainBuffer);
                    while (_mainBuffer.Count > 0)
                    {
                        Beverage item = _mainBuffer.Dequeue();
                        if (item.TypeId == 1)
                        {
                            Monitor.Enter(_beerBuffer);
                            _beerBuffer.Enqueue(item);
                            WriteAt($"Beer Moved from Main buffer to Beer buffer", ConsoleColor.Yellow, 40, $"Main Buffer: {_mainBuffer.Count} ║ Beer Buffer: {_beerBuffer.Count}", ConsoleColor.DarkYellow);
                            Monitor.PulseAll(_beerBuffer);
                        }
                        else
                        {
                            Monitor.Enter(_sodaBuffer);

                            _sodaBuffer.Enqueue(item);
                            WriteAt($"Soda Moved from Main buffer to Soda buffer", ConsoleColor.Yellow, 40, $"Main Buffer: {_mainBuffer.Count} ║ Soda Buffer: {_sodaBuffer.Count}", ConsoleColor.DarkYellow);
                            Monitor.PulseAll(_sodaBuffer);
                        }
                        Thread.Sleep(200);
                    }
                    Monitor.PulseAll(_mainBuffer);
                }
                finally
                {
                    WriteAt($"Splitter Waiting", ConsoleColor.Yellow, 40, "", ConsoleColor.DarkGreen);
                    Monitor.Wait(_mainBuffer);
                }
            }

        }
        static void Consumer(Queue<Beverage> buffer)
        {
            while (true)
            {
                try
                {
                    Monitor.Enter(buffer);
                    if (buffer.Count == 0)
                    {
                        Console.WriteLine("Consumer");
                        Monitor.Wait(buffer);
                    }
                    while (buffer.Count > 0)
                    {
                        Console.WriteLine("sadasdasd");
                        Beverage item = buffer.Dequeue();
                        if (item.TypeId == 1)
                        {
                            _beerBuffer.Enqueue(item);
                            WriteAt($"Beer Consumed from Beer buffer", ConsoleColor.Red, 40, $"Beer Buffer: {buffer.Count}", ConsoleColor.DarkRed);
                        }
                        else
                        {
                            _sodaBuffer.Enqueue(item);
                            WriteAt($"Soda Consumed from Soda buffer", ConsoleColor.Red, 40, $"Soda Buffer: {buffer.Count}", ConsoleColor.DarkRed);

                            WriteAt($"Soda Moved from Main buffer to Soda buffer", ConsoleColor.Yellow, 40, $"Main Buffer: {_mainBuffer.Count} ║ Soda Buffer: {_sodaBuffer.Count}", ConsoleColor.DarkYellow);
                        }
                        Thread.Sleep(200);
                    }
                    Monitor.PulseAll(buffer);
                }
                finally
                {
                    WriteAt($"Consumer Waiting", ConsoleColor.Green, 40, "", ConsoleColor.DarkGreen);
                    Monitor.Wait(buffer);
                }
            }
        }
        static void WriteAt(string startText, ConsoleColor startTextColor, int x, string otherText, ConsoleColor otherTextColor)
        {
            lock (_writeLock)
            {
                Console.ForegroundColor = startTextColor;
                Console.Write(startText);

                int currentY = Console.CursorTop;

                Console.SetCursorPosition(x, currentY);

                Console.Write("║ ");

                Console.ForegroundColor = otherTextColor;
                Console.Write(otherText);

                // Move to the next line
                Console.WriteLine();
                //Console.SetCursorPosition(0, currentY + 1);

                Console.ResetColor();
            }
        }
    }
}
