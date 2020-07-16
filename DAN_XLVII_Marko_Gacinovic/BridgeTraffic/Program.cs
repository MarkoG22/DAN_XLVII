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
        private static ManualResetEvent mre = new ManualResetEvent(true);

        private static Dictionary<string, string> dict = new Dictionary<string, string>();

        

        static Random rnd = new Random();
        static int num = rnd.Next(1, 15);

        private static Thread[] threadArray = new Thread[num];

        static void Main(string[] args)
        {
            Delegate d = new Delegate();                        

            for (int i = 0; i < num; i++)
            {
                if (i%2==0)
                {                    
                    Thread t = new Thread(() => MovingSouth());
                    t.Name = "Vehicle_" + (i+1);
                    dict[t.Name] = "South";
                    threadArray[i] = t;
                }
                else
                {                    
                    Thread t = new Thread(() => MovingNorth());
                    t.Name = "Vehicle_" + (i + 1);
                    dict[t.Name] = "North";
                    threadArray[i] = t;
                }
            }            

            d.AllVehicles(dict);

            for (int i = 0; i < threadArray.Length; i++)
            {
                threadArray[i].Start();
            }

            Console.ReadLine();
        }

        static void MovingSouth()
        {
            
        }

        static void MovingNorth()
        {
            
        }
    }
}
