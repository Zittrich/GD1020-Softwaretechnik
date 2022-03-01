using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD1020_Softwaretechnik
{
    class GraphStructure<T>
    {
        internal Random random = new Random();

        private Dictionary<int, List<(int weight, int connectedVertex)>> ConnectionDicitionary;
        private List<Vertex<T>> VertexList;
        public GraphStructure(Dictionary<int, List<(int weight, int connectedVertex)>> connectionDicitionary, List<Vertex<T>> vertexList)
        {
            ConnectionDicitionary = connectionDicitionary;
            VertexList = vertexList;
        }

        protected internal class Vertex<T>
        {
            private T Information;
            private T ID;

            public Vertex(T information, T id)
            {
                Information = information;
                ID = id;
            }
        }

        static void Main(string[] args)
        {
        }

        public GraphStructure<T> GenerateGraph(int graphSize, int maximumNeighbors, int maximumWeight)
        {
            GraphStructure<T> graphStructure = new GraphStructure<T>(new Dictionary<int, List<(int weight, int connectedVertex)>>(), new List<Vertex<T>>());
            T vertexInformation = (T)(object) "placeholder";

            for (int i = 0; i <= graphSize; i++)
            {
                graphStructure.VertexList.Add(new Vertex<T>(vertexInformation, (T)(object) i));

                int thisNeighborAmount = random.Next(1, maximumNeighbors);
                List<(int, int)> thisList = new List<(int, int)>();

                for (int j = 0; j <= thisNeighborAmount; j++)
                {
                    thisList.Add((random.Next(1, maximumWeight + 1), random.Next(i, maximumNeighbors + 1)));
                }
                graphStructure.ConnectionDicitionary.Add(i, thisList);
            }

            return graphStructure;
        }

        public void MakeNull()
        {
            ConnectionDicitionary = null;
        }

        public void InsertVertex(TConstituent vertex)
        {

        }

        public void DeleteVertex(TConstituent vertex)
        {

        }

        public void ConnectVertices(TConstituent vertexA, TConstituent vertexB)
        {

        }

        public void DisconnectVertices(TConstituent vertexA, TConstituent vertexB)
        {

        }
    }
}
