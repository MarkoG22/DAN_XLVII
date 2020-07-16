using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeTraffic
{
    class Delegate
    {
        public delegate void Vehicles();

        public event Vehicles OnVehicles;
        
        public void AllVehicles(Dictionary<string, string> dictionary)
        {
            OnVehicles += () =>
            {
                Console.WriteLine("Number of vehicles: {0}", dictionary.Count);
                int counter = 0;
                foreach (KeyValuePair<string, string> item in dictionary)
                {
                    Console.WriteLine("{0}. {1} - direction: {2}", ++counter, item.Key, item.Value);
                }
            };

            OnVehicles.Invoke();
        }
    }
}
