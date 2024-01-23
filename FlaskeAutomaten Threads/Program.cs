﻿using System;
using System.Collections.Generic;
using System.Threading;
namespace FlaskeAutomaten_Threads
{
    class Program
    {
        static Queue<Beverage> mainBuffer = new Queue<Beverage>();
        static Queue<Beverage> buffer1 = new Queue<Beverage>();
        static Queue<Beverage> buffer2 = new Queue<Beverage>();
        static object mainBufferLock = new object();
        static object buffer1Lock = new object();
        static object buffer2Lock = new object();

        static object _writeLock = new object();
        static Random rng = new Random();
        static void Main(string[] args)
        {
            // Create threads
            Thread producerThread = new Thread(Producer);
            Thread splitterThread = new Thread(Splitter);
            Thread consumerThread1 = new Thread(Consumer1);
            Thread consumerThread2 = new Thread(Consumer2);

            // Start threads
            producerThread.Start();
            splitterThread.Start();
            consumerThread1.Start();
            consumerThread2.Start();

            // Wait for threads to finish
            producerThread.Join();
            splitterThread.Join();
            consumerThread1.Join();
            consumerThread2.Join();

            Console.WriteLine("Program completed.");
            Console.ReadLine();
        }

        static void Producer()
        {
            Thread.Sleep(500);
            while (true)
            {
                lock (mainBufferLock)
                {
                    while (mainBuffer.Count > 0)
                    {
                        Write("Producer Waiting", ConsoleColor.DarkRed);
                        Monitor.Wait(mainBufferLock);
                        Write("Producer Starting", ConsoleColor.Blue);
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        Beverage beverage;
                        if (rng.Next(1, 3) == 1)
                        {
                            beverage = new Beverage("Beer", 1);
                        }
                        else
                        {
                            beverage = new Beverage("Soda", 2);
                        }
                        mainBuffer.Enqueue(beverage);
                        Write($"Producing item {beverage.Type} {i +1}", ConsoleColor.Green);
                        Thread.Sleep(300);
                    }
                    Monitor.Pulse(mainBufferLock);
                }
                Thread.Sleep(50);
            }
        }
        static void Splitter()
        {
            while (true)
            {
                lock (mainBufferLock)
                {
                    while (mainBuffer.Count < 1)
                    {
                        Write("Splitter Waiting", ConsoleColor.DarkRed);
                        Monitor.Wait(mainBufferLock);
                        Write("Splitter Starting", ConsoleColor.Blue);
                    }
                    while (mainBuffer.Count > 0)
                    {
                        Beverage beverage = mainBuffer.Dequeue();
                        if (beverage.TypeId == 1)
                        {
                            lock (buffer1Lock)
                            {
                                Write($"Splitting item {beverage.Type} to Beer buffer", ConsoleColor.Yellow);
                                buffer1.Enqueue(beverage);
                                Monitor.Pulse(buffer1Lock);
                            }
                        }
                        else
                        {
                            lock (buffer2Lock)
                            {
                                Write($"Splitting item {beverage.Type} to Soda buffer", ConsoleColor.Yellow);
                                buffer2.Enqueue(beverage);
                                Monitor.Pulse(buffer2Lock);
                            }
                        }
                        Thread.Sleep(300);
                    }
                    Monitor.Pulse(mainBufferLock);
                }
                Thread.Sleep(50);
            }
        }
        static void Consumer1()
        {
            while (true)
            {
                lock (buffer1Lock)
                {
                    while (buffer1.Count < 1)
                    {
                        Write("Consumer1 Waiting", ConsoleColor.DarkRed);
                        Monitor.Wait(buffer1Lock);
                        Write("Consumer1 Starting", ConsoleColor.Blue);

                    }
                    while (buffer1.Count > 0)
                    {
                        Beverage beverage = buffer1.Dequeue();
                        Write($"Consuming from Beer: {beverage.Type}", ConsoleColor.Red);
                        Thread.Sleep(300);
                    }
                }
                //Thread.Sleep(50);
            }
        }
        static void Consumer2()
        {
            while (true)
            {
                lock (buffer2Lock)
                {
                    while (buffer2.Count < 1)
                    {
                        Write("Consumer2 Waiting", ConsoleColor.DarkRed);
                        Monitor.Wait(buffer2Lock);
                        Write("Consumer2 Starting", ConsoleColor.Blue);
                    }
                    while (buffer2.Count > 0)
                    {
                        Beverage beverage = buffer2.Dequeue();
                        Write($"Consuming from Buffer 2: {beverage.Type}", ConsoleColor.Red);
                        Thread.Sleep(300);
                    }
                    Monitor.Pulse(buffer2Lock);
                }
                //Thread.Sleep(50);
            }
        }
        static void Write(string startText, ConsoleColor color)
        {
            lock (_writeLock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(startText);
            }
        }
    }
}