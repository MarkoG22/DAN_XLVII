using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace BridgeTraffic
{
    class Program
    {
        // ManualResetEvent for crossing the bridge
        private static ManualResetEvent mre = new ManualResetEvent(false);

        // locker and variable for taking first direction to go
        private static readonly object locker = new object();
        private static string direction = null;

        // dictionary for thread names and directions
        private static Dictionary<string, string> dict = new Dictionary<string, string>();

        // random number for number of vehicles
        static Random rnd = new Random();
        static int num = rnd.Next(1, 15);

        // barrier and array for starting all threads at the same time
        static Barrier barrier = new Barrier(num);  
        private static Thread[] threadArray = new Thread[num];

        // CountdownEvent for waiting all threads to finish
        private static CountdownEvent countdown = new CountdownEvent(num);

        static void Main(string[] args)
        {
            Stopwatch s = new Stopwatch();

            // starting the time
            s.Start();

            // object for calling the delegate
            Delegate d = new Delegate();            

            // loop for creating threads and taking thread names and directions into the dictionary
            for (int i = 0; i < num; i++)
            {
                // random num for thread directions
                int random = rnd.Next(2);

                Thread t = new Thread(() => Moving());

                threadArray[i] = t;
                t.Name = "Vehicle_" + (i + 1);

                // loop for thread directions
                if (random%2==0)
                {       
                    dict[t.Name] = "South";                    
                }
                else
                {     
                    dict[t.Name] = "North";
                }
            }            

            // calling the delegate
            d.AllVehicles(dict);

            // starting the threads
            for (int i = 0; i < threadArray.Length; i++)
            {
                threadArray[i].Start();
            }

            // waiting all threads to finish
            countdown.Wait();

            // stopping the time
            s.Stop();

            Console.WriteLine("\nTotal time elapsed: {0} ms", s.ElapsedMilliseconds);

            Console.ReadLine();

            
        }

        /// <summary>
        /// method for moving the vehicles across the bridge
        /// </summary>
        static void Moving()
        {
            // locker to determinate first direction
            lock (locker)
            {
                bool side = true;                

                if (side)
                {
                    direction = dict[Thread.CurrentThread.Name];
                    side = false;
                }
            }

            // barrier for waiting all threads
            barrier.SignalAndWait();

            // loop for moving vehicles in alternately directions
            if (dict[Thread.CurrentThread.Name] == direction)
            {
                Console.WriteLine("{0} is waiting to cross the bridge {1}.", Thread.CurrentThread.Name, direction);

                Console.WriteLine("===> {0} is crossing the bridge, moving {1}.", Thread.CurrentThread.Name, direction);

                Thread.Sleep(500);

                // signaling that bridge is available
                mre.Set();
            }
            else
            {
                Console.WriteLine("{0} is waiting to cross the bridge {1}.", Thread.CurrentThread.Name, dict[Thread.CurrentThread.Name]);

                // waiting the signal that bridge is available
                mre.WaitOne();

                Thread.Sleep(500);

                Console.WriteLine("===> {0} is crossing the bridge, moving {1}.", Thread.CurrentThread.Name, dict[Thread.CurrentThread.Name]);
            }

            // signaling the thread finishes for the stopwatch
            countdown.Signal();
        }
    }
}
