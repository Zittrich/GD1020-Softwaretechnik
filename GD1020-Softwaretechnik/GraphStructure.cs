using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD1020_Softwaretechnik
{
    public class Program<T>
    {
    }

    public class GraphStructure<T>
    {
        static void Main(string[] args)
        {
            GraphStructure<T> graphStructure = new GraphStructure<T>(null, null);
            graphStructure = graphStructure.GenerateGraph(60, 7, 100);
            Console.WriteLine("Done");
            Console.ReadKey();
        }

        internal Random random = new Random();

        private Dictionary<int, List<(int weight, int connectedVertex)>> ConnectionDicitionary;
        private List<Vertex<T>> VertexList;
        public GraphStructure(Dictionary<int, List<(int weight, int connectedVertex)>> connectionDicitionary, List<Vertex<T>> vertexList)
        {
            ConnectionDicitionary = connectionDicitionary;
            VertexList = vertexList;
        }

        public class Vertex<TVert>
        {
            private readonly TVert Information;
            public  readonly int ID;

            public Vertex(TVert information, int id)
            {
                Information = information;
                ID = id;
            }
        }

        public GraphStructure<T> GenerateGraph(int graphSize, int maximumNeighbors, int maximumWeight)
        {
            GraphStructure<T> graphStructure = new GraphStructure<T>(new Dictionary<int, List<(int weight, int connectedVertex)>>(), new List<Vertex<T>>());
            T vertexInformation = (T)(object) "placeholder";

            for (int i = 0; i <= graphSize; i++)
            {
                graphStructure.VertexList.Add(new Vertex<T>(vertexInformation, i));

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
            VertexList = null;
            ConnectionDicitionary = null;
        }

        public void InsertVertex(int vertexID, T information)
        {
            if (ConnectionDicitionary.ContainsKey(vertexID))
                throw new ArgumentException("Vertex ID already exists");

            VertexList.Add(new Vertex<T>(information, vertexID));
            ConnectionDicitionary.Add(vertexID, new List<(int, int)>());
        }

        public void DeleteVertex(int vertexID)
        {
            if (!ConnectionDicitionary.ContainsKey(vertexID))
                throw new ArgumentException("Vertex ID does not exist");

            foreach(Vertex<T> vertex in VertexList)
            {
                if(vertex.ID == vertexID)
                {
                    VertexList.Remove(vertex);
                    break;
                }
            }

            ConnectionDicitionary.Remove(vertexID);

            foreach (KeyValuePair<int, List<(int,int)>> keyValuePair in ConnectionDicitionary)
            {
                foreach((int weight, int connectedVertex) valuePair in keyValuePair.Value)
                if (valuePair == (valuePair.weight, vertexID))
                {
                    ConnectionDicitionary.Remove(keyValuePair.Key);
                }
            }
        }

        public void ConnectVertices(int vertexA, int vertexB, int weight)
        {

        }

        public void DisconnectVertices(int vertexA, int vertexB, int weight)
        {

        }

        //changeVertexInformation

        //member_vertex
        //member_edge
        //edge_weight
        //mark_vertex
        //mark_edge
    }
}
