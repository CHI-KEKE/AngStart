using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class testing
    {


    private static object lock1 = new object();
    private static object lock2 = new object();

    public static void Main()
    {
        Thread thread1 = new Thread(() =>
        {
            lock (lock1)
            {
                Console.WriteLine("Thread 1 acquired lock1.");
                Thread.Sleep(100); // Simulate some work

                lock (lock2)
                {
                    Console.WriteLine("Thread 1 acquired lock2.");
                }
            }
        });

        Thread thread2 = new Thread(() =>
        {
            lock (lock2)
            {
                Console.WriteLine("Thread 2 acquired lock2.");
                Thread.Sleep(100); // Simulate some work

                lock (lock1)
                {
                    Console.WriteLine("Thread 2 acquired lock1.");
                }
            }
        });

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        Console.WriteLine("Main thread finished.");
    }


    }
}