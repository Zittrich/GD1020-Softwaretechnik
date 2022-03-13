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

            //Beispielgraph
            GraphStructure<int> graphStructure = new GraphStructure<int>();
            GraphStructure<int>.Vertex<int> v0 = graphStructure.InsertVertex(0);
            GraphStructure<int>.Vertex<int> v1 = graphStructure.InsertVertex(1);
            GraphStructure<int>.Vertex<int> v2 = graphStructure.InsertVertex(2);
            GraphStructure<int>.Vertex<int> v3 = graphStructure.InsertVertex(3);
            GraphStructure<int>.Vertex<int> v4 = graphStructure.InsertVertex(4);
            graphStructure.ConnectVertices(v0, v1, 100);
            graphStructure.ConnectVertices(v0, v3, 50);
            graphStructure.ConnectVertices(v1, v2, 100);
            graphStructure.ConnectVertices(v1, v4, 250);
            graphStructure.ConnectVertices(v2, v4, 50);
            graphStructure.ConnectVertices(v3, v1, 100);
            graphStructure.ConnectVertices(v3, v4, 250);

            Console.Clear();

            Console.WriteLine("The following is ment for testing purposes only. The following example works on a pre-definded graph.\n" +
                              "Recommended numbers are based on functionality/useability, other inputs are potentially illogical.\n");

            int start;
            int end;

            try
            {
                Console.WriteLine("Start Vertex ID (recommended = 0):");
                start = int.Parse(Console.ReadLine());
                Console.WriteLine("End Vertex ID(recommended = 4):");
                end = int.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine("Please enter a number");
                throw;
            }

            Dijkstra<int> dijk = new Dijkstra<int>();
            dijk.PrintDijk(graphStructure, start, end);

            Console.WriteLine("Graph:");
            graphStructure.PrintGraph();

            Console.ReadKey();
        }
    }

}
