using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

//Cursor Parkplatz

namespace GD1020_Softwaretechnik
{
    public class Program
    {
        static void Main(string[] args)
        {
            GraphStructure graphStructure = new GraphStructure(null, null);
            graphStructure = graphStructure.GenerateRandomGraph(4, 3, 10);
            graphStructure.PrintGraph();
            Console.ReadKey();
        }
    }
    public class GraphStructure
    {
        internal Random random = new Random();

        //Dictionary mit T(zurück zu int?) und weight + connected Vert
        private Dictionary<int, List<(int weight, int connectedVertex)>> ConnectionDictionary;

        //Liste aller Vertices im Graphen
        private List<Vertex> VertexList;

        public GraphStructure(Dictionary<int, List<(int weight, int connectedVertex)>> connectionDictionary, List<Vertex> vertexList)
        {
            ConnectionDictionary = connectionDictionary;
            VertexList = vertexList;
        }

        public class Vertex
        {
            public  readonly int ID;

            public Vertex(int id)
            {
                ID = id;
            }
        }

        public void PrintGraph()
        {
            foreach (KeyValuePair<int, List<(int weight, int connectedVertex)>> keyValuePair in ConnectionDictionary)
            {
                Console.Write($"Vertex: {keyValuePair.Key} || ");
                foreach (var t in keyValuePair.Value)
                {
                    Console.Write($"[Weight: {t.weight}, Vertex: {t.connectedVertex}];  ");
                }
                Console.WriteLine();
            }
        }

        public GraphStructure GenerateRandomGraph(int graphSize, int maximumNeighbors, int maximumWeight)
        {
            GraphStructure graphStructure = new GraphStructure(new Dictionary<int, List<(int weight, int connectedVertex)>>(), new List<Vertex>());
            for (int i = 0; i < graphSize; i++)
            {
                graphStructure.VertexList.Add(new Vertex(i));

                bool isDirty = false;
                int thisNeighborAmount = random.Next(1, maximumNeighbors);
                List<(int weight, int connectedVertex)> thisList = new List<(int, int)>();
                for (int j = 0; j <= thisNeighborAmount; j++)
                {
                    int weightRandom = random.Next(1, maximumWeight);
                    int connectedRandomVertex = random.Next(i, Math.Min(maximumNeighbors + i, graphSize));

                    for (int k = 0; k < thisList.Count; k++)
                    {
                        if (thisList[k] == (thisList[k].weight, connectedRandomVertex))
                            isDirty = true;
                    }
                    if(!isDirty)
                        thisList.Add((weightRandom, connectedRandomVertex));

                    isDirty = false;
                }
                graphStructure.ConnectionDictionary.Add(i, thisList);
            }

            return graphStructure;
        }

        public void MakeNull()
        {
            VertexList = null;
            ConnectionDictionary = null;
        }

        public void InsertVertex(int vertexID)
        {
            if (ConnectionDictionary.ContainsKey(vertexID))
                throw new ArgumentException("Vertex ID already exists");

            VertexList.Add(new Vertex(vertexID));
            ConnectionDictionary.Add(vertexID, new List<(int, int)>());
        }

        public void DeleteVertex(int vertexID)
        {
            if (!ConnectionDictionary.ContainsKey(vertexID))
                throw new ArgumentException("Vertex ID does not exist");

            foreach(Vertex vertex in VertexList)
            {
                if(vertex.ID == vertexID)
                {
                    VertexList.Remove(vertex);
                    break;
                }
            }

            ConnectionDictionary.Remove(vertexID);

            foreach (KeyValuePair<int, List<(int,int)>> keyValuePair in ConnectionDictionary)
            {
                foreach((int weight, int connectedVertex) valuePair in keyValuePair.Value)
                {
                    if (valuePair == (valuePair.weight, vertexID))
                    {
                        ConnectionDictionary.Remove(keyValuePair.Key);
                    }
                }
                //for schleife statt for each
            }
        }

        public void ConnectVertices(int vertexA, int vertexB, int weight)
        {
            if (VertexList.Count < vertexA || VertexList.Count < vertexB)
                throw new IndexOutOfRangeException("At least one of the given Vertices does not exist");
            if (ConnectionDictionary[vertexA].Contains((vertexB, weight)))
                throw new ArgumentException("Connection already exists");

            ConnectionDictionary[vertexA].Add((vertexB, weight));
        }

        public void DisconnectVertices(int vertexA, int vertexB, int weight)
        {
            if (VertexList.Count < vertexA || VertexList.Count < vertexB)
                throw new IndexOutOfRangeException("At least one of the given Vertices does not exist");
            if (!ConnectionDictionary[vertexA].Contains((vertexB, weight)))
                throw new ArgumentException("Connection does not exist");

            ConnectionDictionary[vertexA].Remove((vertexB, weight));
        }

        //changeVertexInformation

        //member_vertex
        //member_edge
        //edge_weight
        //mark_vertex
        //mark_edge
    }
}
