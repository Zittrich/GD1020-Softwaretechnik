using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD1020_Softwaretechnik
{
    public class Program
    {
        static void Main(string[] args)
        {
            GraphStructure<int> graphStructure = new GraphStructure<int>();
            graphStructure.GenerateRandomGraph(100, 3, 10, 5);
            graphStructure.PrintGraph();
            Console.WriteLine("Graph complete = " + graphStructure.checkComplete());
            Console.ReadLine();
        }
    }

}
