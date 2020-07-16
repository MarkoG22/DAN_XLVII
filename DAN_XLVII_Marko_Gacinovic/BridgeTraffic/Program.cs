using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BridgeTraffic
{
    class Program
    {
        private static ManualResetEvent mre = new ManualResetEvent(false);
        private static readonly object locker = new object();

        private static Dictionary<string, string> dict = new Dictionary<string, string>();

        static Random rnd = new Random();
        static int num = rnd.Next(1, 15);

        static Barrier barrier = new Barrier(num);

        private static string direction = null;

        private static Thread[] threadArray = new Thread[num];

        static void Main(string[] args)
        {
            Delegate d = new Delegate();            

            for (int i = 0; i < num; i++)
            {
                int random = rnd.Next(2);

                Thread t = new Thread(() => Moving());

                threadArray[i] = t;
                t.Name = "Vehicle_" + (i + 1);

                if (random%2==0)
                {       
                    dict[t.Name] = "South";                    
                }
                else
                {     
                    dict[t.Name] = "North";
                }
            }            

            d.AllVehicles(dict);

            for (int i = 0; i < threadArray.Length; i++)
            {
                threadArray[i].Start();
            }

            Console.ReadLine();
        }

        static void Moving()
        {
            lock (locker)
            {
                bool side = true;                

                if (side)
                {
                    direction = dict[Thread.CurrentThread.Name];
                    side = false;
                }
            }

            barrier.SignalAndWait();

            if (dict.Keys.Contains(Thread.CurrentThread.Name))
            {
                if (dict[Thread.CurrentThread.Name] == direction)
                {
                    Console.WriteLine("{0} is waiting to cross the bridge {1}.", Thread.CurrentThread.Name, direction);

                    Console.WriteLine("\n{0} is crossing the bridge, moving {1}.", Thread.CurrentThread.Name, direction);

                    Thread.Sleep(500);

                    mre.Set();
                }
                else
                {
                    Console.WriteLine("{0} is waiting to cross the bridge {1}.", Thread.CurrentThread.Name, dict[Thread.CurrentThread.Name]);

                    mre.WaitOne();

                    Thread.Sleep(500);

                    Console.WriteLine("\n{0} is crossing the bridge, moving {1}.", Thread.CurrentThread.Name, dict[Thread.CurrentThread.Name]);
                }
            }
        }
    }
}
